using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

sealed class LeverManager : MonoBehaviour, IDataPersistance
{
    [Header("ActiveEvents")]
    [SerializeField]
    private UnityEvent[] _activeEvents;

    private bool _lever1;
    private bool _lever2;
    private bool _lever3;

    private bool _smallDoors;
    private bool _doorPuzzle2;

    private bool _upArea;
    private bool _leftArea;
    private bool _rightArea;
    private bool _finalCollider;
    public void LoadData(GameData data)
    {
        _lever1 = data.lever1;
        _lever2 = data.lever2;
        _lever3 = data.lever3;

        _smallDoors = data.smallDoors;
        _doorPuzzle2 = data.doorPuzzle2;

        _upArea = data.upArea;
        _leftArea = data.leftArea;
        _rightArea = data.rightArea;
        _finalCollider = data.finalCollider;

        if (_lever1)
        {
            _activeEvents[0].Invoke();
        }

        if (_lever2)
        {
            _activeEvents[1].Invoke();
        }

        if (_lever3)
        {
            _activeEvents[2].Invoke();
        }

        if (_smallDoors)
        {
            _activeEvents[3].Invoke();
        }

        if (_doorPuzzle2)
        {
            _activeEvents[4].Invoke();
        }

        if (_upArea)
        {
            _activeEvents[5].Invoke();
        }

        if(_leftArea)
        {
            _activeEvents[6].Invoke();
        }

        if(_rightArea)
        {
            _activeEvents[7].Invoke();
        }

        if(_finalCollider)
        {
            _activeEvents[8].Invoke();
        }
    }

    public void SaveData(GameData data)
    {
        data.lever1 = _lever1;
        data.lever2 = _lever2;
        data.lever3 = _lever3;

        data.smallDoors = _smallDoors;
        data.doorPuzzle2 = _doorPuzzle2;

        data.upArea = _upArea;
        data.leftArea = _leftArea;
        data.rightArea = _rightArea;
    }

    public void SetLever(int lever)
    {
        switch (lever)
        {
            case 1:
                _lever1 = true;
                break;
            case 2:
                _lever2 = true;
                break;
            case 3:
                _lever3 = true;
                break;
        }
    }

    public void SmallDoors()
    {
        _smallDoors = true;
    }

    public void DoorPuzzle2()
    {
        _doorPuzzle2 = true;
    }

    public void SetUpArea(bool area)
    {
        _upArea = area;
    }

    public void SetLeftArea(bool area)
    {
        _leftArea = area;
    }

    public void SetRightArea(bool area)
    {
        _rightArea = area;
    }

    public void SetFinalCollider()
    {
        _finalCollider = true;
    }

    public void ResetLever()
    {
        _lever1 = false;
        _lever2 = false;
        _lever3 = false;
        _smallDoors = false;
        _doorPuzzle2 = false;
        _upArea = false;
        _leftArea = false;
        _rightArea = false;
        _finalCollider = false;
    }
}
