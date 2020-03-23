using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    private bool dropAnimal;
    public Animaux animal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getDrop()
    {
        return dropAnimal;
    }

    public void setDrop(bool re_set)
    {
        dropAnimal = re_set;
    }
}
