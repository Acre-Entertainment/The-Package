using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Play_Dialogue : MonoBehaviour
{
    bool played;
    [SerializeField] Dialogue_Localization dialogue;
    [SerializeField] UnityEvent close_Dialogue;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))    
            Start_Dialogue();
    }

    void Start_Dialogue()
    {
        if(played == false)
        {
            Global_Events.OpenDialogue(dialogue, close_Dialogue);
            played = true;
        }

        gameObject.SetActive(false);
    }
}
