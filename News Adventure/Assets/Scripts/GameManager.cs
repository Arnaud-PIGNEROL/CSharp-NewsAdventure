using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Enemy> enemy;
    //public List<Enemy> brasiers;
    //public List<Enemy> tornades;
    //public GameObject Braize;
    //public GameObject Vent;
    public GameObject[] enemyTiles;                  //Array of enemy prefabs.
    public GameObject dontDestroy;
    public float turnDelay = 0.1f;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)            //Check if instance already exists
            instance = this;
        else if (instance != this)
            Destroy(dontDestroy);         //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.


        DontDestroyOnLoad(dontDestroy);

        LayoutEnemyPosition();
        //LayoutEnemyPositionBrasier();
        //LayoutEnemyPositionVent();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        /*
        // brasiers
        yield return new WaitForSeconds(turnDelay);
        if (brasiers.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < brasiers.Count; i++)
        {
            brasiers[i].MoveEnemy();
            yield return new WaitForSeconds(brasiers[i].moveTime);
        }

        // tornades
        if (tornades.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < tornades.Count; i++)
        {
            tornades[i].MoveEnemy();
            yield return new WaitForSeconds(tornades[i].moveTime);
        }
        */

        for (int i = 0; i < enemy.Count; i++)
        {
            Debug.Log("i="+i);
            enemy[i].MoveEnemy();
            yield return new WaitForSeconds(enemy[i].moveTime);
        }
        Debug.Log("___________________________________________________________");
    }

    void LayoutEnemyPositionBrasier() // makes spawn the tile at a random position
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("creation des braizes");
            Vector3 Vposition = new Vector3(i, i, 10);
           // Instantiate(Braize, Vposition, Quaternion.identity);
        }
    }

    void LayoutEnemyPositionVent() // makes spawn the tile at a random position
    {
        for (int i = 0; i > -2; i--)
        {
            Debug.Log("creation du vent");
            Vector3 Vposition = new Vector3(i, i, 10);
            //Instantiate(Vent, Vposition, Quaternion.identity);
        }
    }

    void LayoutEnemyPosition() // makes spawn the tile at a random position
    {
        //le premier ennemy qui bug je sais pas pourquoi on le fou en arrière plan, methode de shlag mais qui marche
        Vector3 Vposition = new Vector3(-100, -100, 0);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);

        Debug.Log("creation des braizes");
        Vposition = new Vector3(1, 1, 10);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);
        Vposition = new Vector3(2, 2, 10);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);
        Vposition = new Vector3(3, 3, 10);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);
        Vposition = new Vector3(4, 4, 10);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);

        
        Debug.Log("creation du vent");
        Vposition = new Vector3(-1, -1, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);
        Vposition = new Vector3(-2, -3, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);
        Vposition = new Vector3(-4, -2, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);
    }
    
}
