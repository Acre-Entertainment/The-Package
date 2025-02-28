using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Level { Level1, Level2, Level3 }

sealed class SaveManager : MonoBehaviour, IDataPersistance
{
    [Header("Level")]
    [SerializeField]
    private Level _level;

    [Header("Player")]
    [SerializeField]
    private GameObject _player;

    [Header("SkipCutscene")]
    [SerializeField]
    private UnityEvent[] _skipCutscene;

    private int _level1Checkpoint;
    private int _level2Checkpoint;
    private int _level3Checkpoint;
    public void LoadData(GameData data)
    {
        if(_level == Level.Level1)
        {
            if(data.level1PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level1PlayerPosition;
            }

            _level1Checkpoint = data.level1Checkpoint;
            _skipCutscene[_level1Checkpoint].Invoke();
        }
        else if(_level == Level.Level2)
        {
            if (data.level2PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level2PlayerPosition;
            }

            _level2Checkpoint = data.level2Checkpoint;
            _skipCutscene[_level2Checkpoint].Invoke();
        }
        else if(_level == Level.Level3)
        {
            if (data.level3PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level3PlayerPosition;
            }

            _level3Checkpoint = data.level3Checkpoint;
            _skipCutscene[_level3Checkpoint].Invoke();
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

        data.level1Checkpoint = _level1Checkpoint;
        data.level2Checkpoint = _level2Checkpoint;
        data.level3Checkpoint = _level3Checkpoint;
    }

    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }

    public void SetCheckPoint(int checkpoint)
    {
        if (_level == Level.Level1)
        {
            _level1Checkpoint = checkpoint;
        }
        else if (_level == Level.Level2)
        {
            _level2Checkpoint = checkpoint;
        }
        else if (_level == Level.Level3)
        {
            _level3Checkpoint = checkpoint;
        }
    }
}
