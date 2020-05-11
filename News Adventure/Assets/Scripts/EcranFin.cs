using UnityEngine;
using UnityEngine.SceneManagement;

public class EcranFin : MonoBehaviour
{

    public bool end;
    public GameObject victory;
    public GameObject defeat;
    public Player player;
    private string scene_actuelle;

    // Start is called before the first frame update
    void Start()
    {
        victory.SetActive(false);
        defeat.SetActive(false);
        scene_actuelle = SceneManager.GetActiveScene().name;
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

        if (PlayerPrefs.HasKey(scene_actuelle))  // if there is already a score on the scene
        {
            if(PlayerPrefs.GetInt(scene_actuelle) < player.score) // save only if we have made a better score
                PlayerPrefs.SetInt(scene_actuelle, player.score); // save of the final score
        }
        else
        {
            PlayerPrefs.SetInt(scene_actuelle, player.score); // save of the final score
        }

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
