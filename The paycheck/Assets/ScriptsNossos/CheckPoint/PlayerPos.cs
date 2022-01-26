using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerPos : MonoBehaviour
{
    void Start()
    {
        transform.position = GameMaster.instance.lastCheckPointPos;
    }

    void Update(){}
}