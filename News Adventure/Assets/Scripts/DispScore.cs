using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DispScore : MonoBehaviour
{
    public GameObject content;
    private string nomNewsActuelle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("x :" + content.transform.position.x);

        if(content.transform.position.x > -10 && content.transform.position.x < -5)
        {
            this.enabled = true;
            this.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("Feu_Australie").ToString();
        }
        else
        {
            this.enabled = false;
        }
    }
}
