using KevinCastejon.MoreAttributes;
using UnityEngine;

public enum SlopeState
{
    noSlope,
    ascending,
    descending
}

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
    [Tooltip("The layers that will be considered as slope.")]
    [SerializeField] LayerMask slopeLayers;
    [Tooltip("The state of the current slope status.")]
    [SerializeField][ReadOnly] SlopeState slopeState = SlopeState.noSlope;

    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.cyan)]
    [Tooltip("If the move command in this character is in an Input Handler, check this box.")]
    [SerializeField] bool moveByInputHandler;

    [HeaderPlus(" ", "- BODY -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The parent object that contains all the bones of the character.")]
    [SerializeField] Transform body;

    [HeaderPlus(" ", "- GIZMOS -", (int)HeaderPlusColor.white)]
    [SerializeField] bool showGizmos;
    #endregion

    #region Private Vars
    private MoveCommand moveCommand;
    #endregion

    #region Unity Methods
    private void Start()
    {
        if (moveByInputHandler)
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();
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

        // check asending first
        RaycastHit2D ascendingHit = Physics2D.Raycast(GetRaycastsPosition(), Vector2.right * DirectionMultiplier(), ascendingRaycastDistance, slopeLayers);
        if (ascendingHit.collider != null)
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
        if (descendingHit.collider != null)
        {
            slopeAngle = Vector2.Angle(descendingHit.normal, Vector2.right * DirectionMultiplier());
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
        RaycastHit2D hit = Physics2D.Raycast(GetRaycastsPosition(), Vector2.right * DirectionMultiplier(), ascendingRaycastDistance, slopeLayers);
        var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        var xValue = Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
        var yValue = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * DirectionMultiplier();

        moveCommand.ModifySlopeVelocity(true, xValue, yValue);
    }

    private void DescendingSlopeVelocity()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetRaycastsPosition(), -Vector2.up, descendingRaycastDistance, slopeLayers);
        var slopeAngle = Vector2.Angle(hit.normal, Vector2.right * DirectionMultiplier());

        var xValue = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
        var yValue = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * -DirectionMultiplier();

        moveCommand.ModifySlopeVelocity(true, xValue, yValue);
    }
    #endregion

    #region Aux
    private Vector2 GetRaycastsPosition()
    {
        return new Vector2(body.position.x + raycastsPosition.x * DirectionMultiplier(), body.position.y + raycastsPosition.y);
    }

    private int DirectionMultiplier()
    {
        if (IsFacingRight())
            return 1;

        else
            return -1;
    }

    private bool IsFacingRight()
    {
        return this.transform.localScale.x > 0;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.red;
        var cubePosition = new Vector2(body.position.x + raycastsPosition.x * DirectionMultiplier(), body.position.y + raycastsPosition.y);
        Gizmos.DrawCube(cubePosition, new Vector3(0.02f, 0.02f, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(cubePosition, new Vector3(cubePosition.x + ascendingRaycastDistance * DirectionMultiplier(), cubePosition.y, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cubePosition, new Vector3(cubePosition.x, cubePosition.y - descendingRaycastDistance, 0));
    }
}
