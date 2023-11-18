using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSet : MonoBehaviour
{
    enum Season {AUTUMN, WINTER};

    [SerializeField] bool isOozing;
    [SerializeField] Season season;
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
            switch(season){
                case Season.AUTUMN:
                    rend.material.SetInt("_IsAutumn", 1);
                    break;
                case Season.WINTER:
                    rend.material.SetInt("_IsAutumn", 0);
                    break;
                default:
                break;
            }
        }
        if (rend.material.HasInt("_IsOozing"))
            rend.material.SetInt("_IsOozing", isOozing ? 1 : 0);
        if (rend.material.HasFloat("_Random"))
            rend.material.SetFloat("_Random", Random.Range(RandomMinMax.x, RandomMinMax.y));
        if (rend.material.HasFloat("_NoiseScale"))
            rend.material.SetFloat("_NoiseScale", NoiseScale);
        if (rend.material.HasFloat("_TimeMultiplier"))
            rend.material.SetFloat("_TimeMultiplier", TimeMultiplier);

    }
}
