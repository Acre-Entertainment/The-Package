//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SetLevelAccess : MonoBehaviour
//{
//    public GameObject secondLevel;
//    public GameObject thirdLevel;
//    private DataHolder dataHolder;
//    void Start()
//    {
//        dataHolder = GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>();
//        if(dataHolder.completedSecondLevel == false)
//        {
//            secondLevel.SetActive(false);
//        }
//        if(dataHolder.completedThirdLevel == false)
//        {
//            thirdLevel.SetActive(false);
//        }
//    }
//}
