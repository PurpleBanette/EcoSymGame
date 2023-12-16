using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class cs_fruitData : MonoBehaviour
{
    [SerializeField] protected string fruitName;
    [SerializeField] protected CreatureFruitID fruitID;
    [SerializeField] protected float fruitDecayTimer;
    [SerializeField] protected float fruitTimeLimit;
    [SerializeField] protected int fruitSaturation;
    public bool fruitSpawned;

    // Start is called before the first frame update
    private void Awake()
    {
        FruitStats();
        //Apply the fruit tag on spawn
        gameObject.tag = "Fruit";
        AssignFruitIDValues();
    }
    void Start()
    {
        FruitTimer();
    }

    public virtual void FruitTimer()
    {
        fruitDecayTimer = fruitTimeLimit;
        fruitTimeLimit = 10f;
    }

    public virtual void AssignFruitIDValues()
    {
        fruitName = "Null";
        fruitID = CreatureFruitID.f_null;
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
