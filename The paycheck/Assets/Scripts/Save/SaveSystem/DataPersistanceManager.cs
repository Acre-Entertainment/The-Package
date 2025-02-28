using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debbuging")]
    [SerializeField]
    private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    [SerializeField]
    private bool useEncryption;

    //[Header("Auto Saving Configuration")]
    //[SerializeField]
    //private float autoSaveTimeSeconds = 60f;

    private GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "save";

    //private Coroutine autoSaveCoroutine;
    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Encontrou mais do que um DataPersistanceManager na cena, destruindo o novo");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();

        //if(autoSaveCoroutine != null)
        //{
        //    StopCoroutine(autoSaveCoroutine);
        //}
        //autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);

        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("Nenhum dado foi encontrado, Inicie um novo jogo");
            return;
        }

        foreach(IDataPersistance dataPersistancesObj in dataPersistanceObjects)
        {
            dataPersistancesObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("Nenhum dado foi encontrado, um novo jogo precisa ser iniciado antes dos dados poderem ser salvos");
            return;
        }

        foreach (IDataPersistance dataPersistancesObj in dataPersistanceObjects)
        {
            dataPersistancesObj.SaveData(gameData);
        }

        dataHandler.Save(gameData, selectedProfileId);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    //private IEnumerator AutoSave()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(autoSaveTimeSeconds);
    //        SaveGame();
    //    }
    //}

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}
}
