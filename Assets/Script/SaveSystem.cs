using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Singelton 
    public static SaveSystem Instance { get; private set; }

    private const string PREF_WORLD = "worldUnlock";
    Int32 worldUnlock;
    Int32 restartInteraction;

    void Awake()
    {
        #region Singelton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }
    private void Start()
    {
        worldUnlock = PlayerPrefs.GetInt(PREF_WORLD, 0);
        if (worldUnlock == 0)
        {
            PlayerPrefs.SetInt(PREF_WORLD, 1);
            worldUnlock = 1;
        }
    }

    public void LvComplete()
    {
        worldUnlock++;
        PlayerPrefs.SetInt(PREF_WORLD, worldUnlock);
    }
}
