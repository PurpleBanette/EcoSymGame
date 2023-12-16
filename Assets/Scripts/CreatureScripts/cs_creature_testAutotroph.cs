using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_creature_testAutotroph : cs_creatureData
{
    private void Start()
    {
        gameManagerScript.creaturesList.Add(gameObject); //Adds creature to gameManager
        //SpawnFruitPool();
        //gameManagerScript.fruitsList.Add(creatureFruit); //Adds fruit prefab to game manager

        IndividualCreatureBaseStatGrowth();
        creatureHpCurrent = creatureHpMax;
        ApplyDexToNavSpeed();
    }

    public override void CreatureMoveRangeDistribution() //Add unique move range
    {
        creatureMoveRange = 3f;
    }

    public override void AssignCreatureIDValues() //Add unique name and ID
    {
        creatureName = "Test Autotroph Plant";
        creatureType = CreatureType.t_autotroph;
        creatureID = CreatureID.c_testAutotroph;
        creatureFruitID = CreatureFruitID.f_testAutotroph;
    }

    /*private void SpawnFruitPool()
    {
        GameObject fruitHolder = new GameObject(creatureName + " fruitHolder");
        for (int i = 0; i < gameManagerScript.fruitPoolAmount; i++)
        {
            GameObject newFruit = Instantiate(creatureFruit);
            newFruit.transform.SetParent(fruitHolder.transform);
            gameManagerScript.availableFruit.Add(newFruit);
            newFruit.SetActive(false);
        }
    }*/

    public override void CreatureHungerStart() //Change hunger start
    {
        creatureHungerMeterCurrent = 40f;
    }

    public override void PhotosynthesisFillAmount() //Change how much is filled
    {
        photosynthesisHungerFill = 10f;
    }

    public override void IndividualCreatureBaseStatGrowth() //Individual stats
    {
        baseStr = 1;
        baseDex = 1;
        baseInt = 3;
        baseSta = 5;
        baseDef = 3;
        baseHp = 4;

        //Math will require the current stats, grades multiplyers, base stats, and level in calculation
        creatureSTR = (int)(baseStr * gradeStrMultiplyer * creatureLevel);
        creatureDEX = (int)(baseDex * gradeDexMultiplyer * creatureLevel);
        creatureINT = (int)(baseInt * gradeIntMultiplyer * creatureLevel);
        creatureSTA = (int)(baseSta * gradeStaMultiplyer * creatureLevel);
        creatureDEF = (int)(baseDef * gradeDefMultiplyer * creatureLevel);
        creatureHP = (int)(baseHp * gradeHpMultiplyer * creatureLevel) + creatureLevel + 10;
        creatureHpMax = creatureHP;
    }

    public override void SearchForFood() //Autotrophs don't need to search
    {
        creatureAnimator.SetBool("isEating", true);
    }
}
