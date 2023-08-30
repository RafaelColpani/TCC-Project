using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

public class ObjectActives : MonoBehaviour, IDataPersistance
{
    #region Vars
    [ValidateInput("IsNotNull")] [ReadOnly]
    [SerializeField] private string id;

    private bool isActive = true;

    // -- Vars functions
    [ContextMenu("Generate unique ID")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool IsNotNull(string str)
    {
        return !string.IsNullOrEmpty(str);
    }
    #endregion

    #region Save & Load
    public void LoadData(SaveData data)
    {
        if (!HasDataExistance(data)) { return; }

        var findedObject = data.objectActives.Find(x => x.id == this.id);
        this.gameObject.SetActive(findedObject.isActive);
    }

    public void SaveData(SaveData data)
    {
        if (HasDataExistance(data))
        {
            int objectIndex = data.objectActives.FindIndex(x => x.id == this.id);
            data.objectActives[objectIndex] = new ObjectActiveTest(id, isActive);
        }

        else
        {
            data.objectActives.Add(new ObjectActiveTest(id, isActive));
        }
    }
    #endregion

    private bool HasDataExistance(SaveData data)
    {
        foreach (ObjectActiveTest obj in data.objectActives)
        {
            if (obj.id != this.id) { continue; }

            return true;
        }

        return false;
    }

    #region Unity Events
    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDisable()
    {
        isActive = false;
    }
    #endregion
}
