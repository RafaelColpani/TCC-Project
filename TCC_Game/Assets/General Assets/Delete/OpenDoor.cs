using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject objetoAlvo; // Arraste o objeto que você deseja afetar para esse campo no Inspector.

    public Transform portaTransform; // Arraste o Transform da porta para esse campo no Inspector.
    public Vector3 posicaoAberta; // A posição da porta quando estiver aberta.


    private bool portaEstaAberta = false;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !portaEstaAberta)
        {
            // Mova a porta para a posição de abertura.
            objetoAlvo.transform.position = posicaoAberta;
            portaEstaAberta = true;
        }
    }

}
