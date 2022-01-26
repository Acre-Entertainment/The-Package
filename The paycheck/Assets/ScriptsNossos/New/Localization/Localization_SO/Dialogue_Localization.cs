using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Dialogue")]
public class Dialogue_Localization : ScriptableObject
{
    public Localization[] localizations;
}

[System.Serializable]
public class Localization
{
    // If need more information put it hear
    // public string option_Line;
    [TextArea(3, 10)] 
    public string dialogue;
}