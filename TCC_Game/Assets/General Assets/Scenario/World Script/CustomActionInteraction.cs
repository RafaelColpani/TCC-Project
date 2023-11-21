using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using KevinCastejon.MoreAttributes;

public class CustomActionInteraction : MonoBehaviour, IInteractable
{
    #region Inspector VARs
    [HeaderPlus(" ", "- INTERACTION -", (int)HeaderPlusColor.green)]
    [Tooltip("Tells if the totem will interaction only one time." +
        " If its in a puzzle that requires a hard reset (like music puzzle), mark it true.")]
    [SerializeField] bool singleActivation = true;
    [Tooltip("Tells if the object has a UI element to pop up when player steps in this object." +
        " The script will get the child object marked with 'UIPopUp' tag.")]
    [SerializeField] bool hasUIPopUp = true;
    [Tooltip("Tells if the interaction will make the object change camera")]
    [SerializeField] bool canChangeCam = false;
    [SerializeField] ChangeCam changeCam;

    [HeaderPlus(" ", "- SHADER -", (int)HeaderPlusColor.cyan)]
    [Tooltip("Tells if the object has a shader to reproduce when interacted.")]
    [SerializeField] bool isShaderInteraction = true;
    [Tooltip("The time that the shader interaction will activate.")]
    [SerializeField] float activationSpeed = 1;

    [HeaderPlus(" ", "- ACTIONS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The list of events that the totem will do when activated.")]
    [SerializeField] UnityEvent action;
    #endregion

    #region Private VARs
    private readonly string popUpTag = "UIPopUp";

    private Renderer rend;
    private GameObject uiPopUp;

    private bool isInteracted = false;

    #endregion

    #region Unity Methods
    private void Start()
    {
        if (isShaderInteraction)
            rend = GetComponent<Renderer>();

        // get the ui pop up element
        if (hasUIPopUp)
        {
            foreach (Transform child in this.transform)
            {
                if (!child.gameObject.CompareTag(popUpTag)) continue;

                uiPopUp = child.gameObject;
                break;
            }
        }
    }
    #endregion

    #region Public Methods
    public void ResetInteraction()
    {
        if (!isInteracted) return;

        isInteracted = false;

        if (!isShaderInteraction) return;

        StopAllCoroutines();
        StartCoroutine(Activation(false));
    }
    #endregion

    #region Interface
    public void Interact()
    {
        if (singleActivation && isInteracted) return;

        if (canChangeCam)
            changeCam.changeCam(ChangeCam.CamType.TEMP, false);

        // deactivate ui pop up
        if (singleActivation && hasUIPopUp)
            uiPopUp.SetActive(false);

        // show the interaction shader
        if (singleActivation && isShaderInteraction)
            StartCoroutine(Activation(true));

        // invoke interaction actions
        isInteracted = true;
        action.Invoke();
    }
    #endregion

    #region Coroutines
    public IEnumerator Activation(bool isActive)
    {
        if (isActive == true)
        {
            while (rend.material.GetFloat("_Floating") < 1)
            {
                rend.material.SetFloat("_Floating",
                    rend.material.GetFloat("_Floating") + Time.deltaTime * activationSpeed);

                yield return null;
            }
        }

        else
        {
            while (rend.material.GetFloat("_Floating") > 0)
            {
                rend.material.SetFloat("_Floating",
                    rend.material.GetFloat("_Floating") - Time.deltaTime * activationSpeed);

                yield return null;
            }
        }

    }
    #endregion

    #region Unity Events
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!hasUIPopUp) return;
        if (singleActivation && isInteracted) return;
        if (uiPopUp.activeInHierarchy) return;

        uiPopUp.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!hasUIPopUp) return;
        if (singleActivation && isInteracted) return;

        uiPopUp.SetActive(false);
    }
    #endregion
}
