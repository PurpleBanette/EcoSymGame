using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class cs_fruitData : MonoBehaviour
{
    [SerializeField] float fruitDecayTimer;
    [SerializeField] float fruitTimeLimit;
    [SerializeField] protected int fruitSaturation;

    public bool fruitSpawned;

    // Start is called before the first frame update
    private void Awake()
    {
        FruitStats();
        //Apply the fruit tag on spawn
        gameObject.tag = "Fruit";
    }
    void Start()
    {
        fruitTimeLimit = 10f;
        fruitDecayTimer = fruitTimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        FruitDecay();
    }

    public virtual void FruitStats()
    {
        fruitSaturation = 1;
    }

    void FruitDecay()
    {
        if (!fruitSpawned) return; //If not spawned, do nothing
        fruitDecayTimer -= Time.deltaTime;
        if (fruitDecayTimer <= 0f)
        {

            fruitDecayTimer = fruitTimeLimit; //Reset timer

            /*Makes the fruit dissapear (Disabled) after timer ends*/
            fruitSpawned = false;
            FindObjectOfType<cs_gameManager>().DestroyFruit(gameObject);
            
        }
    }
}
