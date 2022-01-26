using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Drone_FSM))]
public class Drone_FSM_Editor : Editor
{
    int previous_Update_Spots_Length;

    public void OnSceneGUI()
    {
        Drone_FSM inst = target as Drone_FSM;

        EditorGUI.BeginChangeCheck();

        // Managing spots so when a new one is creted, its position is set to inst position
        if(previous_Update_Spots_Length == 0)
            previous_Update_Spots_Length = inst.spots.Length;

        if(inst.spots.Length > previous_Update_Spots_Length)
            inst.spots[inst.spots.Length - 1] = inst.transform.position;

        previous_Update_Spots_Length = inst.spots.Length;

        for(int i = 0; i < inst.spots.Length; i++)
        {
            //Vector2 new_Spot_Pos = Handles.PositionHandle(new Vector3(inst.spots[i].x, inst.spots[i].y,0), Quaternion.identity);
            Vector2 new_Spot_Pos = Handles.FreeMoveHandle(new Vector3(inst.spots[i].x, inst.spots[i].y,0), Quaternion.identity, .7f, new Vector3(.25f, .25f,0f), Handles.CircleHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(inst, "Change on one of the spots target of the Simple_Enemy_FSM");
                inst.spots[i] = new_Spot_Pos;        
            }
        }        
    }
}