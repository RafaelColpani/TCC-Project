using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    #region Vars
    private string dataDirecotryPath = "";
    private string dataFileName = "";
    private readonly string encryptionCodeWord = "word";

    private bool useEncryption = false;
    #endregion

    public FileDataHandler(string dataDirecotryPath, string dataFileName, bool useEncryption)
    {
        this.dataDirecotryPath = dataDirecotryPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    #region Methods

    #region Save & Load
    public SaveData Load(string profileId)
    {
        if (string.IsNullOrEmpty(profileId)) return null;

        string fullPath = Path.Combine(dataDirecotryPath, profileId, dataFileName);

        SaveData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    using (StreamReader reader = new StreamReader(stream))
                        dataToLoad = reader.ReadToEnd();

                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // deserialization
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception error)
            {
                Debug.LogError($"Error trying to load data in {fullPath}\n {error}");
            }
        }

        return loadedData;
    }

    public void Save(SaveData data, string profileId)
    {
        if (string.IsNullOrEmpty(profileId)) return;

        string fullPath = Path.Combine(dataDirecotryPath, profileId, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(stream))
                    writer.Write(dataToStore);
        }
        catch (Exception error)
        {
            Debug.LogError($"Error trying to save data in {fullPath}\n {error}");
        }
    }
    #endregion

    /// <summary>
    /// Gets the profile ID that was played the most recently, to work with the Continue button on main menu.
    /// </summary>
    /// <returns>The value of the most recently played Profile ID</returns>
    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = "";

        Dictionary<string, SaveData> profilesSaveData = LoadAllProfiles();
        foreach (KeyValuePair<string, SaveData> pair in profilesSaveData)
        {
            string profileId = pair.Key;
            SaveData saveData = pair.Value;

            if (saveData == null) continue;

            if (string.IsNullOrEmpty(mostRecentProfileId))
                mostRecentProfileId = profileId;

            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesSaveData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(saveData.lastUpdated);

                if (newDateTime > mostRecentDateTime)
                    mostRecentProfileId = profileId;
            }
        }

        return mostRecentProfileId;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);

        return modifiedData;
    }

    /// <summary>
    /// Gets all the profile ID`s actually stored.
    /// </summary>
    /// <returns>All the profiles ID`s with it`s respectives SaveData`s</returns>
    public Dictionary<string, SaveData> LoadAllProfiles()
    {
        Dictionary<string, SaveData> profileDictionary = new Dictionary<string, SaveData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirecotryPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirecotryPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory while loading save slots, because directory does not contian " +profileId);
                continue;
            }

            SaveData profileData = Load(profileId);

            if (profileId != null) { profileDictionary.Add(profileId, profileData); }
            else { Debug.LogError("Error while trying to load save slot from " + profileId); }
        }

        return profileDictionary;
    }
    #endregion
}
