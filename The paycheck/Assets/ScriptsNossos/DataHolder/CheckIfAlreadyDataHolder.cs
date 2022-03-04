using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfAlreadyDataHolder : MonoBehaviour
{
    private GameObject _dt;
    void Awake() //Garante que ha apenas um DataHolder por cena e que ele não e destroido ao mudar de cena.
    {
        _dt = GameObject.FindWithTag("DataHolder");
        if(_dt == null)
        {
            transform.gameObject.tag = "DataHolder";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        };
    }
}
