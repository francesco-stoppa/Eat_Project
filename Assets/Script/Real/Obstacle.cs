using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.UI.Image;

public class Obstacle : MonoBehaviour
{
    [Tooltip("Small object dont make you fat")]
    public E_ObjectWeight weight;

    void Awake()
    {
        if (weight == E_ObjectWeight.Not_Set)
            Debug.LogError($"The obstacle: <{gameObject.name}> does not has a [weight].");
    }

    // TEMP vv
    private void Update()
    {
        if (gameObject.transform.position.y == 0
            || weight == E_ObjectWeight.Levitate) return;

        Fall();
    }
    // TEMP ^^

    public virtual void Eat()
    {
        //...
        this.gameObject.SetActive(false);


        if(weight == E_ObjectWeight.Levitate) return;

        RaycastHit hit;
        Vector3 origin = transform.position;

        if (!Physics.Raycast(origin, Vector3.down, out hit, 1)) return;
        GameObject objectHit = hit.collider.gameObject;

        if (!objectHit.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        obstacle.SetpOut();
    }

    public virtual void Split(Vector3 newPosition)
    {
        //...
        this.gameObject.transform.position = newPosition;
        Fall();
        this.gameObject.SetActive(true);
    }


    void Fall()
    {
        if (weight == E_ObjectWeight.Levitate) return;

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

    public virtual void SetpOn(Bob bob = null)
    {
        // ...
    }

    public virtual void SetpOut(Bob bob = null)
    {
        // ...
    }
}
