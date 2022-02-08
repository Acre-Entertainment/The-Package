using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLevers : MonoBehaviour
{
    private SaveWhichLeversWereUsed saveWhichLeversWereUsed;
    void Start()
    {
        saveWhichLeversWereUsed = GameObject.FindGameObjectWithTag("MetrovaniaSave").GetComponent<SaveWhichLeversWereUsed>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player_FSM>())
        {
            saveWhichLeversWereUsed.updateThis();
        }
    }
}
