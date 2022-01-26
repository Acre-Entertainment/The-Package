using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDisabler : MonoBehaviour
{
    private DialogueNullifier dn;
    public GameObject SetAsTrue1;
    public GameObject SetAsTrue2;
    public GameObject SetAsTrue3;
    public GameObject SetAsTrue4;
    public GameObject SetAsTrue5;

    public GameObject SetAsFalse1;
    void Start()
    {
        dn = GameObject.FindGameObjectWithTag("DN").GetComponent<DialogueNullifier>();
        if(dn.thisIsOver == true){
            SetAsTrue1.SetActive(true);
            SetAsTrue2.SetActive(true);
            SetAsTrue3.SetActive(true);
            SetAsTrue4.SetActive(true);
            SetAsTrue5.SetActive(true);
            SetAsFalse1.SetActive(false);
        }
    }
}
