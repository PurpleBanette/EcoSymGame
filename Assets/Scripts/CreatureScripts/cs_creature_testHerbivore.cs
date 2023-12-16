using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_creature_testHerbivore : cs_creatureData
{
    private void Start()
    {
        IndividualCreatureBaseStatGrowth();
        creatureHpCurrent = creatureHpMax;
    }

    public override void AssignCreatureIDValues()
    {
        creatureName = "Test Herbivore Cube";
        creatureType = CreatureType.t_herbivore;
        creatureID = CreatureID.c_testHerbivore;
        creatureFruitID = CreatureFruitID.f_null;
    }

    public override void IndividualCreatureBaseStatGrowth()
    {
        baseStr = 3;
        baseDex = 2;
        baseInt = 4;
        baseSta = 8;
        baseDef = 6;
        baseHp = 15;

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
