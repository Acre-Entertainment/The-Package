using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingEnable : MonoBehaviour{
    public Patroling objectWithPatrolingScript;
    public bool activateMe;
    void Start(){
        if(activateMe == true){
            objectWithPatrolingScript.enabled = true;
        }
        else{
            objectWithPatrolingScript.enabled = false;
        }
    }
}
