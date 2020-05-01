using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToScript : MonoBehaviour
{

    private bool selected;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selected = true;
        }
    }

    private void Update()
    {
        Debug.Log(selected);
        if (selected == true)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            selected = false;
        }
    }

}