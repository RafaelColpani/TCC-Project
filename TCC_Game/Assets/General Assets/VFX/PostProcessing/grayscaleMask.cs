using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;




public class grayscaleMask : MonoBehaviour
{
    [Header("Post Processing")]
    public Volume volume;
    //private ColorAdjustments colorAdjustments;
    UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;

    [Header("Value")]
    [SerializeField] float sliderSaturation = 0f;

    [Header("Collider")]
    [SerializeField] CircleCollider2D collider2D;
    [Range(-100, 100)] [SerializeField] float valueRadius = 2.5f;

    void Start()
    {
        volume.profile.TryGet<UnityEngine.Rendering.Universal.ColorAdjustments>(out colorAdjustments);
    }

    void Update()
    {

    
        colorAdjustments.saturation.value = sliderSaturation;

    }

   

}
