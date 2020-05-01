using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public string panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivePanel()
    {
        bool turnBouger = false;

        foreach(GameObject obj in gameObjects)
        {
            if (obj.name != panel)
            {
                obj.SetActive(false);
            }
            else
            {
                if (obj.name == "Decor")
                    turnBouger = true;
                obj.SetActive(true);
            }
        }

        if (turnBouger == true)
        {
            gameObjects[3].SetActive(true);
        }

    }

}
