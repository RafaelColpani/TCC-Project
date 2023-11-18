using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum SlopeState
{
    noSlope,
    ascending,
    descending
}

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(CharacterManager))]
public class SlopeController : MonoBehaviour
{
    #region Inspector Var
    [HeaderPlus(" ", "- RAYCASTS -", (int)HeaderPlusColor.green)]
    [Tooltip("The position of both raycasts for ascending and desending slopes, based on body posittion.")]
    [SerializeField] Vector2 raycastsPosition;
    [Tooltip("The distance of the raycast that checks for a slope when ascending.")]
    [SerializeField] float ascendingRaycastDistance;
    [Tooltip("The distance of the raycast that checks for a slope when descending.")]
    [SerializeField] float descendingRaycastDistance;

    [HeaderPlus(" ", "- SLOPE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The max angle that character can climb and/or descend a slope.")]
    [Range(0f, 90f)] [SerializeField] float slopeMaxAngle;
    [Tooltip("The state of the current slope status.")]
    [SerializeField][ReadOnly] SlopeState slopeState = SlopeState.noSlope;

    [HeaderPlus(" ", "- GIZMOS -", (int)HeaderPlusColor.white)]
    [SerializeField] bool showGizmos;
    #endregion

    #region Private Vars
    private MoveCommand moveCommand;
    private GravityController gravityController;
    private CharacterManager characterManager;

    private Transform body;

    private LayerMask slopeLayers;

    private bool moveByInputHandler;
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();

        this.body = characterManager.Body;
        this.slopeLayers = characterManager.GroundLayers;
        this.moveByInputHandler = characterManager.CommandsByInputHandler;

        if (moveByInputHandler)
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();

        else if (GetComponent<ChickenFruitFollow>() != null)
            moveCommand = GetComponent<ChickenFruitFollow>().GetMoveCommand();

        else if (GetComponent<GuidePlayerAI>() != null)
            moveCommand = GetComponent<GuidePlayerAI>().GetMoveCommand();

        else
            moveCommand = GetComponent<EnemyCommands>().GetMovementCommand();

        gravityController = GetComponent<GravityController>();
    }

    private void Update()
    {
        SetSlopeState();
        SlopeStateCommand();
    }
    #endregion

    #region Methods
    private void SetSlopeState()
    {
        var slopeAngle = 0f;

        // apply no slope statement if player is jumping
        if (gravityController.Velocity.y > 0)
        {
            slopeState = SlopeState.noSlope;
            return;
        }

        // check asending first
        RaycastHit2D ascendingHit = Physics2D.Raycast(GetRaycastsPosition(), Vector2.right * characterManager.DirectionMultiplier(), ascendingRaycastDistance, slopeLayers);
        if (ascendingHit.collider != null && !ascendingHit.collider.isTrigger)
        {
            slopeAngle = Vector2.Angle(ascendingHit.normal, Vector2.up);
            if (slopeAngle <= slopeMaxAngle)
            {
                slopeState = SlopeState.ascending;
                return;
            }
        }

        // check descending next
        RaycastHit2D descendingHit = Physics2D.Raycast(GetRaycastsPosition(), -Vector2.up, descendingRaycastDistance, slopeLayers);
        if (descendingHit.collider != null && !descendingHit.collider.isTrigger)
        {
            slopeAngle = Vector2.Angle(descendingHit.normal, Vector2.right * characterManager.DirectionMultiplier());
            if (slopeAngle <= slopeMaxAngle)
            {
                slopeState = SlopeState.descending;
                return;
            }
        }

        // it is not on a slope
        slopeState = SlopeState.noSlope;
    }

    private void SlopeStateCommand()
    {
        switch (slopeState)
        {
            case SlopeState.noSlope:
                moveCommand.ModifySlopeVelocity();
                break;

            case SlopeState.ascending:
                AscendingSlopeVelocity();
                break;

            case SlopeState.descending:
                DescendingSlopeVelocity();
                break;
        }
    }

    private void AscendingSlopeVelocity()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetRaycastsPosition(), Vector2.right * characterManager.DirectionMultiplier(), ascendingRaycastDistance, slopeLayers);
        var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        var xValue = Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
        var yValue = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * characterManager.DirectionMultiplier();

        moveCommand.ModifySlopeVelocity(true, xValue, yValue);
    }

    private void DescendingSlopeVelocity()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetRaycastsPosition(), -Vector2.up, descendingRaycastDistance, slopeLayers);
        var slopeAngle = Vector2.Angle(hit.normal, Vector2.right * characterManager.DirectionMultiplier());

        var xValue = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
        var yValue = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * -characterManager.DirectionMultiplier();

        moveCommand.ModifySlopeVelocity(true, xValue, yValue);
    }
    #endregion

    #region Aux
    private Vector2 GetRaycastsPosition()
    {
        return new Vector2(body.position.x + raycastsPosition.x * characterManager.DirectionMultiplier(), body.position.y + raycastsPosition.y);
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        var gizmoBody = GetComponent<CharacterManager>().Body;

        Gizmos.color = Color.red;
        var cubePosition = new Vector2(gizmoBody.position.x + raycastsPosition.x * GetComponent<CharacterManager>().DirectionMultiplier(), gizmoBody.position.y + raycastsPosition.y);
        Gizmos.DrawCube(cubePosition, new Vector3(0.02f, 0.02f, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(cubePosition, new Vector3(cubePosition.x + ascendingRaycastDistance * GetComponent<CharacterManager>().DirectionMultiplier(), cubePosition.y, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cubePosition, new Vector3(cubePosition.x, cubePosition.y - descendingRaycastDistance, 0));
    }
}
