using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level { Level1, Level2, Level3 }

sealed class SaveManager : MonoBehaviour, IDataPersistance
{
    [Header("Player")]
    [SerializeField]
    private GameObject _player;

    [Header("Level")]
    [SerializeField]
    private Level _level;
    public void LoadData(GameData data)
    {
        if(_level == Level.Level1)
        {
            _player.transform.position = data.level1PlayerPosition;
        }
        else if(_level == Level.Level2)
        {
            _player.transform.position = data.level2PlayerPosition;
        }
        else if(_level == Level.Level3)
        {
            _player.transform.position = data.level3PlayerPosition;
        }
    }

    public void SaveData(GameData data)
    {
        if (_level == Level.Level1)
        {
            data.level1PlayerPosition = _player.transform.position;
        }
        else if (_level == Level.Level2)
        {
            data.level2PlayerPosition = _player.transform.position;
        }
        else if (_level == Level.Level3)
        {
            data.level3PlayerPosition = _player.transform.position;
        }
    }

    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }
}
