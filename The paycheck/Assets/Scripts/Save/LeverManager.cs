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
    public void LoadData(GameData data)
    {
        _lever1 = data.lever1;
        _lever2 = data.lever2;
        _lever3 = data.lever3;

        _smallDoors = data.smallDoors;

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
    }

    public void SaveData(GameData data)
    {
        data.lever1 = _lever1;
        data.lever2 = _lever2;
        data.lever3 = _lever3;
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

    public void ResetLever()
    {
        _lever1 = false;
        _lever2 = false;
        _lever3 = false;
        _smallDoors = false;
    }
}
