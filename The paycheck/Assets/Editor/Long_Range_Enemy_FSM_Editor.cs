using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LongRangeEnemy;

[CustomEditor(typeof(LongRangeEnemyFSM))]
public class Long_Range_Enemy_FSM_Editor : Editor
{
    int previousSpotLength = -1;

    public void OnSceneGUI()
    {
        LongRangeEnemyFSM inst = target as LongRangeEnemyFSM;

        if(previousSpotLength < 0)
            previousSpotLength = inst.spots.Length;

        EditorGUI.BeginChangeCheck();

        if (inst.spots.Length > previousSpotLength)
            inst.spots[inst.spots.Length - 1] = inst.transform.position;

        previousSpotLength = inst.spots.Length;

        for(int i = 0; i < inst.spots.Length; i++)
        {
            //Vector2 new_Spot_Pos = Handles.PositionHandle(new Vector3(inst.spots[i].x, inst.spots[i].y,0), Quaternion.identity);
            Vector2 new_Spot_Pos = Handles.FreeMoveHandle(new Vector3(inst.spots[i].x, inst.spots[i].y,0), Quaternion.identity, .7f, new Vector3(.25f, .25f,0f), Handles.CircleHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(inst, "Change on one of the spots target of the Long_Range_Enemy_FSM");
                inst.spots[i] = new_Spot_Pos;        
            }
        }        
    }
}