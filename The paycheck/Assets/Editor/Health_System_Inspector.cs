using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health_System))]
public class Health_System_Inspector : Editor
{
    //bool show_Tags = false; 

    //public override void OnInspectorGUI()
    //{
    //    base.DrawDefaultInspector();

    //    Health_System instance = target as Health_System;

    //    show_Tags = EditorGUILayout.BeginFoldoutHeaderGroup(show_Tags, "Damage Tags");

    //    if(show_Tags)
    //    {
    //        // Tags list size
    //        int tag_Length = EditorGUILayout.IntField(instance.damage_Tags.Length);
    //        // Temporary list
    //        string[] new_damage_Tags = new string[tag_Length];

    //        // Pass current damage_Tags velue to temporary list
    //        for(int i = 0; i < new_damage_Tags.Length; i++)
    //        {
    //            if(i < instance.damage_Tags.Length)
    //                new_damage_Tags[i] = instance.damage_Tags[i];
    //        }

    //        // Create all the buttons
    //        for(int i = 0; i < tag_Length; i++)
    //        {
    //            if (i < instance.damage_Tags.Length)
    //                new_damage_Tags[i] = EditorGUILayout.TagField(instance.damage_Tags[i]);
    //            else
    //                new_damage_Tags[i] = "Default";
    //        }

    //        // Update damage tags with temporary values
    //        instance.damage_Tags = new_damage_Tags;

    //        EditorGUILayout.EndFoldoutHeaderGroup();
    //    }
    //}
}
