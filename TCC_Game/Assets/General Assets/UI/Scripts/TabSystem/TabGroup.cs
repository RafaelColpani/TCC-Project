using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class TabGroup : MonoBehaviour
{
    #region Inspector Vars
    public List<_TabButton> tabButtons;
    public _TabButton selectedTab;
    public List<GameObject> tabContent;
    public Sprite _normalButton, _hoverButton, _selectedButton;
    [SerializeField] GameObject firstTabSelected;
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
        print(tabsQuantity);
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
        selectedTab = button;
        ResetTabs();
        button.buttonBackground.sprite = _selectedButton;
        int index = button.transform.GetSiblingIndex();
        selectedToggleIndex = index;
        for (int i = 0; i < tabContent.Count; i++)
        {
            if (i == index)
            {
                tabContent[i].SetActive(true);
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
             
            button.buttonBackground.sprite = _normalButton;
        }
    }
    #endregion

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
