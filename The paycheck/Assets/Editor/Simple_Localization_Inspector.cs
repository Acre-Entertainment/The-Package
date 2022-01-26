using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Simple_Localization))]
public class Simple_Localization_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        Simple_Localization instance = target as Simple_Localization;

        // Instance localized array lengh should be iqual to languages length
        if (instance.text_Localized == null)
            instance.text_Localized = new string[Global_Localization.languages.Length];

        if(instance.text_Localized.Length != Global_Localization.languages.Length)
            instance.text_Localized = Resize_Array(instance.text_Localized, Global_Localization.languages.Length);

        // Create A text space for each member of language
        for(int i = 0; i < Global_Localization.languages.Length; i++)
        {
            EditorGUILayout.LabelField(Global_Localization.languages[i]);
            
            instance.text_Localized[i] = EditorGUILayout.TextArea(instance.text_Localized[i]);

            GUILayout.Space(20);
        }
    }    

    string[] Resize_Array(string[] text_Array, int new_Length)
    {
        string[] new_Text_Array = new string[new_Length];

        for(int i = 0; i < new_Text_Array.Length; i++)
        {
            if(i < text_Array.Length)
                new_Text_Array[i] = text_Array[i];
            else
                new_Text_Array[i] = "";
        }

        return new_Text_Array;
    }

}