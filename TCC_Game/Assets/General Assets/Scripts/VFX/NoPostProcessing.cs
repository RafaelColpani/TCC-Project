using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using KevinCastejon.MoreAttributes;

public class NoPostProcessing : MonoBehaviour
{
    [HeaderPlus(" ", "- Array -", (int)HeaderPlusColor.yellow)]
    [SerializeField] List<Volume> postProcessingVolumes;
    [SerializeField] List<GameObject> objNoPostProcessing;

    [HeaderPlus(" ", "- Layer Name -", (int)HeaderPlusColor.blue)]
    [SerializeField] string ignoredLayerName;

    void Start()
    {
        if (postProcessingVolumes != null && postProcessingVolumes.Count > 0 && objNoPostProcessing != null && objNoPostProcessing.Count > 0)
        {
            int ignoredLayer = LayerMask.NameToLayer(ignoredLayerName);

            foreach (GameObject obj in objNoPostProcessing)
            {
                if (obj != null)
                {
                    obj.layer = ignoredLayer;
                }
            }

            foreach (Volume volume in postProcessingVolumes)
            {
                if (volume != null)
                {
                    volume.gameObject.layer = ignoredLayer;
                }
            }
        }
        else
        {
            Debug.LogWarning("NULL Obj or Volume.");
        }
    }
}
