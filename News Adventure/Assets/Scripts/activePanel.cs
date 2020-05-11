using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activePanel : MonoBehaviour
{
    public GameObject panel;
    public void press()
    {
        panel.SetActive(true);
    }
}
