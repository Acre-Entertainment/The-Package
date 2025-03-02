using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class MenuManager : MonoBehaviour, IDataPersistance
{
    [Header("Buttons")]
    [SerializeField]
    private GameObject[] _button;

    private bool _level2Unlocked;
    private bool _level3Unlocked;

    public void LoadData(GameData data)
    {
        _level2Unlocked = data.level2Unlocked;
        _level3Unlocked = data.level3Unlocked;

        CheckChapters();
    }

    public void SaveData(GameData data)
    {
        
    }

    public void CheckChapters()
    {
        if(_level2Unlocked)
        {
            _button[0].SetActive(true);
            _button[1].SetActive(false);
        }

        if(_level3Unlocked)
        {
            _button[2].SetActive(true);
            _button[3].SetActive(false);
        }
    }
}
