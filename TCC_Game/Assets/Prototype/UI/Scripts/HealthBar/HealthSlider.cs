using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Status status;
    [SerializeField] Slider sld;
    [SerializeField] Image fill;
    [SerializeField] Color[] fillColors = {Color.white, Color.yellow, Color.red};

    private void Start()
    {
        status = FindObjectOfType<Status>();
        sld = GetComponent<Slider>();

        fill = FindGameObjectByNameInChildren(gameObject, "Fill").GetComponent<Image>();

    }

    private void Update()
    {
        sld.value = status.life;
        print("life: "+status.life);

        if (status.life <= 1)
            fill.color = fillColors[2];
        else if (status.life <= (status.maxLife * 0.75))
            fill.color = fillColors[1];
        else
            fill.color = fillColors[0];

    }

    GameObject FindGameObjectByNameInChildren(GameObject go, string name)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).name == name)
            {
                return go.transform.GetChild(i).gameObject;
            }

            // busca filhos do filho
            GameObject tmp = FindGameObjectByNameInChildren(go.transform.GetChild(i).gameObject, name);

            if (tmp != null)
            {
                return tmp;
            }
        }
        return null;
    }
}
