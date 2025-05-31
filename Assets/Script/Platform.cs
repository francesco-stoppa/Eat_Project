using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Singelton
    GameManager gameManager;
    // Position list
    // Transform[] boxes;
    List<Transform> boxes = new List<Transform>(); // <-- to find the box
    List<Transform> stairs = new List<Transform>();

    Transform playerPos;
    List<GameObject> doors = new List<GameObject>(); // <-- to see the doors

    // [SerializeField] GameObject[] doors;
    Vector3 boxPos = new Vector3 (0, 0.4f, 0);

    private void Start()
    {
        // Singelton
        gameManager = GameManager.Instance;
        // Fill the list
        boxes = gameManager.GetBoxes();
        stairs = gameManager.GetStairs();
        playerPos = gameManager.GetPlayer();
        foreach (Transform child in transform)
        {
            doors.Add(child.gameObject);
        }
        Check();
    }

    public void OpenDoor()
    {
        foreach(GameObject d in doors)
            d.SetActive(false);
    }
    public void CloseDoor()
    {
        foreach (GameObject d in doors)
            d.SetActive(true);
    }
    public void Check()
    {
        int press = 0;
        if (playerPos.position == transform.position - Vector3.up / 2)
        {
            OpenDoor();
            press++;
        }
        foreach (Transform b in boxes)
        {
            if (b.position == transform.position + Vector3.up / 2)
            {
                OpenDoor();
                press++;
            }
        }
        foreach (Transform s in stairs)
        {
            if (s.position == transform.position + Vector3.up / 2)
            {
                OpenDoor();
                press++;
            }
        }
        if (press == 0)
            CloseDoor();
    }
}
