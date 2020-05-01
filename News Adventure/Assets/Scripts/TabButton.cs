using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;
    public GameObject panel;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void ClickOn()
    {
        tabGroup.OnTabSelected(this);
    }

}
