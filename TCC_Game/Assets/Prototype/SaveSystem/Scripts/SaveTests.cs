using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SaveTests : MonoBehaviour, IDataPersistance
{
    [Button("Increase Test Count")]
    private void IncreaseTest() { testCount++; print(testCount); }
    [Button("Decrease Test Count")]
    private void DecreaseTest() { testCount--; print(testCount); }
    [Button("Print Test Count")]
    private void PrintTest() { print(testCount); }

    int testCount = 0;

    public void LoadData(SaveData data)
    {
        this.testCount = data.testCount;
    }

    public void SaveData(SaveData data)
    {
        data.testCount = this.testCount;
    }
}
