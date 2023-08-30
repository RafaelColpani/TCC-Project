using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour, IDataPersistance
{
    public void LoadData(SaveData data)
    {
        this.transform.position = data.testPosition;
    }

    public void SaveData(SaveData data)
    {
        data.testPosition = this.transform.position;
    }
}
