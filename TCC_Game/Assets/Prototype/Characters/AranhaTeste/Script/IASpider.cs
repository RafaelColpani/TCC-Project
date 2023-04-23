using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class IASpider : MonoBehaviour
{
    #region Inspector VARs
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
    [SerializeField] float waitAttackedTime = 2f;
    [SerializeField] float maxXDistance = 10f;
    private float distance = 2.5f;
    [Space(5)]

    [HeaderPlus(" ", "- Debug Zone -", (int)HeaderPlusColor.red)]
    [SerializeField] bool _debugPlayerVariables;
    #endregion

    #region VARs
    private bool pursuit = true;
    private IsDamagedAndDead damagedAndDead;
    private Vector3 initialPosition;
    #endregion

    void Start()
    {
        playerTarget = GameObject.FindWithTag("TargetPlayer");
        initialPosition = spiderBody.transform.position;
        damagedAndDead = GetComponentInChildren<IsDamagedAndDead>();
    }

    void Update()
    {
        if (PauseController.GetIsPaused()) return;
        if (_debugPlayerVariables)
        {
            DebugZone();
        }
    }

    #region IA
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (PauseController.GetIsPaused()) return;
        if (collider.CompareTag("Player") && damagedAndDead.IsAlive) 
        {
            SimpleIA();
        }
    }
    
    void SimpleIA()
    {
        if (!pursuit) return;

        distance = Vector2.Distance(spiderBody.transform.position, playerTarget.transform.position);
        Vector2 direction = playerTarget.transform.position - spiderBody.transform.position;

        if (spiderBody.transform.position.x > initialPosition.x + maxXDistance || spiderBody.transform.position.x < initialPosition.x - maxXDistance)
        {
            spiderBody.transform.position = Vector2.MoveTowards(this.spiderBody.transform.position, initialPosition, speed * Time.deltaTime);
        }

        else
        {
            spiderBody.transform.position = Vector2.MoveTowards(this.spiderBody.transform.position, playerTarget.transform.position, speed * Time.deltaTime);
        }
    }

    public void Attacked()
    {
        //if (!pursuit) return;

        //pursuit = false;
        //StartCoroutine(WaitAttack());
    }
    #endregion

    #region Corroutines
    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(waitAttackedTime);
        pursuit = true;
    }
    #endregion

    void DebugZone()
    {
        Debug.Log("Player target debug [ " + playerTarget + " ]");
    }
}