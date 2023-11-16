using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class VFXColorManager : MonoBehaviour
{
    [HeaderPlus(" ", "- Public Bool -", (int)HeaderPlusColor.yellow)]
    public bool randomColorON = true;
    
    [HeaderPlus(" ", "- VFX GameObject Array -", (int)HeaderPlusColor.yellow)]
    [SerializeField] ParticleSystem[] VFX;
    private Gradient[] initialGradients; 

    private void Start()
    {
        if (VFX == null || VFX.Length == 0)
        {
            VFX = GetComponentsInChildren<ParticleSystem>();
        }

        SaveInitialGradients();
    }

    private void Update()
    {
        if (randomColorON)
        {
            ChangeRandomColors();
        }

        else
        {
            RestoreInitialGradients();
        }
    }

    void SaveInitialGradients()
    {
        initialGradients = new Gradient[VFX.Length];

        for (int i = 0; i < VFX.Length; i++)
        {
            if (VFX[i] != null)
            {
                ParticleSystem.MainModule mainModule = VFX[i].main;
                initialGradients[i] = mainModule.startColor.gradient;
            }
        }
    }

    void ChangeRandomColors()
    {
        for (int i = 0; i < VFX.Length; i++)
        {
            if (VFX[i] != null)
            {
                ParticleSystem.MainModule mainModule = VFX[i].main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(RandomGradient());
            }
        }
    }

    void RestoreInitialGradients()
    {
        for (int i = 0; i < VFX.Length; i++)
        {
            if (VFX[i] != null)
            {
                ParticleSystem.MainModule mainModule = VFX[i].main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(initialGradients[i]);
            }
        }
    }

    Gradient RandomGradient()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Random.ColorHSV(), 0f), new GradientColorKey(Random.ColorHSV(), 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        return gradient;
    }
}