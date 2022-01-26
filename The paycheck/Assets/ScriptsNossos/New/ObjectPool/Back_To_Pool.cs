using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_To_Pool : MonoBehaviour
{
    [SerializeField] float time_Active = 1f;
    float time_To_Deactivate = 0;

    void OnEnable()
    {
        time_To_Deactivate = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time > time_To_Deactivate + time_Active)
            PoolManager.ReleaseObject(this.gameObject);
    }
}