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
    [SerializeField] [Tooltip("Circle Collider 2D in trigger mode")] CircleCollider2D circleCollider2D;
    private GameObject playerTarget;
    [Space(5)]

    [HeaderPlus(" ", "- General Variables -", (int)HeaderPlusColor.green)]
    [SerializeField] float colliderRadiusRaycast = 0.5f;
    [SerializeField] float raycastDist = 2.5f;
    [SerializeField] float speed = 2.5f;
    [Space(5)]

    [HeaderPlus(" ", "- Debug Zone -", (int)HeaderPlusColor.red)]
    [SerializeField] bool _debugPlayerVariables;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D raycast2D;
        playerTarget = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        circleCollider2D.radius = colliderRadiusRaycast;
        
        if (_simplesIA)
        {
            SimpleIA();
        }

        //Debug Zone
        if (_debugPlayerVariables)
        {
            DebugZone();
        }

    }

    #region IA
    void SimpleIA()
    {
        //Get distance (radius) form collider for limit the raycast

        //find player

        //check distance between player and spider

        //go to player
    }
    #endregion

    void DebugZone()
    {
        Debug.Log("Player target debug [ " + playerTarget + " ]");
    }
}
