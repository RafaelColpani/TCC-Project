using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [Header("HEALTH - HP")]
    [Tooltip("Componente \"Status\" do player, que provavelmente est� em \"Body\"")]
    [SerializeField] Status status;
    [SerializeField] Slider sld;
    [SerializeField] Image fill;
    [SerializeField] Color[] fillColors = { Color.green, Color.yellow, Color.red };

    [Header("BELLY - HUNGER")]
    [SerializeField] Color hungerColor;
    public bool isBelly = false;

    public enum Type { Player, Enemy };
    
    [Space(10)]
    public Type type = Type.Player;

    private void Start()
    {
        sld = GetComponent<Slider>();
        fill = FindGameObjectByNameInChildren(gameObject, "Fill").GetComponent<Image>();
        
        if(type == Type.Player)
        {

        }



        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject po in playerObjects)
        {
            if (po.GetComponent<Status>() != null)
            {
                status = po.GetComponent<Status>();
            }
        }

        if (!isBelly)
        {
            sld.minValue = 0;
            sld.maxValue = status.maxHp;
        }
        else
        {
            sld.minValue = 0;
            sld.maxValue = status.maxBelly;
        }
    }

    private void Update()
    {
        if(type == Type.Player)
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

            if (!isBelly)
            {
                sld.value = status.hp;

                if (status.hp <= 2)
                    fill.color = fillColors[2]; //red
                else if (status.hp <= (status.maxHp * 0.6))
                    fill.color = fillColors[1]; //yellow
                else
                    fill.color = fillColors[0]; //ok
            }
            else
            {
                sld.value = status.belly;
            }
        }
        
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
