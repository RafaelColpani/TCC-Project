using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using NaughtyAttributes;
using System.Linq;

public class ObstacleBlock : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- ROOT POSITIONS -", (int)HeaderPlusColor.green)]
    [Tooltip("Sets the position of the most top raycast detection.")]
    [ValidateInput("IsGreaterThanBottom", "Top Raycast must be greater or equal than bottom")]
    [SerializeField] float topRaycastPositionValue;
    [Tooltip("Sets the position of the most bottom raycast detection.")]
    [ValidateInput("IsLowerThanTop", "Bottom Raycast must be lower or equal than top")]
    [SerializeField] float bottomRaycastPositionValue;
    [Tooltip("The x position offset that that raycast will cast.")]
    [MinValue(0)]
    [SerializeField] float xOffset;

    [HeaderPlus(" ", "- RAYCASTS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The number of raycasts that will generate between the bottom and top raycasts. This number DO NOT INCLUDE the top and bottom.")]
    [MinValue(0)]
    [SerializeField] int numberOfMidRaycasts;
    [Tooltip("The sum of the distance for each raycast.")]
    [MinValue(0)]
    [SerializeField] float raycastsDistanceSum;
    [Tooltip("The distance of bottom raycast.")]
    [MinValue(0)]
    [SerializeField] float bottomRaycastDistance;

    [HeaderPlus(" ", "- BODY -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The parent object that contains all the bones of the character.")]
    [SerializeField] Transform body;

    [HeaderPlus(" ", "- DETECTION -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The layers that will be considered as obstacle.")]
    [SerializeField] LayerMask obstacleLayers;

    [HeaderPlus(" ", "- GIZMO -", (int)HeaderPlusColor.white)]
    [SerializeField] bool showGizmo;
    #endregion

    #region VARS
    private List<Vector3> raycastPositions;
    private List<RaycastHit2D> raycastHits;
    #endregion

    #region Naughty Attributos Validators
    private bool IsGreaterThanBottom(float top)
    {
        return top >= bottomRaycastPositionValue;
    }

    private bool IsLowerThanTop(float bottom)
    {
        return bottom <= topRaycastPositionValue;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        SetRaycastPositions();
    }

    private void Update()
    {
        print(HaveAnyRaycastHit());
        if (!HaveAnyRaycastHit()) return;
    }
    #endregion

    #region Methods
    private void SetRaycastPositions()
    {
        raycastHits = new List<RaycastHit2D>();
        raycastPositions = new List<Vector3>();

        int quantityDivision = numberOfMidRaycasts + 1;
        float sumOfMidPositions = (this.topRaycastPositionValue - this.bottomRaycastPositionValue) / quantityDivision;
        float fixedSum = sumOfMidPositions;

        var bottomRaycastPosition = new Vector3(
            body.position.x + xOffset * DirectionMultiplier(),
            body.position.y + this.bottomRaycastPositionValue,
            body.position.z);

        raycastPositions.Add(bottomRaycastPosition);

        for (int i = 0; i < numberOfMidRaycasts; i++)
        {
            raycastPositions.Add(new Vector3(
                body.position.x + xOffset * DirectionMultiplier(),
                body.position.y + this.bottomRaycastPositionValue + sumOfMidPositions,
                body.position.z));

            sumOfMidPositions += fixedSum;
        }

        raycastPositions.Add(new Vector3(
            this.body.position.x + xOffset * DirectionMultiplier(),
            this.body.position.y + topRaycastPositionValue,
            this.body.position.z));
    }

    private bool HaveAnyRaycastHit()
    {
        raycastHits.Clear();
        var distance = bottomRaycastDistance;

        foreach (Vector3 raycastPosition in raycastPositions)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.right * DirectionMultiplier(), distance, obstacleLayers);
            raycastHits.Add(hit);
            distance += raycastsDistanceSum;
        }

        return raycastHits.Any(hit => hit.collider != null && !hit.collider.isTrigger);
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
        if (!showGizmo) return;

        var bottomPos = new Vector3(this.body.position.x + xOffset * DirectionMultiplier(), this.body.position.y + bottomRaycastPositionValue, 0);
        var topPos = new Vector3(this.body.position.x + xOffset * DirectionMultiplier(), this.body.position.y + topRaycastPositionValue, 0);

        Gizmos.color = Color.red;
        //top raycast position
        Gizmos.DrawCube(topPos, new Vector3(0.1f, 0.1f, 0));
        // bottom raycast position
        Gizmos.DrawCube(bottomPos, new Vector3(0.1f, 0.1f, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(bottomPos, new Vector3(bottomPos.x + bottomRaycastDistance * DirectionMultiplier(), bottomPos.y, bottomPos.z));

        int quantityDivision = numberOfMidRaycasts + 1;
        float sumOfMidPositions = (this.topRaycastPositionValue - this.bottomRaycastPositionValue) / quantityDivision;
        float fixedSum = sumOfMidPositions;
        var distance = bottomRaycastDistance + raycastsDistanceSum;

        for (int i = 0; i < numberOfMidRaycasts; i++)
        {
            var newPosition = new Vector3(
                body.position.x + xOffset * DirectionMultiplier(),
                body.position.y + this.bottomRaycastPositionValue + sumOfMidPositions,
                body.position.z);

            Gizmos.DrawLine(newPosition, new Vector3(newPosition.x + distance * DirectionMultiplier(), newPosition.y, newPosition.z));

            sumOfMidPositions += fixedSum;
            distance += raycastsDistanceSum;
        }

        Gizmos.DrawLine(topPos, new Vector3(topPos.x + distance * DirectionMultiplier(), topPos.y, topPos.z));
    }
}
