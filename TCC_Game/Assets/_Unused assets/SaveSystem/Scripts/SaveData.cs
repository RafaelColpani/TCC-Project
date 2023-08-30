using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectActiveTest
{
    public string id;
    public bool isActive;

    public ObjectActiveTest(string id, bool isActive)
    {
        this.id = id;
        this.isActive = isActive;
    }
}

[System.Serializable]
public class SaveData
{
    public long lastUpdated;
    public int testCount;
    public string activeScene;
    public Vector3 testPosition;
    public List<ObjectActiveTest> objectActives;

    public SaveData()
    {
        this.testCount = 0;
        this.activeScene = "Save_1";
        this.testPosition = Vector3.zero;
        this.objectActives = new List<ObjectActiveTest>();
    }
}
