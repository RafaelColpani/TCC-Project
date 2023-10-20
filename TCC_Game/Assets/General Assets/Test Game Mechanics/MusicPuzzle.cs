using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPuzzle : MonoBehaviour
{
    [Header("Obj to interact")]
    [SerializeField] [Tooltip("Array de GameObjects que representam as alavancas na cena")] GameObject[] alavancas; 
    [Space(10)]

    [Header("Puzzle Settings")]
    [SerializeField] [Tooltip("Sequência correta das alavancas a serem ativadas")] string[] sequenciaCorreta; //
    private int indiceSequencia = 0; // Índice atual na sequência
    [Space(10)]

    [Header("Final Events")]
    [SerializeField] GameObject objetoParaAtivar;
    [SerializeField] GameObject objetoParaDesativar;

    private AudioSource audioSource; // Componente AudioSource para tocar a música de vitória
    [SerializeField] bool _MusicVictOnOff = false;
    [SerializeField] AudioClip musicaVitoria; // Música a ser tocada quando o quebra-cabeça for resolvido

    void Start()
    {
        DesativarTodasAlavancas();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AtivarProximaAlavanca();
        }
    }

    void AtivarProximaAlavanca()
    {
        if (indiceSequencia < sequenciaCorreta.Length)
        {
            if (alavancas[indiceSequencia].name == sequenciaCorreta[indiceSequencia])
            {
                alavancas[indiceSequencia].SetActive(true);
                indiceSequencia++;

                if (indiceSequencia == sequenciaCorreta.Length)
                {
                    Debug.Log("Quebra-cabeça resolvido! As alavancas corretas foram ativadas.");
                    objetoParaAtivar.SetActive(!objetoParaAtivar.activeSelf);
                    objetoParaDesativar.SetActive(false);

                    if(_MusicVictOnOff == true)
                    {
                        // Toca a música de vitória
                        if (musicaVitoria != null)
                        {
                            audioSource.clip = musicaVitoria;
                            audioSource.Play();
                        }  
                    }
                }
            }
            
            else
            {
                ReiniciarQuebraCabeca();
            }
        }
    }

    void ReiniciarQuebraCabeca()
    {
        DesativarTodasAlavancas();
        indiceSequencia = 0;
        Debug.Log("Sequência errada! Reiniciando o quebra-cabeça.");
    }

    void DesativarTodasAlavancas()
    {
        foreach (GameObject alavanca in alavancas)
        {
            alavanca.SetActive(true);
        }
    }
}
