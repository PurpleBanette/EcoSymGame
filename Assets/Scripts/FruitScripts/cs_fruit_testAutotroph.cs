using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_fruit_testAutotroph : cs_fruitData
{
    public override void FruitTimer()
    {
        fruitTimeLimit = 60f;
        fruitDecayTimer = fruitTimeLimit;
    }
    public override void AssignFruitIDValues()
    {
        fruitName = "Test Fruit";
        fruitID = CreatureFruitID.f_testAutotroph;
    }
    public override void FruitStats()
    {
        fruitSaturation = 10;
    }
}
