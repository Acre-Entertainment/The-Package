using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    public GameObject myObject;
    public bool activeMe;
    //public Time timeToAct =  0f; 

    void update()
    {
        if(activeMe == true)
        {
            myObject.SetActive (true);
        }
        else
        {
            myObject.SetActive (false);
        }
    }
}
