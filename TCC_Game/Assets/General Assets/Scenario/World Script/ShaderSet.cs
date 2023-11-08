using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSet : MonoBehaviour
{
    [SerializeField] bool isAutumn;
    [SerializeField] Vector2 RandomMinMax;
    [SerializeField] float NoiseScale;
    [Range(0,1)]
    [SerializeField] float TimeMultiplier;

    private Renderer rend;

    void Start()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        if (rend.material.HasInt("_IsAutumn"))
        {
            print("hasautumn");
            int autumn;
            autumn = isAutumn ? 1:0;
            rend.material.SetInt("_IsAutumn", autumn);
        }
        if (rend.material.HasFloat("_Random"))
            rend.material.SetFloat("_Random", Random.Range(RandomMinMax.x, RandomMinMax.y));
        if (rend.material.HasFloat("_NoiseScale"))
            rend.material.SetFloat("_NoiseScale", NoiseScale);
        if (rend.material.HasFloat("_TimeMultiplier"))
            rend.material.SetFloat("_TimeMultiplier", TimeMultiplier);

    }
}
