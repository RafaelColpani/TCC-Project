using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerForTest; // Referência ao objeto "Player_for_test".

    /*
    [Header("FPS Display")]
    [SerializeField] TMP_Text fpsText;
    [SerializeField] TMP_Text fpsNameFrame;
    private int lastFPS;
    private float[] frameDeltaTime;
    */

    private void Start()
    {
        //FPS Log
        //frameDeltaTime = new float[50];

        // Obtenha o índice da cena atual.
        int cenaAtualIndex = SceneManager.GetActiveScene().buildIndex;

        // Verifique se o índice da cena está após o índice 3.
        if (cenaAtualIndex > 6)
        {
            // Habilite o script "Bullet" no objeto "Player_for_test".
            Bullet bulletScript = playerForTest.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.enabled = true;
            }
            else
            {
                Debug.LogWarning("O script 'Bullet' não foi encontrado no objeto 'Player_for_test'. Certifique-se de que o objeto e o script estejam configurados corretamente.");
            }
        }
    }

    /*
    void Update()
    {
        //FPS Log
        fpsNameFrame.enabled = true;
        fpsText.enabled = true;
        frameDeltaTime[lastFPS] = Time.deltaTime;
        lastFPS = (lastFPS + 1) % frameDeltaTime.Length;
        fpsText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    float CalculateFPS()
    {
        float totalFPS = 0f;
        foreach (float deltaTime in frameDeltaTime)
        {
            totalFPS += deltaTime;
        }

        return frameDeltaTime.Length / totalFPS;
    }

    public void EnableFPSCounter(bool condition)
    {
        fpsNameFrame.transform.parent.gameObject.SetActive(condition);
        
    }
    */
}
