using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;

public class TutorialTrigger : MonoBehaviour
{
    [HeaderPlus(" ", "- Tutorial GameObject -", (int)HeaderPlusColor.yellow)]
    [SerializeField] GameObject[] objectsToAppear;
    [SerializeField] float timeBetweenAppearances = 0.35f;
    private bool coroutineRunning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !coroutineRunning)
        {
            StartCoroutine(AppearObjectsSequentially());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateAllObjects();
            coroutineRunning = false;
            StopCoroutine(AppearObjectsSequentially());
        }
    }

    IEnumerator AppearObjectsSequentially()
    {
        coroutineRunning = true;

        if (objectsToAppear != null && objectsToAppear.Length > 0)
        {
            foreach (GameObject obj in objectsToAppear)
            {
                if (!coroutineRunning) break;
                if (obj != null)
                {
                    obj.SetActive(true);
                    yield return new WaitForSeconds(timeBetweenAppearances);
                }
            }
        }

        coroutineRunning = false;
        yield return null;
    }

    void DeactivateAllObjects()
    {
        if (objectsToAppear != null && objectsToAppear.Length > 0)
        {
            for (int i = 0; i < objectsToAppear.Length; i++)
            {
                GameObject obj = objectsToAppear[i];

                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
