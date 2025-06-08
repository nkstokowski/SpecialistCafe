using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public String[] clipNames;
    public AudioClip[] clips;
    Dictionary<String, AudioClip> soundDict;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        soundDict = new Dictionary<String, AudioClip>();
        int maxClipsIndex = Math.Min(clipNames.Length, clips.Length);
        for (int i = 0; i < maxClipsIndex; i++)
        {
            soundDict.Add(clipNames[i], clips[i]);
        }
    }

    public void PlaySound(String soundName)
    {
        audioSource.clip = soundDict[soundName];
        audioSource.Play();
    }
}
