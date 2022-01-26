using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Pool : MonoBehaviour
{
    [SerializeField] GameObject prefab_To_Pool;
    [SerializeField] int initial_Amount;
    //[SerializeField] int max_Amount;
    public List<GameObject> pool = new List<GameObject>();

    private void Awake() 
    {
        for(int i = 0; i < initial_Amount; i++)
            pool.Add(Instantiate(prefab_To_Pool, transform.position, Quaternion.identity));
    }

    public GameObject Get_Obj_From_Pool()
    {
        //if(pool.Count == max_Amount)
            //return null;

        foreach(GameObject go in pool)
        {
            if(!go.activeInHierarchy)
                return go;
        }

        GameObject new_Obj = Instantiate(prefab_To_Pool, transform.position, Quaternion.identity);
        new_Obj.SetActive(false);
        pool.Add(new_Obj);

        return new_Obj;
    }
}