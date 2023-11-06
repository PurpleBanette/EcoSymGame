using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //To edit in scene
using UnityEngine.InputSystem; //To edit the input system
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;

public class cs_creatureData : MonoBehaviour
{
    [Header("Unity-specific data")]
    [Tooltip("The creature's rigidbody")]
    [SerializeField] protected Rigidbody creatureRigidBody;
    [Tooltip("The creature's physics collider")]
    [SerializeField] protected Collider creatureCollider;
    [Tooltip("The creature's gameobject for referencing")]
    [SerializeField] protected GameObject creatureObj;
    [Tooltip("The creature's mesh")]
    [SerializeField] protected MeshFilter creatureMeshFilter;
    [Tooltip("The creature's mesh renderer")]
    [SerializeField] protected MeshRenderer creatureMeshRenderer;
    [Tooltip("The creature's materisl")]
    [SerializeField] protected Material creatureMaterial;
    //[Tooltip("Reference to the game manager")]
    //public cs_gameManager gameManagerScriptReference;*/

    [Header("Creature Stats")]
    [Tooltip("The creature's name")]
    [SerializeField] protected string creatureName;
    [Tooltip("The creature's health")]
    public int creatureHpCurrent;
    [Tooltip("The creature's maximum health")]
    [SerializeField] protected int creatureHpMax;
    [Tooltip("The creature's base stats that are modified by the grades. STR = Strength and damage, DEX = speed, INT = thinking speed and good decisions, STA = health and energy gain/loss, DEF = damage resistance and endurance, HP = Base health")]
    [SerializeField] protected int creatureSTR, creatureDEX, creatureINT, creatureSTA, creatureDEF, creatureHP;
    [Tooltip("The stat grades and their modifications")]
    [SerializeField] protected int creatureStatGradeSTR, creatureStatGradeDEX, creatureStatGradeINT, creatureStatGradeSTA, creatureStatGradeDEF, creatureStatGradeHP;
    [Tooltip("The base stats of the individual creature species")]
    [SerializeField] protected int baseStr, baseDex, baseInt, baseSta, baseDef, baseHp;
    [Tooltip("The stat grade modifier multiplyer")]
    [SerializeField] protected float gradeStrMultiplyer, gradeDexMultiplyer, gradeIntMultiplyer, gradeStaMultiplyer, gradeDefMultiplyer, gradeHpMultiplyer;
    [Tooltip("The size stat of the creature")]
    [SerializeField] protected int creatureSize;
    [Tooltip("The creature's current level (Lv.100 = Max)")]
    [SerializeField] protected int creatureLevel;
    [Tooltip("The creature's current exp amount")]
    [SerializeField] protected int creatureCurrentExp;
    [Tooltip("The creature's exp goal to level up")]
    [SerializeField] protected int creatureGoalExp;
    [Tooltip("The creature's color data")]
    [SerializeField] protected float creatureColorHue, creatureColorSaturation, creatureColorValue;
    [Tooltip("The chance of mutation of the creature's offspring")]
    [SerializeField] protected float creatureMutationChance;

    [Header("AI")]
    [Tooltip("The transform that the creature uses to pathfind")]
    public Transform creatureTarget;

    private void Awake()
    {
        creatureRigidBody = GetComponent<Rigidbody>();
        creatureCollider = GetComponent<Collider>();
        creatureObj = gameObject;
        creatureMeshFilter = GetComponent<MeshFilter>();
        creatureMeshRenderer = GetComponent<MeshRenderer>();
        creatureTarget = null;
        gameObject.tag = "Creature";
        creatureMaterial = GetComponent<Renderer>().material;

        CreatureStatsDistribution();
    }

    private void Start()
    {
        
    }

    /*public virtual void Test() 
    {
        Debug.Log("testing " + this.gameObject.name); 
    }*/

    public void CreatureStatsDistribution() //Starting functions
    {
        CreatureStatGradeRandomization();
        CreatureSizeRandomization();
        CreatureColorRandomization();
        CreatureLevelUponSpawn();
        CreatureMutationGrowth();
    }

    public void CreatureStatGradeRandomization() //Randomizes the stat grades that affect the stat growth of the creature
    {
        creatureStatGradeSTR = Random.Range(0, 6);
        creatureStatGradeDEX = Random.Range(0, 6);
        creatureStatGradeINT = Random.Range(0, 6);
        creatureStatGradeSTA = Random.Range(0, 6);
        creatureStatGradeDEF = Random.Range(0, 6);
        creatureStatGradeHP = Random.Range(0, 6);

        switch (creatureStatGradeSTR)
        {
            case 0:
                gradeStrMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeStrMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeStrMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeStrMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeStrMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeStrMultiplyer = 1.5f; //S Rank
                break;
        }

        switch (creatureStatGradeDEX)
        {
            case 0:
                gradeDexMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeDexMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeDexMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeDexMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeDexMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeDexMultiplyer = 1.5f; //S Rank
                break;
        }

        switch (creatureStatGradeINT)
        {
            case 0:
                gradeIntMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeIntMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeIntMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeIntMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeIntMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeIntMultiplyer = 1.5f; //S Rank
                break;
        }

        switch (creatureStatGradeSTA)
        {
            case 0:
                gradeStaMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeStaMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeStaMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeStaMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeStaMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeStaMultiplyer = 1.5f; //S Rank
                break;
        }

        switch (creatureStatGradeDEF)
        {
            case 0:
                gradeDefMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeDefMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeDefMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeDefMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeDefMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeDefMultiplyer = 1.5f; //S Rank
                break;
        }

        switch (creatureStatGradeHP)
        {
            case 0:
                gradeHpMultiplyer = 1f; //E Rank
                break;
            case 1:
                gradeHpMultiplyer = 1.1f; //D Rank
                break;
            case 2:
                gradeHpMultiplyer = 1.2f; //C Rank
                break;
            case 3:
                gradeHpMultiplyer = 1.3f; //B Rank
                break;
            case 4:
                gradeHpMultiplyer = 1.4f; //A Rank
                break;
            case 5:
                gradeHpMultiplyer = 1.5f; //S Rank
                break;
        }
    }

    public void CreatureSizeRandomization() //Randomizes the size of the creature
    {
        creatureSize = Random.Range(0, 6);
        switch (creatureSize)
        {
            case 0:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.40f, 0.60f); //Tiny
                break;
            case 1:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.61f, 0.80f); //Small
                break;
            case 2:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.81f, 1.20f); //Medium
                break;
            case 3:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.21f, 1.40f); //Large
                break;
            case 4:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.41f, 1.60f); //Huge
                break;
            case 5:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.61f, 2.00f); //Gargantuan
                break;
        }
    }

    public void CreatureColorRandomization() //Randomizes slight color variations of the creature
    {
        Color randomizedColor = creatureMaterial.color;
        Color.RGBToHSV(randomizedColor, out creatureColorHue, out creatureColorSaturation, out creatureColorValue);
        creatureColorHue = 0 + Random.Range(0, 361);
        creatureColorSaturation = 0 + Random.Range(0, 51);
        creatureColorValue = 100 - Random.Range(0, 26);
        creatureMaterial.color = Color.HSVToRGB(creatureColorHue / 360, creatureColorSaturation / 100, creatureColorValue / 100);
    }

    public void CreatureLevelUponSpawn()
    {
        creatureLevel = 1;
        creatureCurrentExp = 0;
        creatureGoalExp = 100;
    }

    public virtual void IndividualCreatureBaseStatGrowth() //TEMPLATE for stat growth upon level up. Creatures override the base stats depending on their species
    {
        //This goes in the creature's Start function
        //creatureHpCurrent = creatureHpMax;

        baseStr = 1;
        baseDex = 1;
        baseInt = 1;
        baseSta = 1;
        baseDef = 1;
        baseHp = 10;

        //Math will require the current stats, grades multiplyers, base stats, and level in calculation
        creatureSTR = (int)(baseStr * gradeStrMultiplyer * creatureLevel);
        creatureDEX = (int)(baseDex * gradeDexMultiplyer * creatureLevel);
        creatureINT = (int)(baseInt * gradeIntMultiplyer * creatureLevel);
        creatureSTA = (int)(baseSta * gradeStaMultiplyer * creatureLevel);
        creatureDEF = (int)(baseDef * gradeDefMultiplyer * creatureLevel);
        creatureHP = (int)(baseHp * gradeHpMultiplyer * creatureLevel) + creatureLevel + 10;
        creatureHpMax = creatureHP;
    }

    public void CreatureMutationGrowth()
    {
        creatureMutationChance = (Random.Range(0f, 0.1f) / 100f) * 100f; //Calculating percentage from 0% to 100%
        Debug.Log(creatureMutationChance);
    }
}