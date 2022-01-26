using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZOnBirth : MonoBehaviour
{
    private Vector3 newPosition;
    private float posX, posY;
    public float posZ;
    void Start()
    {
        posX = gameObject.transform.position.x;
        posY = gameObject.transform.position.y;
        newPosition = new Vector3(posX, posY, posZ);
        gameObject.transform.position = newPosition;
    }
}
