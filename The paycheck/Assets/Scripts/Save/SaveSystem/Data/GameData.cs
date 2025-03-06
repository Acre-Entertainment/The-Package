using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector2 level1PlayerPosition;
    public Vector2 level2PlayerPosition;
    public Vector2 level3PlayerPosition;

    public int level1Checkpoint;
    public int level2Checkpoint;
    public int level3Checkpoint;

    public bool lever1;
    public bool lever2;
    public bool lever3;

    public bool smallDoors;
    public bool doorPuzzle2;

    public bool upArea;
    public bool leftArea;
    public bool rightArea;

    public bool level2Unlocked;
    public bool level3Unlocked;
    public bool gameComplete;
    public bool acceptEnding;
    public bool denyEnding;

    public int lastLevel;
    public GameData()
    {
        level1PlayerPosition = new Vector2(0, 0);
        level2PlayerPosition = new Vector2(0, 0);
        level3PlayerPosition = new Vector2(0, 0);

        level1Checkpoint = -1;
        level2Checkpoint = -1;
        level3Checkpoint = -1;

        lever1 = false;
        lever2 = false;
        lever3 = false;

        smallDoors = false;
        doorPuzzle2 = false;

        upArea = false;
        leftArea = false;
        rightArea = false;

        level2Unlocked = false;
        level3Unlocked = false;
        gameComplete = false;
        acceptEnding = false;
        denyEnding = false;

        lastLevel = 0;
    }
}
