using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton selectedTab;

    public List<GameObject> tabContent;

    public Sprite _normalButton, _hoverButton, _selectedButton;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.buttonBackground.sprite = _hoverButton;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
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
        
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
             
            button.buttonBackground.sprite = _normalButton;
        }
    }
}
