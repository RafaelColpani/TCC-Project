using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System.Linq;

[RequireComponent(typeof(ProceduralTentacle))]
public class TentaclePuzzle : MonoBehaviour
{
    #region Inspector VARS
    [HeaderPlus(" ", "- OBJECTS TO CATCH -", (int)HeaderPlusColor.green)]
    [Tooltip("The list of the object TAGs that the tentacle will try to get for the puzzle.")]
    [SerializeField] string[] objectToCatchTags;

    [HeaderPlus(" ", "- CATCHED OBJECT -", (int)HeaderPlusColor.red)]
    [Tooltip("The speed that the catched object will go to the destination")]
    [SerializeField] float moveSpeed;
    [Tooltip("The distance in sqrMagnitude that is considered that the catchedObject reached the destination.")]
    [SerializeField] float minimumDestinationDistance;

    [HeaderPlus(" ", "- TENTACLE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The transform of the tentacle that is the catcher of the objects. Standard is the effector.")]
    [SerializeField] Transform objectCatcher;
    [Tooltip("The distance in sqrMagnitude that is considered that the tetacle catch an object.")]
    [SerializeField] float minimumCatchDistance;

    [HeaderPlus(" ", "- FRUIT PUZZLE -", (int)HeaderPlusColor.cyan)]
    [SerializeField] private FruitPuzzle fruitPuzzle;
    #endregion

    #region Private VARs
    private List<Transform> objectsInRange = new List<Transform>();

    private Transform objectCatched;
    private Transform destination;

    private bool objectHasBeenCatched = false;
    #endregion

    #region Unity Methods
    private void Update()
    {
        if (PauseController.GetIsPaused()) return;
        if (!ObjectIsCatched()) return;

        ChallengeFlow();
    }
    #endregion

    #region Private Methods
    private bool ObjectIsCatched()
    {
        if (objectsInRange.Count() <= 0) return false;

        foreach (var objectInRange in objectsInRange)
        {
            var distance = (objectInRange.position - objectCatcher.position).sqrMagnitude;
            if (distance <= minimumCatchDistance)
            {
                objectCatched = objectInRange;
                return true;
            }
        }

        return false;
    }

    private void ChallengeFlow()
    {
        // to run only 1 time
        if (!objectHasBeenCatched)
        {
            objectHasBeenCatched = true;
            SetDestination();
        }

        MoveObjectToDestination();

        if (!IsObjectReachedDestination()) return;

        ConcludeChallengeStep();
    }

    private void SetDestination()
    {
        var objectRb = objectCatched.GetComponent<Rigidbody2D>();

        objectRb.gravityScale = 0;
        objectRb.mass = 0;
        objectRb.velocity = Vector2.zero;
        objectRb.isKinematic = true;
        destination = fruitPuzzle.GetCorrectDestination(objectCatched.gameObject);
    }

    private void MoveObjectToDestination()
    {
        var newPosition = Vector3.Lerp(objectCatched.position, destination.position, moveSpeed * Time.deltaTime);
        objectCatched.position = newPosition;
    }

    private bool IsObjectReachedDestination()
    {
        return (objectCatched.position - destination.position).sqrMagnitude <= minimumDestinationDistance;
    }

    private void ConcludeChallengeStep()
    {
        objectHasBeenCatched = false;
        objectCatched.gameObject.SetActive(false);
        destination.gameObject.SetActive(false);
        fruitPuzzle.FruitReachedDestination(objectCatched.gameObject);
    }

    private bool IsRightTag(Collider2D collision)
    {
        for (int i = 0; i < objectToCatchTags.Length; i++)
        {
            if (collision.CompareTag(objectToCatchTags[i]))
                break;

            if (i == objectToCatchTags.Length - 1)
                return false;
        }

        return true;
    }
    #endregion

    #region Unity Events
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsRightTag(collision)) return;

        // adds the transform object to the list of objects to catch if is not already at the list
        if (!objectsInRange.Contains(collision.transform))
            objectsInRange.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsRightTag(collision)) return;

        // removes the transform object of the list of objects if is not already removed
        if (objectsInRange.Contains(collision.transform))
            objectsInRange.Remove(collision.transform);
    }
    #endregion
}
