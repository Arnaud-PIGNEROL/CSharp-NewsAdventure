using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string MapToLoad;

   //Function called when page pressed
    public void change()
    {
        SceneManager.LoadScene(MapToLoad);
    }
}
