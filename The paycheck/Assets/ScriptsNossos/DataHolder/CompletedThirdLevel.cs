using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedThirdLevel : MonoBehaviour
{
    private DataHolder dataHolder;
    void Start()
    {
        dataHolder = GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>();
        dataHolder.completedThirdLevel = true;
    }
}
