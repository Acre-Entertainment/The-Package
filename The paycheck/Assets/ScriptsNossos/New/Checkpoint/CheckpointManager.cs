//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CheckpointManager : Singleton<CheckpointManager>
//{
//    [SerializeField]
//    private string level;
//    private Vector2 lastCheckpoint;

//    List<SaveCheckpoint> checkpoints = new List<SaveCheckpoint>();

//    private void Awake()
//    {
//        DontDestroyOnLoad(this);
//    }

//    public static void SaveCheckpoint(string levelName, Vector2 checkpointPos)
//    {
//        Instance.level = levelName;
//        Instance.lastCheckpoint = checkpointPos;
//    }    

//    public static Vector3 LoadCheckpointPos(Transform player)
//    {
//        if(SceneManager.GetActiveScene().name == Instance.level)
//        {
//            return Instance.lastCheckpoint;
//        }

//        return player.position;
//    }

//    public static bool ThisIsTheLatestCheckpoint(Vector2 checkpoint)
//    {
//        if (checkpoint == Instance.lastCheckpoint)
//            return true;

//        return false;
//    }

//    /// <summary>
//    /// Erase saved data
//    /// </summary>
//    public static void Clean()
//    {
//        Instance.level = "";
//        Instance.lastCheckpoint = Vector2.positiveInfinity;
//    }
//}