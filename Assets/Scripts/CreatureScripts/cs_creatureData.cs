using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //To edit in scene
using UnityEngine.InputSystem; //To edit the input system
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;
using UnityEngine.AI;

public class cs_creatureData : MonoBehaviour
{
    [Header("Unity-specific data")]
    [Tooltip("The creature's rigidbody")]
    [SerializeField] protected Rigidbody creatureRigidBody;
    [Tooltip("The creature's physics collider")]
    [SerializeField] protected Collider creatureCollider;
    [Tooltip("The creature's gameobject for referencing")]
    [SerializeField] protected GameObject creatureObj;
    [Tooltip("The creature's material")]
    [SerializeField] protected Material creatureMaterial;
    [Tooltip("The creature's animator")]
    [SerializeField] protected Animator creatureAnimator;
    [Tooltip("The game manager script")]
    public cs_gameManager gameManagerScript;
    [Tooltip("The creature's script")]
    [SerializeField] protected cs_creatureData creatureScript;
    [Tooltip("AUTOTROPHS ONLY: The creature's fruit")]
    public GameObject creatureFruit;

    [Header("Creature Stats")]
    [Tooltip("The creature's name")]
    [SerializeField] protected string creatureName;
    [Tooltip("The creature's type")]
    [SerializeField] protected CreatureType creatureType;
    [Tooltip("The creature's health")]
    public int creatureHpCurrent;
    [Tooltip("The creature's maximum health")]
    [SerializeField] protected int creatureHpMax;
    [Tooltip("The creature's base stats that are modified by the grades. STR = Strength and damage, DEX = speed, INT = thinking speed and good decisions, STA = health and energy gain/loss, DEF = damage resistance and endurance, HP = Base health")]
    public int creatureSTR, creatureDEX, creatureINT, creatureSTA, creatureDEF, creatureHP;
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
    [Tooltip("The current hunger of the creature")]
    public float creatureHungerMeterCurrent;
    [Tooltip("The max hunger of the creature")]
    [SerializeField] float creatureHungerMeterMax;
    [Tooltip("AUTOTROPHS ONLY: How much food they receive from photosynthesis")]
    public float photosynthesisHungerFill;

    [Header("AI")]
    [Tooltip("The transform that the creature uses to pathfind")]
    public Vector3 creatureTarget;
    [Tooltip("Bool that checks if the creature is moving randomly")]
    public bool creatureMovementCheck;
    [Tooltip("The range of how far the creature can walk")]
    [SerializeField] protected float creatureMoveRange;
    [Tooltip("A check that resets the creature if they're stuck walking")]
    [SerializeField] float walkStuckCheck = 30f;
    [Tooltip("Layermasks for the navmesh")]
    public LayerMask groundLayer;
    [Tooltip("The creature's navmesh agent")]
    public NavMeshAgent creatureNavMeshAgent;
    [Tooltip("The timer how long it takes for the creature to think of something to do")]
    public float creatureThinkTimer;
    [Tooltip("Bool that checks if the creature is hungry")]
    public bool creatureIsHungry;
    [Tooltip("Checks if the autotroph is requesting a fruit")]
    public bool requestingFruit = false;

    private void Awake()
    {
        creatureScript = GetComponent<cs_creatureData>();
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<cs_gameManager>();

        //Grab all this from the creature
        creatureRigidBody = GetComponent<Rigidbody>();
        creatureCollider = GetComponent<Collider>();
        creatureObj = gameObject;

        //Set this to zero as a default for the pathfinding
        creatureTarget = Vector3.zero;

        //Apply the creature tag on spawn
        gameObject.tag = "Creature";

        creatureMaterial = GetComponentInChildren<Renderer>().material;
        creatureNavMeshAgent = GetComponent<NavMeshAgent>();
        creatureAnimator = GetComponent<Animator>();
        
        //Start setting creature stats
        CreatureStatsDistribution();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        CreatureMovement(); //Random movement
        CreatureHungerSystem(); //Creature's hunger system
    }

    private void FixedUpdate()
    {
        CreatureDeath();
    }

    public void CreatureStatsDistribution() //Starting functions
    {
        CreatureMoveRangeDistribution();
        CreatureNameDistribution();
        CreatureStatGradeRandomization();
        CreatureSizeRandomization();
        CreatureColorRandomization();
        CreatureLevelUponSpawn();
        CreatureMutationGrowth();
        creatureThinkTimer = 10f;
        creatureAnimator.SetFloat("animSpeedMultiplier", gradeDexMultiplyer); //Multiply the animation speed by their dex multiplyer
        CreatureHungerStart();
        PhotosynthesisFillAmount();
        creatureHungerMeterMax = 100f; //Max hunger on the meter
    }

    public virtual void CreatureMoveRangeDistribution() //May change with calculations later
    {
        /*This is the range of how far the creature can move from its center position. This number will
         vary based on creature*/
        creatureMoveRange = 10f;
    }

    public virtual void CreatureHungerStart()
    {
        /*This is the creature's current hunger that will decrease slowly over time. This number will
         vary based on creature*/
        creatureHungerMeterCurrent = 70f; //Current hunger
    }

    public virtual void PhotosynthesisFillAmount()
    {
        /*This is specifically for autotrophs. It fills their hunger meter after they photosynthesize. Default
         is 0 will will vary based on individual autotrophs*/
        photosynthesisHungerFill = 0f;
    }

    public virtual void CreatureNameDistribution()
    {
        /*A null check is needed to prevent errors. Each creature will be assigned their name and type*/
        creatureName = null;
        creatureType = CreatureType.typeNull;
    }

    public void CreatureStatGradeRandomization() 
    {
        /*Randomizes the stat grades that affect the stat growth of the creature. The stat grade is from a range
         of 0(E) through 5(S). Each grade adds a multiplyer to the stats they gain during each level-up.*/
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

    public void CreatureSizeRandomization() 
    {
        /*Randomizes the scale of the creature from a range of Tiny to Massive.*/
        creatureSize = Random.Range(0, 6);
        switch (creatureSize)
        {
            case 0:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.71f, 0.80f); //Tiny
                break;
            case 1:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.81f, 0.90f); //Small
                break;
            case 2:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(0.91f, 1.10f); //Medium
                break;
            case 3:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.11f, 1.20f); //Large
                break;
            case 4:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.21f, 1.30f); //Huge
                break;
            case 5:
                creatureObj.transform.localScale = creatureObj.transform.localScale * Random.Range(1.31f, 1.40f); //Massive
                break;
        }
    }

    public void CreatureColorRandomization() 
    {
        /*Randomizes slight color variations of the creature. The RGB channels of their material color are converted
         to HSV, then randomized to an extent, then converted back to RGB to apply the colors to the creature*/
        Color randomizedColor = creatureMaterial.color;
        Color.RGBToHSV(randomizedColor, out creatureColorHue, out creatureColorSaturation, out creatureColorValue);
        creatureColorHue = 0 + Random.Range(0, 361);
        creatureColorSaturation = 0 + Random.Range(0, 51);
        creatureColorValue = 100 - Random.Range(0, 26);
        creatureMaterial.color = Color.HSVToRGB(creatureColorHue / 360, creatureColorSaturation / 100, creatureColorValue / 100);
    }

    public void CreatureLevelUponSpawn()
    {
        /*The creature starts out at level 1 with 0 exp*/
        creatureLevel = 1;
        creatureCurrentExp = 0;
        creatureGoalExp = 100;
    }

    public virtual void IndividualCreatureBaseStatGrowth()
    {
        /*TEMPLATE for stat growth upon level up. Creatures override the base stats depending on their species.
         Upon level up, their stats slightly increase based on a calculation that checks their stat grades*/

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
        /*The chance of mutation for the creature upon being born. This increases slowly over time and will affect
         the chances of their offspring being mutated*/
        creatureMutationChance = (Random.Range(0f, 0.1f) / 100f) * 100f; //Calculating percentage from 0% to 100%
    }

    public void ApplyDexToNavSpeed()
    {
        /*The creature's movement speed will be affected by their Dexterity*/
        creatureNavMeshAgent.speed = creatureDEX; //Temporary, until a better calculation is made
    }

    public void CreatureRandomMovementRandomization()
    {
        /*This will affect the creature's navigation, affecting what they can and cannot walk on, and what they prefer to walk on.
         Each terrain piece has its own navmesh layer, and the creature will take those into account when selecting a random
        raycast spot within range to walk towards. If they cannot walk anywhere, they will attempt to try again or do something
        else*/
        int nav_notWalkable = 0 << NavMesh.GetAreaFromName("Not Walkable");
        int nav_walkable = 1 << NavMesh.GetAreaFromName("Walkable"); //Sets walkable navmesh to 1
        int nav_grass = 2 << NavMesh.GetAreaFromName("Grass");
        int nav_dirt = 3 << NavMesh.GetAreaFromName("Dirt");
        int nav_water = 4 << NavMesh.GetAreaFromName("Water");
        int nav_rocky = 5 << NavMesh.GetAreaFromName("Rocky");
        int nav_snow = 6 << NavMesh.GetAreaFromName("Snow");
        int nav_ice = 7 << NavMesh.GetAreaFromName("Ice");
        int nav_lava = 8 << NavMesh.GetAreaFromName("Lava");

        float destinationX = Random.Range(-creatureMoveRange, creatureMoveRange); //Random X
        float destinationZ = Random.Range(-creatureMoveRange, creatureMoveRange); //Random Z
        RaycastHit destinationYTarget; //The raycasted point after a random position is made
        NavMeshHit finalNavmeshDestination; //The final point that then agent walks to

        //Raycast that finds the Y coordinate of the random point + your position
        Physics.Raycast(new Vector3(transform.position.x + destinationX, transform.position.y + 10f, transform.position.z + destinationZ), Vector3.down, out destinationYTarget, Mathf.Infinity, groundLayer);

        //Checks if the random point is on the walkable navmesh within a 2ft range
        bool possibleLocation = NavMesh.SamplePosition(destinationYTarget.point, out finalNavmeshDestination, 2f, NavMesh.AllAreas);

        //Success
        if (possibleLocation && finalNavmeshDestination.mask != nav_notWalkable)
        {
            //Debug.Log(finalNavmeshDestination.mask);
            creatureTarget = finalNavmeshDestination.position; //Walk here
            creatureMovementCheck = true; //Creature is now moving
            creatureAnimator.SetBool("isWalking", true);
        }
        //Failure
        else
        {
            Debug.Log("cannot walk on " + finalNavmeshDestination.mask);
            creatureAnimator.SetBool("isThinking", true);
        }

    }

    public void CreatureMovement()
    {
        /*The creature will begin walking towards their selected destination. Their stuck check will decide whether they are
         stuck or not, by timing how long it takes for them to move. If they take too long to get somewhere, they are considered stuck
        or lost an will reset.*/
        if (creatureMovementCheck) //If bool is active
        {
            walkStuckCheck -= Time.deltaTime;
            creatureNavMeshAgent.SetDestination(creatureTarget); //If bool is active, start moving
            Debug.DrawRay(creatureTarget + new Vector3(0, 0.5f, 0), Vector3.down, Color.green);
        }
        if (Vector3.Distance(transform.position, creatureTarget) < 0.3 && creatureMovementCheck) //If destination reached within a range, stop
        {
            walkStuckCheck = 30f;
            creatureNavMeshAgent.SetDestination(transform.position);
            creatureAnimator.SetBool("isThinking", true);
            creatureAnimator.SetBool("isWalking", false);

            creatureMovementCheck = false;
        }
        if (walkStuckCheck <= 0f) //If the creature is stuck after 30 seconds, think again
        {
            walkStuckCheck = 30f;
            creatureNavMeshAgent.SetDestination(transform.position);
            creatureAnimator.SetBool("isThinking", true);
            creatureAnimator.SetBool("isWalking", false);

            creatureMovementCheck = false;
        }

    }
    void CreatureHungerSystem()
    {
        /*The creature's hunger meter will slowly decrease over time.*/
        creatureHungerMeterCurrent = (creatureHungerMeterCurrent / creatureHungerMeterMax) * 100f; //Percentage out of 100%

        creatureHungerMeterCurrent -= Time.deltaTime / 20; //How long it takes for hunger to drop

    }

    public void HungerUpdateCheck() //Do not activate on update to save memory
    {
        /*The creature will check if their hunger reaches a certain threshold. This function
         is called in the creature's "Thinking" animation state, and checks their hunger
        before making a decision*/
        if (creatureHungerMeterCurrent <= 40f)
        {
            creatureIsHungry = true;
        }
        else
        {
            creatureIsHungry = false;
        }
    }

    public virtual void SearchForFood()
    {
        /*This is called in the creature's "Thinking" animation state, and will cause
         the creature to search for food if their hunger is low enough*/
        
    }

    public virtual void AutotrophPhotosynthesis()
    {
        /*Unique to autotrophs. They will generate a fruit from their object pool, which will increase
         their hunger meter, and drop a fruit for herbivores*/
        GameObject newFruit = gameManagerScript.RequestFruit(gameManagerScript.availableFruit);
        if (newFruit != null)
        {
            gameManagerScript.CreateFruit(newFruit, transform.position);
            newFruit.GetComponent<cs_fruitData>().fruitSpawned = true;
        }
        //Switch statement matching creatures + fruits so that each fruit has its own object pool;

        requestingFruit = true;
        //gameManagerScript.RequestCreatureFruit();
        creatureHungerMeterCurrent += photosynthesisHungerFill;
        //Pool fruit
    }

    //Temporary
    private void CreatureDeath()
    {
        /*Kills the creature if their HP is 0*/
        if (creatureHpCurrent <= 0f)
        {
            Destroy(gameObject);
        }
    }
}