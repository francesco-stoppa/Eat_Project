using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeWorld : MonoBehaviour
{
    bool isMoving = false;
    WorldSelection worldSelection = null;
    [SerializeField]
    float speed;
    float left_limit = -8.5f;
    float right_limit = 8.5f;
    float start_position_x = 0;
    bool limit_reached = false;
    Vector3 moving;
    int direction = 0;
    
    public void SetScript(WorldSelection ws)
    {
        worldSelection = ws;
    }
    public void StartAnim(bool right)
    {
        isMoving = true;
        if (right)
        { moving = new Vector3(-1, 0, 1); }
        else
        { moving = new Vector3(1, 0, -1); }
    }

    void Update()
    {
        if(isMoving)
        {
            transform.position += moving * speed * Time.deltaTime;

            if (transform.position.x <= left_limit && moving.z >= 0)
            {
                transform.position = new Vector3(right_limit, 3.5f, left_limit);
                limit_reached = true;
            }

            if (transform.position.x <= start_position_x && limit_reached && moving.z >= 0)
            {
                isMoving = false;
                limit_reached = false;
                transform.position = Vector3.up * 3.5f;

                if (worldSelection)
                {
                    worldSelection.UnlockInput();
                }
            }

            if (transform.position.x >= right_limit && moving.z <= 0)
            {
                transform.position = new Vector3(left_limit, 3.5f, right_limit);
                limit_reached = true;
            }

            if (transform.position.x >= start_position_x && limit_reached && moving.z <= 0)
            {
                isMoving = false;
                limit_reached = false;
                transform.position = Vector3.up * 3.5f;
                moving = Vector3.zero;

                if (worldSelection)
                {
                    worldSelection.UnlockInput();
                }
            }
        }
    }
}
