using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    #region Vars

    #region Inspector
    [Header("Debugging")]
    [Tooltip("Mark this checkbox if you want to not store your progress during test.")]
    [SerializeField] private bool disableDataPersistance = false;
    [Tooltip("Initialize the save data even if there`s no data stored.")]
    [SerializeField] private bool initializeDataIfNull = false;
    [Tooltip("Overrides a store data, with the same name as testSelectedProfileId.")]
    [SerializeField] private bool overrideSelectedProfileId = false;
    [Tooltip("Write the name of the profile to override the stored data (or creates a brand new storage). Needs to check the Override box to work")]
    [ValidateInput("IsOverriding")] [SerializeField] private string testSelectedProfileId = "";

    [Header("File Storage")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    #endregion

    #region Private vars
    private SaveData _saveData;
    private List<IDataPersistance> iDataPersistanceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "test";
    #endregion

    private bool IsOverriding(string str)
    {
        return !(overrideSelectedProfileId && string.IsNullOrEmpty(str));
    }
    #endregion

    #region Unity Methods & Events
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (disableDataPersistance)
            Debug.LogWarning("Data persistance is currently Disabled!");

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Override selected profile ID with test ID: " +testSelectedProfileId);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.iDataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    #endregion

    #region Methods
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    /// <summary>
    /// Creates a new SaveData to work with this instance.
    /// </summary>
    public void NewGame()
    {
        this._saveData = new SaveData();
        SaveGame();
    }

    #region Save & Load
    public void LoadGame()
    {
        if (disableDataPersistance) return;

        this._saveData = dataHandler.Load(selectedProfileId);

        if (this._saveData == null && initializeDataIfNull)
            NewGame();

        if (this._saveData == null)
        {
            Debug.LogWarning("No save data was found, start a new game before.");
            return;
        }

        foreach (IDataPersistance dataObject in iDataPersistanceObjects)
            dataObject.LoadData(_saveData);
    }

    public void SaveGame()
    {
        if (disableDataPersistance) return;

        if (this._saveData == null)
        {
            Debug.LogWarning("No data found. Needs to start a new game before data can be saved");
            return;
        }

        foreach (IDataPersistance dataObject in iDataPersistanceObjects)
        {
            dataObject.SaveData(_saveData);
        }

        _saveData.lastUpdated = System.DateTime.Now.ToBinary();

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            _saveData.activeScene = SceneManager.GetActiveScene().name;
        }

        dataHandler.Save(_saveData, selectedProfileId);
    }
    #endregion

    #region Data Persistance
    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public bool HasGameData()
    {
        return _saveData != null;
    }

    public string GetActiveScene()
    {
        return _saveData.activeScene;
    }

    public Dictionary<string, SaveData> GetAllProfilesSaveData()
    {
        return dataHandler.LoadAllProfiles();
    }
    #endregion

    private void OnApplicationQuit() => SaveGame();
    #endregion
}
