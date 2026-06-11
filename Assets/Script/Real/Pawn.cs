using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Pawn : MonoBehaviour
{
    InputOnGrid _input;
    Box eatenObject;
    Box currentTile;

    [Tooltip("This variable determines the distance between \nthe center of one box and another.")]
    [SerializeField] float distanceCheck;
    public float DistantCheck => distanceCheck;

    void Awake()
    {
        BindEvents();

        if (distanceCheck < 0)
            Debug.LogWarning($"The [Distance Check] value is 0 (or less)." +
                $"\nCurrent value: <{distanceCheck}>.");

        SetpOnNewTile();
    }

    #region Base System
    Box SomethingAhead()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        if (!Physics.Raycast(origin, transform.forward, out hit, distanceCheck)) return null;
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead)) return null;
        return objectAhead;
    }
    #endregion

    #region Movement System
    void Move(E_Direction direction)
    {
            // 1. Recive input
        if (direction == E_Direction.Not_Set) return;

            // 2. Rotate 
        switch (direction)
        {
            case E_Direction.Back:
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case E_Direction.Forward:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case E_Direction.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case E_Direction.Right:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            /* Old rotation
            case E_Direction.Left:
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case E_Direction.Right:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case E_Direction.Forward:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case E_Direction.Back:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;*/
        }

            // 3. Check forward
        Box objectAhead = SomethingAhead();
        // check if it can pass through
        if (objectAhead != null)
        {
            if (objectAhead.HasPassThroughTeleportation(this))
            {
                SetpOnNewTile();
                return;
            }
        }

            // 4. Check tile 
        // if can not step out you will get an error
        if (currentTile == null)
        {
            Debug.LogError("The [Pawn] sand on nothing.");
            return;
        }

        if (currentTile.HasSetpOutTeleportation(this))
        {
            SetpOnNewTile();
            return;
        }

            // 5. Check next tile
        // check nex tile only if you ar not enter into a portal
        if (CheckIfWillFallAfterStepOut()) return;

            // 6. Move
        currentTile.SetpOut(this);
        transform.position += transform.forward;
        SetpOnNewTile();
    }

    void SetpOnNewTile()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;

        if (!Physics.Raycast(origin, -transform.up, out hit, distanceCheck))
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

        // repositioning
        transform.position = Vector3Int.RoundToInt(transform.position);
    }
    bool CheckIfWillFallAfterStepOut()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + transform.forward;

        // check if there is a Box
        if (!Physics.Raycast(origin, -transform.up, out hit, distanceCheck)) return true;

        // check if the Box hit can be walkable
        if (!hit.collider.gameObject.TryGetComponent<Box>(out Box objectAhead)) return true;
        if (objectAhead.walkable) return false;

        return true;
    }
    #endregion

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

    public bool HaveFullStomach()
    {
        if (eatenObject == null) return false;
        if (eatenObject.weight == E_ObjectWeight.Small) return false;
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

