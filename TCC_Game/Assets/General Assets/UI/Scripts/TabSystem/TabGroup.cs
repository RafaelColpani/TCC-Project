using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Linq;
using KevinCastejon.MoreAttributes;

public class TabGroup : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- TABs -", (int)HeaderPlusColor.green)]
    [Tooltip("The list of the toggle group that composes the tabs")]
    public List<_TabButton> tabButtons;
    [Tooltip("The first selected button when changing a tab. Must match with the tab index in tabButtons")]
    public List<GameObject> firstSelectedByTab;

    [Space(10)]

    [Tooltip("The first tab that will be selected when open the settings")]
    public GameObject firstTabSelected;
    [Tooltip("The current selected tab.")]
    [ReadOnly] public _TabButton selectedTab;

    [HeaderPlus(" ", "- CONTENT -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The list of content of each tab. Must match with the correct index in the tabButtons list")]
    public List<GameObject> tabContent;
    public Sprite _normalButton;
    public Sprite _hoverButton;
    public Sprite _selectedButton;
    #endregion

    private PlayerActions playerActions;
    private int selectedToggleIndex = 0;

    #region Unity Methods
    private void Awake()
    {
        playerActions = new PlayerActions();

        playerActions.UI.TabChange.performed += ctx => OnChangeTab(ctx);
    }
    private void Start()
    {
        int tabsQuantity = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            tabsQuantity++;
        }
    }

    private void Update()
    {
        if(selectedTab == null && firstTabSelected != null)
        {
            selectedTab = firstTabSelected.GetComponent<_TabButton>();
            OnTabSelected(selectedTab);
        }
    }
    #endregion

    #region Private Methods
    private void OnChangeTab(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();

        if (value > 0)
            selectedToggleIndex = ChangeSelectedToggleId();

        else
            selectedToggleIndex = ChangeSelectedToggleId(false);

        OnTabSelected(tabButtons[selectedToggleIndex]);
    }

    private int ChangeSelectedToggleId(bool byPositive = true)
    {
        if (byPositive)
        {
            if (selectedToggleIndex == tabButtons.Count() - 1)
                return 0;

            else
                return selectedToggleIndex + 1;
        }

        if (selectedToggleIndex == 0)
            return tabButtons.Count() - 1;

        else
            return selectedToggleIndex - 1;
    }
    #endregion

    #region Public Methods
    public void Subscribe(_TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<_TabButton> { button };
            OnTabSelected(firstTabSelected.GetComponent<_TabButton>());
        }

        tabButtons.Add(button);

        tabButtons.Reverse();
    }

    public void SelectFirstTab()
    {
        OnTabSelected(firstTabSelected.GetComponent<_TabButton>());
    }
    public void OnTabEnter(_TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.buttonBackground.sprite = _hoverButton;
        }
    }
    public void OnTabExit(_TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(_TabButton button)
    {
        if (button == null)
            selectedTab = tabButtons[0];
        else
            selectedTab = button;

        ResetTabs();
        if (button.buttonBackground != null)
        {
            selectedTab.buttonBackground.sprite = _selectedButton;
        }

        int index = button.transform.GetSiblingIndex();
        selectedToggleIndex = index;
        for (int i = 0; i < tabContent.Count; i++)
        {
            if (i == index)
            {
                tabContent[i].SetActive(true);
                if (i > firstSelectedByTab.Count() - 1) continue;

                if (firstSelectedByTab[i] != null)
                {
                    EventSystem.current.SetSelectedGameObject(firstSelectedByTab[i]);
                }
            }
            else
                tabContent[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        
        foreach(_TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }

            if (button.buttonBackground != null)
                button.buttonBackground.sprite = _normalButton;
        }
    }
    #endregion

    #region Enable & Disable
    private void OnEnable()
    {
        playerActions.Enable();
        OnTabSelected(firstTabSelected.GetComponent<_TabButton>());
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    #endregion
}
