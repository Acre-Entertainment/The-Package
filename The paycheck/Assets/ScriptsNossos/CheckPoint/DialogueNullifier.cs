using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNullifier : MonoBehaviour
{
    private static DialogueNullifier mineInstance;
    public bool thisIsOver;
    void Awake(){
        if(mineInstance == null){
            mineInstance = this;
            DontDestroyOnLoad(mineInstance);
        }
        else{
            Destroy(gameObject);
        }
    }
}