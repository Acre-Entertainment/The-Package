using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_pause : MonoBehaviour
{
    public KeyCode pauseButton;
    public GameObject pauseMenuPanel;
    public AudioSource music;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            Pause();
        }
    }

    void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            //music.Pause();
            pauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            //music.Play();
            pauseMenuPanel.SetActive(false);
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
}
