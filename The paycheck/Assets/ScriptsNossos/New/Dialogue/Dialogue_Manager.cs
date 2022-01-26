using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialogue_Manager : MonoBehaviour
{
    [SerializeField] GameObject dialogue_GUI;
    [SerializeField] TextMeshProUGUI text_Space;
    bool is_Running;                                        // If the dialog is runing

    string[] names;
    string[] lines;

    int lines_Iterator;
    UnityEvent close_Event;

    private void Awake() 
    {
        Global_Events.Open_Dialogue_Event += Open_Dialogue;    
    }

    void Update()
    {
        if(dialogue_GUI.activeInHierarchy)
        {
            if(Input.GetMouseButtonDown(0))
                Write_Dialogue();
        }
    }

    void Open_Dialogue(Dialogue_Localization dialogue, UnityEvent close_Event)
    {
        //close_Event.Invoke();
        Global_Localization.Text_To_Dialogue(dialogue, out names, out lines);

        if(lines == null)
        {
            Close_Dialogue();
            return;
        }

        is_Running = true;
        this.close_Event = close_Event;
        // Enable Menu
        dialogue_GUI.SetActive(true);
        // Reset 
        lines_Iterator = 0;
        
        Write_Dialogue();
    }

    void Write_Dialogue()
    {
        if(lines_Iterator >= lines.Length)
        {
            Close_Dialogue();    
            return;
        }

        text_Space.text = lines[lines_Iterator];
        lines_Iterator++;
    }

    void Close_Dialogue()
    {
        close_Event.Invoke();
        Global_Events.CloseDialogue();

        names = lines = null;
        close_Event = null;
        dialogue_GUI.SetActive(false);
    }
}