using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<_TabButton> tabButtons;
    public _TabButton selectedTab;

    public List<GameObject> tabContent;

    public Sprite _normalButton, _hoverButton, _selectedButton;

    [SerializeField] GameObject firstTabSelected;

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

    public void Subscribe(_TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<_TabButton>();
            tabButtons.Add(button);
            OnTabSelected(firstTabSelected.GetComponent<_TabButton>());
        }

        tabButtons.Add(button);

        tabButtons.Reverse();
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
}
