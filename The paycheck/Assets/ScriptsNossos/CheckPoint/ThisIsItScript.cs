using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisIsItScript : MonoBehaviour
{
    private DialogueNullifier dn;

    void Start()
    {
        dn = GameObject.FindGameObjectWithTag("DN").GetComponent<DialogueNullifier>();
        dn.thisIsOver = true;
    }
}