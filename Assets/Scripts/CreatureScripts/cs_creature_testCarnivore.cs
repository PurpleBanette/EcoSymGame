using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_creature_testCarnivore : cs_creatureData
{
    private void Start()
    {
        IndividualCreatureBaseStatGrowth();
        creatureHpCurrent = creatureHpMax;
    }

    public override void CreatureNameDistribution()
    {
        creatureName = "Test Carnivore Cube";
        creatureType = "Carnivore";
    }

    public override void IndividualCreatureBaseStatGrowth()
    {
        baseStr = 3;
        baseDex = 3;
        baseInt = 2;
        baseSta = 5;
        baseDef = 3;
        baseHp = 12;

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
