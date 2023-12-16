using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //To edit in scene
using UnityEngine.InputSystem; //To edit the input system
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;
using UnityEngine.AI;
using UnityEngine.Pool;

public class cs_gameManager : MonoBehaviour
{
    [Tooltip("The script instance")]
    public static cs_gameManager gameManagerInstance;

    [Header("Lists")]
    [Tooltip("Creature scripts in list")]
    public List<GameObject> creaturesList;

    [Header("Object Pools")]
    public ObjectPool<GameObject> fruitPool;
    public int fruitPoolAmount;
    public Transform fruitPoolHolder;
    public int lastFruitPulled;
    public List<GameObject> availableFruit;

    void Awake()
    {
        gameManagerInstance = this;
    }

    void Start()
    {
        fruitPoolAmount = 10;
        GenerateFruitPool();
    }

    void FixedUpdate()
    {
        CreatureCleanup();
    }

    public void GenerateFruitPool()
    {
        /*For each creature in the world, generate a set of fruits that belong to them*/
        fruitPoolHolder = new GameObject("fruitPoolHolder").transform;
        foreach (var creature in creaturesList)
        {
            //If the creature has a fruit
            if (creature.GetComponent<cs_creatureData>().creatureFruit != null)
            {
                for (int i = 0; i < fruitPoolAmount; i++)
                {
                    GameObject newFruit = Instantiate(creature.GetComponent<cs_creatureData>().creatureFruit);
                    newFruit.transform.SetParent(fruitPoolHolder);
                    availableFruit.Add(newFruit);
                    newFruit.SetActive(false);
                }
            }
        }
        /*fruitPoolHolder = new GameObject("fruitPoolHolder").transform;
        for (int i = 0; i < fruitPoolAmount; i++)
        {
            GameObject newFruit = Instantiate(fruitPrefab);
            newFruit.transform.SetParent(fruitPoolHolder);
            availableFruit.Add(newFruit);
            newFruit.SetActive(false);
        }*/
    }

    public void CreateFruit(GameObject fruit, Vector3 spawnPosition)
    {
        fruit.transform.position = spawnPosition;
        fruit.SetActive(true);
    }
    public void DestroyFruit(GameObject fruit)
    {
        fruit.transform.position = fruitPoolHolder.position;
        if (availableFruit.Contains(fruit) == false)
        availableFruit.Add(fruit);
        fruit.SetActive(false);
    }
    public GameObject RequestFruit(List<GameObject> pool)
    {
        GameObject returnValue = null;
        if (pool.Count > 0)
        {
            returnValue = pool[0]; //Grabs first object in pool
            pool.Remove(pool[0]); //Take out of list
        }
        //Add to pool
        return returnValue;
    }

    //Deletes creatures with their scripts if they are deleted
    public void CreatureCleanup()
    {
        for (var creatureInList = creaturesList.Count - 1; creatureInList > -1; creatureInList--)
        {
            //Maybe code to find the creature value
            if (creaturesList[creatureInList] == null)
            {
                creaturesList.RemoveAt(creatureInList);
                //fruitsList.RemoveAt(creatureInList);
            }
        }
    }
}
