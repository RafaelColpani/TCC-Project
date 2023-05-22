using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using NaughtyAttributes;
using System.Linq;
using UnityEngine.UIElements;

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(CharacterManager))]
public class ObstacleBlock : MonoBehaviour
{
    #region Inspector Vars
    #region Front Block
    [Foldout("Front Block")]
    [HeaderPlus(" ", "- ROOT POSITIONS -", (int)HeaderPlusColor.green)]
    [Tooltip("Sets the position of the most top raycast detection.")]
    [ValidateInput("IsGreaterThanBottom", "Top Raycast must be greater or equal than bottom")]
    [SerializeField] float topFrontRaycastPositionValue;

    [Tooltip("Sets the position of the most bottom raycast detection.")]
    [ValidateInput("IsLowerThanTop", "Bottom Raycast must be lower or equal than top")]
    [Foldout("Front Block")]
    [SerializeField] float bottomFrontRaycastPositionValue;

    [Tooltip("Sets the position of the most bottom raycast detection when in mid air.")]
    [ValidateInput("IsLowerThanTop", "Bottom Raycast must be lower or equal than top")]
    [Foldout("Front Block")]
    [SerializeField] float bottomFrontAirRaycastPositionValue;

    [Tooltip("The x position offset that front raycasts will cast.")]
    [MinValue(0)] [Foldout("Front Block")]
    [SerializeField] float xOffset;

    // -------------

    [Foldout("Front Block")]
    [HeaderPlus(" ", "- RAYCASTS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The number of raycasts that will generate between the bottom and top raycasts. This number DO NOT INCLUDE the top and bottom.")]
    [MinValue(0)]
    [SerializeField] int numberOfFrontMidRaycasts;

    [Tooltip("The distance of each front raycasts.")]
    [MinValue(0)] [Foldout("Front Block")]
    [SerializeField] float frontRaycastsDistance;

    [Foldout("Front Block")]
    [Tooltip("The maximum angle allowed for the character walk in a slope, in degrees.")]
    [SerializeField] float maxSlopeAngle;
    #endregion

    // -------------

    #region Ceiling Block
    [Foldout("Ceiling Block")]
    [HeaderPlus(" ", "- ROOT POSITIONS -", (int)HeaderPlusColor.green)]
    [Tooltip("Sets the posiiton of the most leading (most left) raycast position.")]
    [ValidateInput("IsLowerThanTrailing", "Leading Raycast must be lower or equal than trailing")]
    [SerializeField] float leadingTopRaycastPositionValue;

    [Tooltip("Sets the posiiton of the most trailing (most right) raycast position.")]
    [Foldout("Ceiling Block")] [ValidateInput("IsGreaterThanLeading", "Trailing Raycast must be greater or equal than leading")]
    [SerializeField] float trailingTopRaycastPositionValue;

    [Tooltip("The y position offset that upper raycasts will cast.")]
    [MinValue(0)] [Foldout("Ceiling Block")]
    [SerializeField] float yOffset;

    // -------------

    [Foldout("Ceiling Block")]
    [HeaderPlus(" ", "- RAYCASTS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The number of raycasts that will generate between the leading and trailing raycasts. This number DO NOT INCLUDE the leading and trailing.")]
    [MinValue(0)]
    [SerializeField] int numberOfTopMidRaycasts;

    [Tooltip("The distance of each top raycasts.")]
    [MinValue(0)]
    [Foldout("Ceiling Block")]
    [SerializeField] float topRaycastsDistance;
    #endregion

    // -------------

    #region General
    [HeaderPlus(" ", "- GENERAL DETECTION -", (int)HeaderPlusColor.white)]
    [Tooltip("The layers that will be considered as obstacle.")]
    [SerializeField] LayerMask obstacleLayers;
    [Tooltip("Tells if this object is a player or an enemy.")]
    [SerializeField] bool isEnemy;

    [HeaderPlus(" ", "- GIZMO -", (int)HeaderPlusColor.white)]
    [SerializeField] bool showGizmo;
    [SerializeField] bool showBottomMidAirRaycastPosition;
    #endregion
    #endregion

    #region VARS
    private List<Vector3> frontRaycastPositions;
    private List<Vector3> topRaycastPositions;

    private MoveCommand moveCommand;
    private GravityController gravityController;
    private CharacterManager characterManager;

    private Transform body;

    private bool moveByInputHandler;
    #endregion

    #region Naughty Attributes Validators
    private bool IsGreaterThanBottom(float top)
    {
        return top >= bottomFrontRaycastPositionValue;
    }

    private bool IsLowerThanTop(float bottom)
    {
        return bottom <= topFrontRaycastPositionValue;
    }

    private bool IsGreaterThanLeading(float trailing)
    {
        return trailing >= leadingTopRaycastPositionValue;
    }

    private bool IsLowerThanTrailing(float leading)
    {
        return leading <= trailingTopRaycastPositionValue;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();

        this.body = characterManager.Body;
        this.moveByInputHandler = characterManager.CommandsByInputHandler;

        if (moveByInputHandler)
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();

        gravityController = GetComponent<GravityController>();
    }

    private void Update()
    {
        // front raycasts
        SetFrontRaycastPositions();
        SetMovementBlock(HaveHitedObstacle());

        // top raycasts
        SetTopRaycastsPosition();
        SetUpperVelocityBlock(HaveHitedCeiling());

    }
    #endregion

    #region Methods
    #region Front Raycasts
    private void SetFrontRaycastPositions()
    {
        frontRaycastPositions = new List<Vector3>();

        int quantityDivision = numberOfFrontMidRaycasts + 1;
        float sumOfMidPositions = 0; 
        float fixedSum = 0;
        var bottomRaycastPosition = Vector3.zero;

        if (gravityController.Velocity.y > 0)
        {
            bottomRaycastPosition = new Vector3(
            body.position.x + xOffset * DirectionMultiplier(),
            body.position.y + this.bottomFrontAirRaycastPositionValue,
            body.position.z);

            sumOfMidPositions = (this.topFrontRaycastPositionValue - this.bottomFrontAirRaycastPositionValue) / quantityDivision;
        }

        else
        {
            bottomRaycastPosition = new Vector3(
            body.position.x + xOffset * DirectionMultiplier(),
            body.position.y + this.bottomFrontRaycastPositionValue,
            body.position.z);

            sumOfMidPositions = (this.topFrontRaycastPositionValue - this.bottomFrontRaycastPositionValue) / quantityDivision;
        }

        fixedSum = sumOfMidPositions;
        frontRaycastPositions.Add(bottomRaycastPosition);

        for (int i = 0; i < numberOfFrontMidRaycasts; i++)
        {
            frontRaycastPositions.Add(new Vector3(
                body.position.x + xOffset * DirectionMultiplier(),
                body.position.y + this.bottomFrontRaycastPositionValue + sumOfMidPositions,
                body.position.z));

            sumOfMidPositions += fixedSum;
        }

        frontRaycastPositions.Add(new Vector3(
            this.body.position.x + xOffset * DirectionMultiplier(),
            this.body.position.y + topFrontRaycastPositionValue,
            this.body.position.z));
    }

    public bool HaveHitedObstacle()
    {
        foreach (Vector3 raycastPosition in frontRaycastPositions)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.right * DirectionMultiplier(), frontRaycastsDistance, obstacleLayers);

            if (!IsSlope(hit) && HitedObstacle(hit))
            {
                return true;
            }
        }

        return false;
    }

    private void SetMovementBlock(bool isHit)
    {
        if (isEnemy) return;

        if (isHit)
        {
            moveCommand.CanMove = false;
            return;
        }

        moveCommand.CanMove = true;
    }

    #region Front Aux
    private bool IsSlope(RaycastHit2D hit)
    {
        float angle = Vector2.Angle(hit.normal, Vector2.up);

        return angle <= maxSlopeAngle;
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
    #endregion

    #region Top Raycasts
    private void SetTopRaycastsPosition()
    {
        topRaycastPositions = new List<Vector3>();

        int quantityDivision = numberOfTopMidRaycasts + 1;
        float sumOfMidPositions = (this.trailingTopRaycastPositionValue - this.leadingTopRaycastPositionValue) / quantityDivision;
        float fixedSum = sumOfMidPositions;

        var trailingRaycastPosition = new Vector3(
            body.position.x + this.leadingTopRaycastPositionValue,
            body.position.y + yOffset,
            body.position.z);

        topRaycastPositions.Add(trailingRaycastPosition);

        for (int i = 0; i < numberOfTopMidRaycasts; i++)
        {
            topRaycastPositions.Add(new Vector3(
                body.position.x + this.leadingTopRaycastPositionValue + sumOfMidPositions,
                body.position.y + yOffset,
                body.position.z));

            sumOfMidPositions += fixedSum;
        }

        frontRaycastPositions.Add(new Vector3(
            this.body.position.x + trailingTopRaycastPositionValue,
            this.body.position.y + yOffset,
            this.body.position.z));
    }

    private bool HaveHitedCeiling()
    {
        foreach (Vector3 raycastPosition in topRaycastPositions)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.up, topRaycastsDistance, obstacleLayers);

            if (HitedObstacle(hit))
            {
                return true;
            }
        }

        return false;
    }

    private void SetUpperVelocityBlock(bool isHit)
    {
        if (!isHit && gravityController.Velocity.y <= 0)
        {
            gravityController.SetCanJump(true);
        }

        else if (isHit && gravityController.Velocity.y <= 0)
        {
            gravityController.SetCanJump();
            
        }

        else if (isHit && gravityController.Velocity.y > 0)
        {
            gravityController.SetYVelocity(0);
        }
    }
    #endregion

    private bool HitedObstacle(RaycastHit2D hit)
    {
        return hit.collider != null && !hit.collider.isTrigger;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;

        var gizmoBody = GetComponent<CharacterManager>().Body;

        if (showBottomMidAirRaycastPosition)
        {
            Gizmos.color = Color.red;
            var bottomMidAir = new Vector3(gizmoBody.position.x + xOffset * DirectionMultiplier(), gizmoBody.position.y + bottomFrontAirRaycastPositionValue, 0);
            //top raycast position
            Gizmos.DrawCube(bottomMidAir, new Vector3(0.1f, 0.1f, 0));
        }

        #region Front Raycasts
        var bottomPos = new Vector3(gizmoBody.position.x + xOffset * DirectionMultiplier(), gizmoBody.position.y + bottomFrontRaycastPositionValue, 0);
        var topPos = new Vector3(gizmoBody.position.x + xOffset * DirectionMultiplier(), gizmoBody.position.y + topFrontRaycastPositionValue, 0);

        Gizmos.color = Color.red;
        //top raycast position
        Gizmos.DrawCube(topPos, new Vector3(0.1f, 0.1f, 0));
        // bottom raycast position
        Gizmos.DrawCube(bottomPos, new Vector3(0.1f, 0.1f, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(bottomPos, new Vector3(bottomPos.x + frontRaycastsDistance * DirectionMultiplier(), bottomPos.y, bottomPos.z));

        int quantityDivision = numberOfFrontMidRaycasts + 1;
        float sumOfMidPositions = (this.topFrontRaycastPositionValue - this.bottomFrontRaycastPositionValue) / quantityDivision;
        float fixedSum = sumOfMidPositions;

        for (int i = 0; i < numberOfFrontMidRaycasts; i++)
        {
            var newPosition = new Vector3(
                gizmoBody.position.x + xOffset * DirectionMultiplier(),
                gizmoBody.position.y + this.bottomFrontRaycastPositionValue + sumOfMidPositions,
                gizmoBody.position.z);

            Gizmos.DrawLine(newPosition, new Vector3(newPosition.x + frontRaycastsDistance * DirectionMultiplier(), newPosition.y, newPosition.z));

            sumOfMidPositions += fixedSum;
        }

        Gizmos.DrawLine(topPos, new Vector3(topPos.x + frontRaycastsDistance * DirectionMultiplier(), topPos.y, topPos.z));
        #endregion

        #region Top Raycasts
        var leadingPos = new Vector3(gizmoBody.position.x + leadingTopRaycastPositionValue, gizmoBody.position.y + yOffset, 0);
        var trailingPos = new Vector3(gizmoBody.position.x + trailingTopRaycastPositionValue, gizmoBody.position.y + yOffset, 0);

        Gizmos.color = Color.yellow;
        //top raycast position
        Gizmos.DrawCube(leadingPos, new Vector3(0.1f, 0.1f, 0));
        // bottom raycast position
        Gizmos.DrawCube(trailingPos, new Vector3(0.1f, 0.1f, 0));

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(leadingPos, new Vector3(leadingPos.x, leadingPos.y + topRaycastsDistance, leadingPos.z));

        int quantityTopDivision = numberOfTopMidRaycasts + 1;
        float sumOfMidTopPositions = (this.trailingTopRaycastPositionValue - this.leadingTopRaycastPositionValue) / quantityTopDivision;
        float fixedTopSum = sumOfMidTopPositions;

        for (int i = 0; i < numberOfTopMidRaycasts; i++)
        {
            var newPosition = new Vector3(
                gizmoBody.position.x + this.leadingTopRaycastPositionValue + sumOfMidTopPositions,
                gizmoBody.position.y + yOffset,
                gizmoBody.position.z);

            Gizmos.DrawLine(newPosition, new Vector3(newPosition.x, newPosition.y + topRaycastsDistance, newPosition.z));

            sumOfMidTopPositions += fixedTopSum;
        }

        Gizmos.DrawLine(trailingPos, new Vector3(trailingPos.x, trailingPos.y + topRaycastsDistance, trailingPos.z));
        #endregion
    }
}
