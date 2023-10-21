using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLevel : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] string tagDoPlayer = "Player";
    [SerializeField] KeyCode teclaAtivacao = KeyCode.E;
    [Space(10)]

    [Header("Obj Settings")]
    [SerializeField] Transform objetoParaBaixo;
    [SerializeField] Transform objetoParaCima;
    [SerializeField] float velocidadeMovimento = 5.0f;
    [SerializeField] GameObject vfxSmoke;
    [Space(10)]

    [Header("Active Bool")]
    private bool ativado = false;
    [SerializeField] bool _AtivacaoUnicaONOFF = true;
    private int countAtivacaoONOFF = 0;
    private bool ativacaoUnica = false;

    [Header("Timer")]
    [SerializeField] float cooldownTempo = 1.0f; // Tempo de cooldown em segundos
    private bool emCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer) && !ativacaoUnica)
        {
            ativado = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer))
        {
            ativado = false;
            vfxSmoke.SetActive(false);
        }
    }

    private void Update()
    {
        if (ativado && Input.GetKeyDown(teclaAtivacao) && !ativacaoUnica && !emCooldown)
        {
            // Move o objetoParaBaixo para baixo
            objetoParaBaixo.Translate(Vector3.down * velocidadeMovimento * Time.deltaTime);
            vfxSmoke.SetActive(true);

            // Move o objetoParaCima para cima
            objetoParaCima.Translate(Vector3.up * velocidadeMovimento * Time.deltaTime);

            // Define ativacaoUnica como verdadeira para impedir ativação adicional
            if (_AtivacaoUnicaONOFF == true)
            {
                ativacaoUnica = true;
            }

            if (_AtivacaoUnicaONOFF == false)
            {
                countAtivacaoONOFF++;
                IniciarCooldown();
            }

            if (countAtivacaoONOFF > 1)
            {
                ativacaoUnica = true;
                _AtivacaoUnicaONOFF = true;
            }
        }
    }

    private void IniciarCooldown()
    {
        emCooldown = true;
        StartCoroutine(EncerrarCooldown());
    }

    private IEnumerator EncerrarCooldown()
    {
        yield return new WaitForSeconds(cooldownTempo);
        emCooldown = false;
    }

}
