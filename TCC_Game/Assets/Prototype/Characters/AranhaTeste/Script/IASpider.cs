using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class IASpider : MonoBehaviour
{
    [HeaderPlus(" ", "- Start IA -", (int)HeaderPlusColor.white)]
    [SerializeField] [Tooltip("Simple IA - Find player and go to your position")] bool _simplesIA = true;
    [Space(5)]

    [HeaderPlus(" ", "- Set Itens? -", (int)HeaderPlusColor.magenta)]
    [SerializeField] [Tooltip("This is a trigger obj for the player detect")] Collider2D TriggerCollider2D;
    private GameObject playerTarget;
    [SerializeField] GameObject spiderBody;
    [Space(5)]

    [HeaderPlus(" ", "- General Variables -", (int)HeaderPlusColor.green)]
    [SerializeField] float colliderRadiusRaycast = 0.5f;
    [SerializeField] float raycastDist = 2.5f;
    [SerializeField] float speed = 2.5f;
    private float distance = 2.5f;
    [Space(5)]

    [HeaderPlus(" ", "- Debug Zone -", (int)HeaderPlusColor.red)]
    [SerializeField] bool _debugPlayerVariables;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D raycast2D;
        playerTarget = GameObject.FindWithTag("TargetPlayer");
        
    }

    // Update is called once per frame
    void Update()
    {
        //FindPlayer

       // SimpleIA();
        
        //Debug Zone
        if (_debugPlayerVariables)
        {
            DebugZone();
        }
    }

    #region IA
  //  /*
    private void OnTriggerStay2D(Collider2D collider)
    { 
        if (collider.CompareTag("Player")) 
        {
            print("aaaaaaaa");
            SimpleIA();
        }
        /*
       if(playerTarget)
            print("Simple IA Start" + " " + " [IA Spider.cs] ");
        */

    }
  // */
    
    void SimpleIA()
    {
        print("bbbbbbbb");

        distance = Vector2.Distance(spiderBody.transform.position, playerTarget.transform.position);
        Vector2 direction = playerTarget.transform.position - spiderBody.transform.position;

        spiderBody.transform.position = Vector2.MoveTowards(this.spiderBody.transform.position, playerTarget.transform.position, speed * Time.deltaTime);
    }
    #endregion

    void DebugZone()
    {
        Debug.Log("Player target debug [ " + playerTarget + " ]");
    }

   
}
