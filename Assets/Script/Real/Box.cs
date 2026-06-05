using UnityEngine;

public class Box : MonoBehaviour
{
    [Tooltip("Small object dont make you fat")]
    public E_ObjectWeight weight;
    [Tooltip("Bob ca walk on this object?")]
    public bool walkable = true;
    [Tooltip("Bob ca eat this object?")]
    public bool eatable = true; 

    void Awake()
    {
        if (weight == E_ObjectWeight.none)
            Debug.LogError($"The obstacle: <{gameObject.name}> does not has a [weight].");
    }

    #region Eat System
    public virtual void Eat(Pawn pawn)
    {
        if (!eatable) return; // << Temp

        this.gameObject.SetActive(false);


        if (weight == E_ObjectWeight.levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        if (!Physics.Raycast(origin, Vector3.down, out hit, 1)) return;
        GameObject objectHit = hit.collider.gameObject;

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        obstacle.SetpOut();
    }

    public virtual void Split(Vector3 newPosition)
    {
        this.gameObject.transform.position = newPosition;
        FreeFall();
        this.gameObject.SetActive(true);
    }
    #endregion

    #region Walkable System
    public virtual void SetpOn(Pawn pawn)
    {
        // ...
    }

    public virtual void SetpOut(Pawn pawn)
    {
        // ...
    }
    #endregion

    #region Physic System

    #region Temp-Update()
    private void Update()
    {
        if (gameObject.transform.position.y == 0
            || weight == E_ObjectWeight.levitate) return;

        FreeFall();
    }
    #endregion

    void FreeFall()
    {
        if (weight == E_ObjectWeight.levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        Vector3 newPosition = new Vector3(origin.x, 0, origin.z);
        this.transform.position = newPosition;


        if (!Physics.Raycast(origin, Vector3.down, out hit, origin.y)) return;
        GameObject objectHit = hit.collider.gameObject;

        newPosition += transform.up * objectHit.transform.position.y + transform.up;
        this.transform.position = newPosition;

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        obstacle.SetpOn();
    }

    public virtual bool CanPassThrough(Pawn pawn)
    {
        return false;
    }
    #endregion
}
