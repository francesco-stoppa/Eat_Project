using UnityEngine;

public class Bob : MonoBehaviour
{
    InputOnGrid input;
    Obstacle goEat;

    [SerializeField] float distanceCheck;

    [SerializeField] bool canSplitInTunnel;

    GameObject currentTile;

    public bool Fat()
    {
        if (goEat == null) return false;
        if (goEat.weight == E_ObjectWeight.small)
            return false;

        return true;
    }

    void Start()
    {
        input = InputOnGrid.Instance;
        if(input == null)
        {
            Debug.LogError("The [input manager] is missing");
            return;
        }
        input.onTakeDirection += Move;
        input.onCommands += Eat;

        if (distanceCheck <= 0)
            Debug.LogWarning($"The [Distance Check] value is 0 (or less)." +
                $"\nCurrent value: <{distanceCheck}>.");
    }

    GameObject HaveSomethingAhead(bool eatCheck = false, bool checkFromStair = false)
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        if (checkFromStair)
            origin -= Vector3.up;

        if (!Physics.Raycast(origin, transform.forward, out hit, distanceCheck))
            return null;
        
        // string tag = hit.collider.tag;
        switch (hit.collider.tag)
        {
            case "Invisible":
                return null;
            case "Stair":
                if (eatCheck)
                    break;

                if (Physics.Raycast(origin, -transform.up, out hit, distanceCheck))
                {
                    GameObject go = hit.collider.gameObject;
                    if(go != null)
                    {
                        if(go.TryGetComponent<Obstacle>(out Obstacle o))
                        {
                            o.SetpOut(this);
                        }
                    }
                }
                transform.position += Vector3.up;
                return null;
        }

        return hit.collider.gameObject;
    }
    bool CheckIfFall()
    {
        RaycastHit hit;

        Vector3 origin = transform.position - transform.up;
        if (Physics.Raycast(origin, transform.forward, out hit, distanceCheck))
            return false;
        return true;
    }
    

    #region Events
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

        // need to chek the current tile (stair and wall)
        /* first try
        GameObject lastTile;
        RaycastHit hitto;

        if (Physics.BoxCast(transform.position + transform.forward, Vector3.one * 0.5f, transform.forward, out hitto, Quaternion.identity, 1))
        {
            Debug.Log($"i hit: {hitto.collider.gameObject}");
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hitto.collider.gameObject.transform.position, Quaternion.identity);
        }*/

        RaycastHit hit;
        GameObject lastTile = null;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceCheck))
        {
            // Debug.Log($"Hit: <{hit.collider.gameObject.name}>");
            lastTile = hit.collider.gameObject;

            // if stair
            // creo una funzione su stair che chiede la direzione e controlla se ci sono
            // ostacoli nella mia direzione -> SI, controllo se è un ostacolo calpestabile -> SI, return (nel chill)
            // L-> NO, abbasso bob e continuo con la funzione.                          L-> NO, blocco il movimento
            // L -> NO, BUT non c'è il pavimento, blocco il movimento
        }

        // Check for stairs
        /* if (transform.position.y > 1)
        {
            RaycastHit hit;

            bool isOnStair = Physics.Raycast(transform.position, Vector3.down, out hit, distanceCheck) && 
                hit.collider.CompareTag("Stair");

            if (isOnStair && CheckIfFall() && HaveSomethingAhead(false, true) == null &&
                Physics.Raycast(transform.position + transform.forward, Vector3.down, out hit, distanceCheck * 2))
            {
                transform.position += transform.forward;
                transform.position += Vector3.down;
                return;
            }
        }*/

        GameObject go = HaveSomethingAhead();
        if (go != null)
        {
            if (go.TryGetComponent<Door>(out Door ob))
            {
                if (!ob.CanEnter(this)) return;
                /* v
                ob.Enter(this);
                RaycastHit oo;
                if (Physics.Raycast(transform.position, -transform.up, out oo, distanceCheck))
                {
                    GameObject ooGo = oo.collider.gameObject;
                    if (ooGo.TryGetComponent<Tile>(out Tile to))
                        to.SetpOn(this);
                } ^ */
            }
            // > return;
        }

        if (CheckIfFall()) return;

        // add for Special Tile
        RaycastHit hot;
        if (Physics.Raycast(transform.position, -transform.up, out hot, distanceCheck))
        {
            GameObject hitGo = hot.collider.gameObject;
            if (hitGo.TryGetComponent<Obstacle>(out Obstacle tile))
                tile.SetpOut(this);
        }    

        // Move
        transform.position += transform.forward;
        // Add
        if(lastTile != null)
            if (lastTile.TryGetComponent<Obstacle>(out Obstacle tole))
                tole.SetpOut(this);
        // Added

        if (Physics.Raycast(transform.position, -transform.up, out hot, distanceCheck))
        {
            GameObject hotGo = hot.collider.gameObject;
            if (hotGo.TryGetComponent<Obstacle>(out Obstacle tole))
                tole.SetpOn(this);
        }
    }
    void Eat(E_Commands commandRecive)
    {
        if (commandRecive != E_Commands.EatOrSplit) return;
         
        // Split
        if(goEat != null)
        {
            if (HaveSomethingAhead(true) != null) return; // << cant split

            // vv split
            goEat.Split(transform.position + transform.forward);
            goEat = null;
            return;
        }


        // Eat
        GameObject obstacleFound = HaveSomethingAhead(true);
        if (obstacleFound == null) return;
        if (!obstacleFound.TryGetComponent<Obstacle>(out Obstacle obstacleEat)) return;

        goEat = obstacleEat;
        goEat.Eat();
    }

    private void OnDisable()
    {
        if (input == null) return;
        input.onTakeDirection -= Move;
        input.onCommands -= Eat;
    }
    #endregion

    // riceve input di movimento
    // raycast davanti a lui (free? or dislivello) 
    // check grid (the next tile exist?)
    // no - check se è su un dislivello
}

