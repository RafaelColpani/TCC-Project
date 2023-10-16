using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPuzzle : MonoBehaviour
{
     public GameObject[] objetosParaAtivar; // Array de objetos que representam as alavancas do quebra-cabeça
    public Material alavancaMaterial; // Material roxo da alavanca
    public string[] sequenciaCorreta; // Sequência correta das alavancas a serem ativadas
    public GameObject objetoParaAtivar; // O objeto que será ativado ao resolver o quebra-cabeça
    public GameObject objetoParaDesativar; // O objeto que será desativado ao resolver o quebra-cabeça

    private int indiceSequencia = 0; // Índice atual na sequência
    private AudioSource audioSource; // Componente AudioSource para tocar a música de vitória
    public AudioClip musicaVitoria; // Música a ser tocada quando o quebra-cabeça for resolvido
    private bool resolvido = false; // Para controlar o estado do quebra-cabeça

    void Start()
    {
        DesativarTodasAlavancas();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!resolvido && Input.GetKeyDown(KeyCode.E))
        {
            AtivarProximaAlavanca();
        }
    }

    void AtivarProximaAlavanca()
    {
        if (!resolvido && indiceSequencia < sequenciaCorreta.Length)
        {
            if (objetosParaAtivar[indiceSequencia].name == sequenciaCorreta[indiceSequencia])
            {
                objetosParaAtivar[indiceSequencia].SetActive(true);

                // Altera a cor da alavanca para roxo
                if (alavancaMaterial != null)
                {
                    Renderer renderer = objetosParaAtivar[indiceSequencia].GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = alavancaMaterial;
                    }
                }

                indiceSequencia++;

                if (indiceSequencia == sequenciaCorreta.Length)
                {
                    Debug.Log("Quebra-cabeça resolvido! As alavancas corretas foram ativadas.");
                    objetoParaAtivar.SetActive(!objetoParaAtivar.activeSelf);
                    objetoParaDesativar.SetActive(false);

                    // Toca a música de vitória
                    if (musicaVitoria != null)
                    {
                        audioSource.clip = musicaVitoria;
                        audioSource.Play();
                    }

                    resolvido = true;
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
        foreach (GameObject alavanca in objetosParaAtivar)
        {
            alavanca.SetActive(false);
        }
    }
}
