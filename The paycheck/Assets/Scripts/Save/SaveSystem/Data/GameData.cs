using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector2 level1PlayerPosition;
    public Vector2 level2PlayerPosition;
    public Vector2 level3PlayerPosition;
    public GameData()
    {
        level1PlayerPosition = new Vector2(144.1126f, -0.0105f);
        level2PlayerPosition = new Vector2(-21.1f, 0);
        level3PlayerPosition = new Vector2(208.7f, 0);
    }
}
