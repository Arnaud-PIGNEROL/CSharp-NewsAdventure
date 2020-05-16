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
        if (!PlayerPrefs.HasKey("Feu_Australie"))
            PlayerPrefs.SetInt("Feu_Australie", 0);
        if (!PlayerPrefs.HasKey("Corona"))
            PlayerPrefs.SetInt("Corona", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("x :" + content.transform.position.x);

        if(content.transform.position.x > -10 && content.transform.position.x < -5)
        {
            this.GetComponent<UnityEngine.UI.Text>().text = ("Best score : " + PlayerPrefs.GetInt("Feu_Australie").ToString());
        }
        else if (content.transform.position.x > -25 && content.transform.position.x < -20)
        {
            this.GetComponent<UnityEngine.UI.Text>().text = ("Best score : " + PlayerPrefs.GetInt("Corona").ToString());
        }
        else
        {
            this.GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }
}
