using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using KevinCastejon.MoreAttributes;
using UnityEngine.InputSystem;

public class InterTutorial : MonoBehaviour
{
    [HeaderPlus(" ", "- TEXT -", (int)HeaderPlusColor.cyan)]
    [SerializeField] TextMeshProUGUI TMP;

    [HeaderPlus(" ", "- SCENE -", (int)HeaderPlusColor.cyan)]
    [SerializeField] string scene = "BasePosCut";

    [HeaderPlus(" ", "- INPUT -", (int)HeaderPlusColor.cyan)]
    [SerializeField] InputActionReference rebindAction;

    [HeaderPlus(" ", "- OBJECT -", (int)HeaderPlusColor.cyan)]
    [SerializeField] GameObject objectToDeactivate;
    [SerializeField] GameObject objectToActivate;

    private readonly string rebindsKey = "playerRebinds";
    private PlayerActions playerActions;

    private void Awake()
    {
        playerActions = new PlayerActions();
        playerActions.Movement.Interaction.performed += _ => PassScene();
    }

    void Start()
    {
        string rebinds = PlayerPrefs.GetString(rebindsKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
            playerActions.LoadBindingOverridesFromJson(rebinds);
        else
            playerActions.RemoveAllBindingOverrides();

        var inputText = InputControlPath.ToHumanReadableString(
            rebindAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        TMP.text = $"< Tecla {inputText} | {BindingUtils.GetButtonImageTextByInputAction(rebindAction.action, 1)} >";
    }

    private void PassScene()
    {
        objectToDeactivate.SetActive(false);
        objectToActivate.SetActive(true);
        SceneManager.LoadScene(scene);
    }

    #region Enable & Disable
    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    #endregion
}
