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

    private bool _level2;
    private bool _level3;

    private bool _isCheckpointSave;
    public void LoadData(GameData data)
    {
        if(_level == Level.Level1)
        {
            if(data.level1PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level1PlayerPosition;
            }

            if(data.level1Checkpoint != -1)
            {
                _level1Checkpoint = data.level1Checkpoint;
                _skipCutscene[_level1Checkpoint].Invoke();
            }
        }
        else if(_level == Level.Level2)
        {
            if (data.level2PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level2PlayerPosition;
            }

            if (data.level2Checkpoint != -1)
            {
                _level2Checkpoint = data.level2Checkpoint;
                _skipCutscene[_level2Checkpoint].Invoke();
            }
        }
        else if(_level == Level.Level3)
        {
            if (data.level3PlayerPosition != Vector2.zero)
            {
                _player.transform.position = data.level3PlayerPosition;
            }

            if (data.level3Checkpoint != -1)
            {
                _level3Checkpoint = data.level3Checkpoint;
                _skipCutscene[_level3Checkpoint].Invoke();
            }
        }

        _level2 = data.level2Unlocked;
        _level3 = data.level3Unlocked;
    }

    public void SaveData(GameData data)
    {
        if (_isCheckpointSave)
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

            if (_level == Level.Level1)
            {
                data.level1Checkpoint = _level1Checkpoint;
            }
            else if (_level == Level.Level2)
            {
                data.level2Checkpoint = _level2Checkpoint;
            }
            else if (_level == Level.Level3)
            {
                data.level3Checkpoint = _level3Checkpoint;
            }
        }
        
        data.level2Unlocked = _level2;
        data.level3Unlocked = _level3;
    }

    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }

    public void SetCheckPoint(int checkpoint)
    {
        _isCheckpointSave = true;

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

    public void UnlockLevel(int level)
    {
        _isCheckpointSave = false;

        if(level == 2)
        {
            _level2 = true;
        }
        else if(level == 3)
        {
            _level3 = true;
        }
    }
}
