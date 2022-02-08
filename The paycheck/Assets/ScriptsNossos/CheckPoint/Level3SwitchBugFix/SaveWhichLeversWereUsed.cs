using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWhichLeversWereUsed : MonoBehaviour
{
    public GameObject dialogue, door1, door2, door3, finalDoor1, finalDoor2, finalDoor3, alavanca1_on, alavanca1_off, alavanca2_on, alavanca2_off, alavanca3_on, alavanca3_off;
    public bool dialogueStat, door1Stat, door2Stat, door3Stat, finalDoor1Stat, finalDoor2Stat, finalDoor3Stat, alavanca1_onStat, alavanca1_offStat, alavanca2_onStat, alavanca2_offStat, alavanca3_onStat, alavanca3_offStat;
    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("MetrovaniaSave") != null)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
    public void updateThis()
    {
        dialogueStat = dialogue.activeSelf;
        door1Stat = door1.activeSelf;
        door2Stat = door2.activeSelf;
        door3Stat = door3.activeSelf;
        finalDoor1Stat = finalDoor1.activeSelf;
        finalDoor2Stat = finalDoor2.activeSelf;
        finalDoor3Stat = finalDoor3.activeSelf;
        alavanca1_onStat = alavanca1_on.activeSelf;
        alavanca1_offStat = alavanca1_off.activeSelf;
        alavanca2_onStat = alavanca2_on.activeSelf;
        alavanca2_offStat = alavanca2_off.activeSelf;
        alavanca3_onStat = alavanca3_on.activeSelf;
        alavanca3_offStat = alavanca3_off.activeSelf;
    }
    public void deleteThis()
    {
        Destroy(this);
    }
    void Start()
    {
        dialogue.SetActive(dialogueStat);
        door1.SetActive(door1Stat);
        door2.SetActive(door2Stat);
        door3.SetActive(door3Stat);
        finalDoor1.SetActive(finalDoor1Stat);
        finalDoor2.SetActive(finalDoor2Stat);
        finalDoor3.SetActive(finalDoor3Stat);
        alavanca1_off.SetActive(alavanca1_offStat);
        alavanca1_on.SetActive(alavanca1_onStat);
        alavanca2_off.SetActive(alavanca2_offStat);
        alavanca2_on.SetActive(alavanca2_onStat);
        alavanca3_off.SetActive(alavanca3_offStat);
        alavanca3_on.SetActive(alavanca3_onStat);
    }
}
