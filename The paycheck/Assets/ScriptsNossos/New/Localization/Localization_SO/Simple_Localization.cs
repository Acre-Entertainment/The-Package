using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Simple_Localization")]
public class Simple_Localization : ScriptableObject
{
    [TextArea(3, 10)] 
    public string[] text_Localized;
}