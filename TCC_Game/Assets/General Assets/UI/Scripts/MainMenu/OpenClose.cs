using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] GameObject dimBG;
    public void Switch()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            if (dimBG) dimBG.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            if (dimBG) dimBG.SetActive(true);
        }
        //KeepThisActive();
    }

    //public bool KeepThisActive()
    //{
    //    for (int i = 0; i < transform.root.childCount; i++)
    //    {
    //        if (panel.activeSelf &&
    //            transform.root.GetChild(i).TryGetComponent<OpenClose>(out OpenClose oc)  &&
    //            transform.root.GetChild(i).GetComponent<OpenClose>().panel.gameObject.activeSelf &&
    //            transform.root.GetChild(i).GetComponent<OpenClose>().panel.gameObject != this)
    //        {
    //            transform.root.GetChild(i).GetComponent<OpenClose>().panel.gameObject.SetActive(false);
    //        }
    //    }
    //    return false;
    //}
}
