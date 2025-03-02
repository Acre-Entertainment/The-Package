using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuPrincipal : MonoBehaviour{

    public void JogarJogo ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SairDoJogo ()
    {
        Application.Quit();
    }

    //private void Awake()
    //{
    //    CheckpointManager.Clean();
    //}
}