using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    InputOnGrid _input;
    Box eatenObject;
    Box currentTile;

    [Tooltip("This variable determines the distance between \nthe center of one box and another.")]
    [SerializeField] float distanceCheck;

    void Start()
    {
        BindEvents();
        if (distanceCheck > 0) return;
        
        Debug.LogWarning($"The [Distance Check] value is 0 (or less)." + 
            $"\nCurrent value: <{distanceCheck}>.");
    }

    Box SomethingAhead()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        if (!Physics.Raycast(origin, transform.forward, out hit, distanceCheck)) return null;
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead)) return null;
        return objectAhead;
    }

    void Move(E_Direction direction)
    {
        if (direction == E_Direction.None) return;

        // Rotate 
        switch (direction)
        {
            case E_Direction.Left:
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case E_Direction.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case E_Direction.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case E_Direction.Down:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }

        // check if there is a  object ahead
        Box objectAhead = SomethingAhead();
        // check if it can pass through
        if (objectAhead != null)
            if (!objectAhead.CanPassThrough(this)) return;

        // se sono su una piattaforma
        if (CheckIfWillFallAfterStepOut()) return;

        // Move
        transform.position += transform.forward;

        if(currentTile == null)
        {
            Debug.LogError("The [Pawn] sand on nothing.");
            return;
        }
        currentTile.SetpOut(this);


        RaycastHit hit;

        if (!Physics.Raycast(transform.position, -transform.up, out hit, distanceCheck))
        {
            Debug.LogError("The [Pawn] sand on nothing.");
            return;
        }
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box standBox))
        {
            Debug.LogError("The [Pawn] sand on nothing.");
            return;
        }
        standBox.SetpOn(this);
        currentTile = standBox;
    }
    bool CheckIfWillFallAfterStepOut()
    {
        RaycastHit hit;
        Vector3 origin = transform.position - transform.up;

        // check if there is a Box
        if (Physics.Raycast(origin, transform.forward, out hit, distanceCheck)) return false;

        // check if the Box hit can be walkable
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead)) return false;
        return objectAhead.walkable;
    }



    #region Stomach System
    void SplitAndEat(E_Commands commandRecive)
    {
        if (commandRecive != E_Commands.EatOrSplit) return;
        Box objectAhead;

        // Split
        if (eatenObject != null)
        {
            objectAhead = SomethingAhead();
            if (objectAhead != null) return; // << cant split

            eatenObject.Split(transform.position + transform.forward);
            eatenObject = null;
            return;
        }

        // Eat
        objectAhead = SomethingAhead();
        if (objectAhead == null) return;
        if (!objectAhead.eatable) return;

        eatenObject = objectAhead;
        eatenObject.Eat(this);
    }

    public bool FullStomach()
    {
        if (eatenObject == null) return false;
        if (eatenObject.weight == E_ObjectWeight.small) return false;
        return true;
    }
    #endregion

    #region Event System
    private void BindEvents()
    {
        _input = InputOnGrid.Instance;
        if (_input == null)
        {
            Debug.LogError("The [input manager] is missing");
            return;
        }
        _input.onTakeDirection += Move;
        _input.onCommands += SplitAndEat;
    }
    private void OnDisable()
    {
        if (_input == null) return;
        _input.onTakeDirection -= Move;
        _input.onCommands -= SplitAndEat;
    }
    #endregion
}

