using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLevers : MonoBehaviour
{
    private SaveWhichLeversWereUsed saveWhichLeversWereUsed;
    void Start()
    {
        saveWhichLeversWereUsed = GameObject.FindGameObjectWithTag("MetrovaniaSave").GetComponent<SaveWhichLeversWereUsed>();
        saveWhichLeversWereUsed.deleteThis();
    }
}