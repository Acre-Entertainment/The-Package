using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

sealed class MenuManager : MonoBehaviour, IDataPersistance
{
    [Header("Buttons")]
    [SerializeField]
    private GameObject[] _button;

    private bool _level2Unlocked;
    private bool _level3Unlocked;
    private bool _gameComplete;
    private bool _acceptEndning;
    private bool _denyEnding;

    private bool _resetLevel1 = false;
    private bool _resetLevel2 = false;
    private bool _resetLevel3 = false;

    private int _lastLevel;

    public void LoadData(GameData data)
    {
        _level2Unlocked = data.level2Unlocked;
        _level3Unlocked = data.level3Unlocked;
        _gameComplete = data.gameComplete;
        _acceptEndning = data.acceptEnding;
        _denyEnding = data.denyEnding;

        _lastLevel = data.lastLevel;

        CheckChapters();
    }

    public void SaveData(GameData data)
    {
        if (_resetLevel1)
        {
            data.level1PlayerPosition = Vector2.zero;
            data.level1Checkpoint = -1;
        }

        if (_resetLevel2)
        {
            data.level2PlayerPosition = Vector2.zero;
            data.level2Checkpoint = -1;
        }

        if (_resetLevel3)
        {
            data.level3PlayerPosition = Vector2.zero;
            data.level3Checkpoint = -1;
            data.lever1 = false;
            data.lever2 = false;
            data.lever3 = false;
            data.smallDoors = false;
            data.doorPuzzle2 = false;
        }
    }

    public void CheckChapters()
    {
        if(_lastLevel != 0)
        {
            _button[4].SetActive(true);
        }

        if(_level2Unlocked)
        {
            _button[0].SetActive(true);
            _button[1].SetActive(false);
        }

        if(_level3Unlocked)
        {
            _button[2].SetActive(true);
            _button[3].SetActive(false);
            _button[5].SetActive(true);
            _button[6].SetActive(false);
        }

        if (_gameComplete)
        {
            _button[7].SetActive(true);
            _button[8].SetActive(false);
            _button[9].SetActive(true);
            _button[10].SetActive(false);
        }

        if (_acceptEndning)
        {
            _button[11].SetActive(true);
            _button[12].SetActive(false);
        }

        if (_denyEnding)
        {
            _button[13].SetActive(true);
            _button[14].SetActive(false);
        }
    }

    public void ResetLevels(int level)
    {
        switch (level)
        {
            case 1:
                _resetLevel2 = true;
                _resetLevel3 = true;
                break;
            case 2:
                _resetLevel1 = true;
                _resetLevel3 = true;
                break;
            case 3:
                _resetLevel1 = true;
                _resetLevel2 = true;
                break;
        }
    }

    public void ContinueLastLevel()
    {
        if(_lastLevel == 1)
        {
            SceneManager.LoadScene("Load 1");
        }
        else if(_lastLevel == 2)
        {
            SceneManager.LoadScene("Load 2");
        }
        else if(_lastLevel == 3)
        {
            SceneManager.LoadScene("Load 3");
        }
    }

    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }
}
