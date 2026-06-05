using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public virtual void Enter(Bob bob)
    {
        //...
    }
    public virtual bool CanEnter(Bob bob)
    {
        return false;
    }
}
