using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_creature_testAutotroph : cs_creatureData
{
    private void Start()
    {
        IndividualCreatureBaseStatGrowth();
        creatureHpCurrent = creatureHpMax;
        ApplyDexToNavSpeed();
    }

    public override void CreatureMoveRangeDistribution()
    {
        creatureMoveRange = 3f;
    }

    public override void CreatureNameDistribution()
    {
        creatureName = "Test Autotroph Plant";
        creatureType = "Autotroph";
    }

    public override void CreatureHungerStart()
    {
        creatureHungerMeterCurrent = 40f;
    }

    public override void PhotosynthesisFillAmount()
    {
        photosynthesisHungerFill = 10f;
    }

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

    public override void ApplyDexToNavSpeed()
    {
        creatureNavMeshAgent.speed = creatureDEX;
    }

    public override void SearchForFood() //Autotrophs don't need to search
    {
        creatureAnimator.SetBool("isEating", true);
    }

}
