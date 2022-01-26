using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsToPool : MonoBehaviour
{
    [SerializeField] ObjectToPool[] objs_To_Pool;

    void Start()
    {
        foreach(ObjectToPool obj in objs_To_Pool)
            PoolManager.WarmPool(obj.prefab_To_Pool, obj.amount_To_Pool);
    }
}

[System.Serializable]
public class ObjectToPool
{
    [SerializeField] string obj_Name;
    public GameObject prefab_To_Pool;
    public int amount_To_Pool;
}
