using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_creature_testHeterotroph : cs_creatureData
{
    private void Start()
    {
        //Test();
        IndividualCreatureBaseStatGrowth();
        creatureHpCurrent = creatureHpMax;
    }

    /*public override void Test() //Example of inheriting and overriding functions
    {
        base.Test(); //Runs original code first
        Debug.Log("testing " + this.gameObject.name + " override"); //Adds new code to function
    }*/

    public override void IndividualCreatureBaseStatGrowth()
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
}
