using UnityEngine;
using UnityEngine.EventSystems;

public class OpenClose : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] GameObject dimBG;
    [SerializeField] GameObject firstSelected;
    public void Switch()
    {
        if (panel.activeSelf)
        {
            print("sai");
            panel.SetActive(false);
            if (dimBG) dimBG.SetActive(false);
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
        else
        {
            print("sai2");
            panel.SetActive(true);
            if (dimBG) dimBG.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}
