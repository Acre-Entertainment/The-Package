//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Events;

//public class SaveCheckpoint : MonoBehaviour
//{
//    private Transform playerHeight;

//    public UnityEvent onLoadCheckpoint;

//    private void Awake()
//    {
//        playerHeight = transform.GetChild(0).transform;

//        if(playerHeight ==  null)
//        {
//            gameObject.SetActive(false);
//            return;
//        }

//        if (CheckpointManager.ThisIsTheLatestCheckpoint(playerHeight.position))
//            LoadCheckpoint();
//    }

//    void Save()
//    {
//        CheckpointManager.SaveCheckpoint(SceneManager.GetActiveScene().name, playerHeight.position);

//        Destroy(gameObject);
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.GetComponent<Player_FSM>())
//            Save();
//    }

//    void LoadCheckpoint()
//    {
//        onLoadCheckpoint.Invoke();

//        Destroy(gameObject);
//    }
//}