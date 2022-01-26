using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue_Localization))]
public class Dialogue_Localization_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        Dialogue_Localization instance = target as Dialogue_Localization;

        // Instance localizations array length should be iqual to languages length
        if(instance.localizations.Length != Global_Localization.languages.Length)
            instance.localizations = Resize_Array(instance.localizations, Global_Localization.languages.Length);

        for(int i = 0; i < instance.localizations.Length; i++)
        {
            // Show all Localizations
            EditorGUILayout.LabelField(Global_Localization.languages[i]);
            
            instance.localizations[i].dialogue = EditorGUILayout.TextArea(instance.localizations[i].dialogue);

            GUILayout.Space(30);
        }
    }

    Localization[] Resize_Array(Localization[] localizations, int new_Length)
    {
        Localization[] resized_Array = new Localization[new_Length];

        for(int i = 0; i < resized_Array.Length; i++)
        {
            if(i < localizations.Length)
                resized_Array[i] = localizations[i];
            else
                resized_Array[i] = new Localization();
        }

        return resized_Array;
    }
}

/*
        // Me certificando que todos os idiomas terão algum valor
        for(int i = 0; i < instance.localizations.Count; i++)
        {
            if(instance.localizations[i].optionLine == "")
            {
                instance.localizations[i].optionLine = "Hey, Don't forget to translate me to " + Global_Localization.languages[i];
            }

            if(instance.localizations[i].dialogue == "")
            {
                instance.localizations[i].dialogue = 
                "- Alice/Neutral\n" +
                "Hey, you forget to translate this dialogue to " + 
                Global_Localization.languages[i] +
                ", now go do your work ;)"; 
            }
        }
        */