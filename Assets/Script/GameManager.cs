using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singelton 
    public static GameManager Instance { get; private set; }
    // GameManager child & their order
    Transform winColumns;
    Transform player;
    Transform map;
    Transform boxContainer;
    Transform platformContainer;
    Transform limitContainer;
    Transform stair;

    // List to fill & share
    List<Transform> boxes = new List<Transform>();
    List<Platform> platformScript = new List<Platform>();
    List<Transform> platformsPos = new List<Transform>();
    List<Transform> limits = new List<Transform>();
    List<Object> objectScript = new List<Object>();
    List<Transform> stairs = new List<Transform>();


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
        #region GetComponent
        winColumns = transform.GetChild(0);
        player = transform.GetChild(1);
        map = transform.GetChild(2);
        boxContainer = transform.GetChild(3);
        platformContainer = transform.GetChild(4);
        limitContainer = transform.GetChild(5);
        stair = transform.GetChild(6);

        #endregion
        #region ListFiller
        // Fill boxes list
        foreach (Transform b in boxContainer)
        {
            boxes.Add(b);
        }
        if (boxes != null)
        {
            foreach (Transform t in boxes)
            {
                Object o = t.GetComponent<Object>();
                objectScript.Add(o);
            }
        }

        // Fill platformsPos list
        foreach (Transform p in platformContainer)
        {
            platformsPos.Add(p);
        }
        if (platformsPos != null)
        {
            foreach (Transform ps in platformsPos)
            {
                Platform pl = ps.GetComponent<Platform>();
                platformScript.Add(pl);
            }
        }
        // Fill limits list
        foreach (Transform l in limitContainer)
        {
            limits.Add(l);
        }
        // Fill stairs
        foreach(Transform s in stair)
        {
            stairs.Add(s);
        }
        #endregion
    }
    public void Check()
    {
        if (boxes != null)
        {
            foreach (Object o in objectScript)
            {
                o.Fall();
            }
        }

        if (platformScript != null)
        {
            foreach (Platform p in platformScript)
            {
                p.Check();
            }
        }

        if (stairs != null)
        {
            foreach (Transform s in stairs)
            {
                if (s.position == player.position + Vector3.up)
                {
                    player.position = s.position;
                }
            }
        }
    }
    #region Getter

    public Transform GetWinColumns() { return winColumns; }
    public Transform GetPlayer() { return player; }
    public Transform GetMap() { return map; }
    public List<Transform> GetBoxes() { return boxes; }
    public List<Transform> GetLimits() { return limits; }
    public List<Transform> GetStairs() { return stairs; }

    #endregion
}

