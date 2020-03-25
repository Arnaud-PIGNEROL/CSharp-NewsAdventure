using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Enemy> enemy;
    public GameObject[] enemyTiles;                  //Array of enemy prefabs.
    public List<Animaux> animals;
    public GameObject[] animalsTiles;
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
        LayoutAnimalsPosition();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoveEntities());
    }

    IEnumerator MoveEntities()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            enemy[i].MoveEnemy();
            yield return new WaitForSeconds(enemy[i].moveTime);
        }
        
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i].MoveAnimal();
            yield return new WaitForSeconds(animals[i].moveTime);
        }
        
    }

    void LayoutEnemyPosition() // makes spawn the tile at a random position
    {
        //le premier ennemy qui bug je sais pas pourquoi on le fou en arrière plan, methode de shlag mais qui marche
        Vector3 Vposition = new Vector3(-100, -100, 0);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);
        
        Debug.Log("creation des braizes");
        Vposition = new Vector3(2, 2, 10);
        Instantiate(enemyTiles[0], Vposition, Quaternion.identity);

        
        Debug.Log("creation du vent");
        Vposition = new Vector3(-1, -1, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);
        Vposition = new Vector3(-2, -2, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);
        Vposition = new Vector3(-3, -3, 10);
        Instantiate(enemyTiles[1], Vposition, Quaternion.identity);


        Debug.Log("creation du BOSS");
        Vposition = new Vector3(-2, 2, 10);
        Instantiate(enemyTiles[2], Vposition, Quaternion.identity);
    }

    void LayoutAnimalsPosition()
    {
        Debug.Log("creation des koala");
        Vector3 Vposition = new Vector3(3, -3, 10);
        Instantiate(animalsTiles[0], Vposition, Quaternion.identity);

        Debug.Log("creation des walabi");
        Vposition = new Vector3(4, -4, 10);
        Instantiate(animalsTiles[1], Vposition, Quaternion.identity);
    }
}


