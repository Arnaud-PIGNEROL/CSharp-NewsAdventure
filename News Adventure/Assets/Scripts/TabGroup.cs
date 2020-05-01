using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> objectToSwap;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabSelected(TabButton button)
    {
        ResetTabs();
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectToSwap.Count; i++)
        {
            if(i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }


    }

    public void ResetTabs()
    {
        foreach(GameObject objec in objectToSwap)
        {
            objec.SetActive(false);
        }
    }

}
