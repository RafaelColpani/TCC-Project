using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class StarWarsIntro : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] TextMeshProUGUI introText; // Referência ao componente TextMeshPro onde o texto será exibido
    [SerializeField] float scrollSpeed = 10.0f; // Velocidade de rolagem do texto

    [Header("Text")]
    [SerializeField] string text;

    [Header("Scene")]
    [SerializeField] string sceneName;

    private bool isScrolling = false;

    private void Start()
    {
        // Inicia a rolagem do texto
        StartCoroutine(ScrollText());
    }

    private IEnumerator ScrollText()
    {
        isScrolling = true;
        introText.text = ""; // Limpa o texto no início

        string intro = text;

        for (int i = 0; i < intro.Length; i++)
        {
            introText.text += intro[i];
            yield return new WaitForSeconds(1 / scrollSpeed);
        }

        // Aguarde um momento antes de desativar a rolagem
        yield return new WaitForSeconds(2.0f);
        isScrolling = false;

        // Opcional: Inicie alguma ação, como carregar o próximo nível
    }

    private void SkipIntro()
    {
        if (isScrolling)
        {
            // Se estiver rolando, pule a introdução e complete o texto
            StopAllCoroutines();
            introText.text = text;
            isScrolling = false;

            // Opcional: Inicie alguma ação, como carregar o próximo nível
            SceneManager.LoadScene(sceneName);
        }
    }
}
