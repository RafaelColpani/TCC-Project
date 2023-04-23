using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [Tooltip("Componente \"Status\" do player, que provavelmente est� em \"Body\"")]
    [SerializeField] Status status;
    [SerializeField] Slider sld;
    [SerializeField] Image fill;
    [SerializeField] Color[] fillColors = { Color.green, Color.yellow, Color.red };

    private void Start()
    {
        sld = GetComponent<Slider>();

        fill = FindGameObjectByNameInChildren(gameObject, "Fill").GetComponent<Image>();

        sld.minValue = 0;
        sld.maxValue = status.maxHp;

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject po in playerObjects)
        {
            if (po.GetComponent<Status>() != null)
            {
                status = po.GetComponent<Status>();
            }
        }
    }

    private void Update()
    {

        if (status == null)
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject po in playerObjects)
            {
                if (po.GetComponent<Status>() != null)
                {
                    status = po.GetComponent<Status>();
                }
            }
        }
        sld.value = status.hp;
        print("life: " + status.hp);

        if (status.hp <= 2)
            fill.color = fillColors[2]; //red
        else if (status.hp <= (status.maxHp * 0.6))
            fill.color = fillColors[1]; //yellow
        else
            fill.color = fillColors[0]; //ok

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
