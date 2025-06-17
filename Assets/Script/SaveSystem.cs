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
    Int32 currentLevel = 1;

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
            PlayerPrefs.SetInt(PREF_WORLD, 0);
            worldUnlock = 0;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LvComplete(Int32 newWorld)
    {
        worldUnlock = newWorld;
        PlayerPrefs.SetInt(PREF_WORLD, worldUnlock);
    }
    public int GetWorld()
    { return worldUnlock; }
    public int GetLevel()
    { return currentLevel; }
    public void SetLevel(int level)
    { currentLevel = level; }
}
