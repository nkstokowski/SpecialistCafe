using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [SerializeField]
    private float saveIntervalSeconds = 10f;
    private float timer = 0f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Data Persistance Manager in the scene.");
        }

        instance = this;
    }

    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= saveIntervalSeconds)
        {
            SaveGame();
            timer = 0f;
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    public void NewGame() {
        this.gameData = new GameData();
    }

    public void LoadGame() {
        this.gameData = dataHandler.Load();

        if (this.gameData == null) {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        // push the loaded data to all other scripts
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame() {
        // pass the data to other scripts
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // save that data to a file using data handler
        dataHandler.Save(this.gameData);
    }

    public void SaveComponent(String component)
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            if (component == dataPersistenceObj.gameObject.name)
            {
                dataPersistenceObj.SaveData(ref gameData);
                dataHandler.Save(this.gameData);
            }
        }
    }
}
