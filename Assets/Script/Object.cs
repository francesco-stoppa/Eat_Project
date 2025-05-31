using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ObjectType { eatable, cube, splittable, tile, spicy, spine, scale }

public class Object : MonoBehaviour
{
    [SerializeField] ObjectType objectType;
    
    public ObjectType GetObjectType() { return objectType; }

    public void Fall()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitFloor, 1))
            transform.position += Vector3.down;
    }
    
}
