using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDestroyer : MonoBehaviour
{
    public Component myComponent;
    public bool destroyMe;
    //public Time timeToDestroy = 0;

    void Update() 
    {
        if(destroyMe == true)
        {
            Destroy(GetComponent<MeshRenderer>());
        }
    }
}
