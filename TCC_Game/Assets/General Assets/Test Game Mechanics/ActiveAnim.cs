using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
   public GameObject objetoAlvo; // Arraste o objeto que você deseja afetar para esse campo no Inspector.
    public string nomeDaAnimacao; // O nome da animação que você deseja ativar.

    private Animator animador;

    private void Start()
    {
        animador = objetoAlvo.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ative o componente Animator no objeto alvo.
            if (animador != null)
            {
                animador.enabled = true;
            }

            // Ative a animação no objeto alvo.
            animador.SetBool(nomeDaAnimacao, true);
        }
    }
}
