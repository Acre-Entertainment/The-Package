using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReloader : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
