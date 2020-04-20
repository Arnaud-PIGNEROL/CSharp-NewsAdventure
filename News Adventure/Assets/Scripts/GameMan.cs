using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour
{

    public bool end;
    public GameObject victory;
    public GameObject defeat;

    // Start is called before the first frame update
    void Start()
    {
        victory.SetActive(false);
        defeat.SetActive(false);
        end = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.V) && end == false)
        {
            Win();
            end = true;
        }
         
        if (Input.GetKey(KeyCode.L) && end == false)
        {
            Lose();
            end = true;
        }
    }


    public void Win()
    {
        FindObjectOfType<AudioManager>().Play("Victory");
        victory.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("Game");
        end = true;
    }
    
    public void Lose()
    {
        FindObjectOfType<AudioManager>().Play("Loss");
        defeat.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("Game");
        end = true;
    }

}
