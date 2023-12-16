using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_stateMachine_creature_thinking : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        creature.creatureNavMeshAgent.isStopped = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        creature.creatureThinkTimer -= Time.deltaTime * creature.creatureINT;
        if (creature.creatureThinkTimer <= 0f && animator.GetBool("isThinking") == true)
        {
            creature.HungerUpdateCheck(); //Check if hungry

            if (creature.creatureIsHungry) //Hungry
            {
                creature.SearchForFood();
            }
            else //Random action
            {
                int possibleDecisions = 2; // exclusive, last number does not count
                int chooseDecision = Random.Range(0, possibleDecisions);

                switch (chooseDecision)
                {
                    case 0: //Return to idle
                        animator.SetBool("isIdle", true);
                        break;
                    case 1: //Walk in a random direction
                        animator.SetBool("isWalking", true);
                        creature.CreatureRandomMovementRandomization();
                        break;
                }
            }
            animator.SetBool("isThinking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        //Reset timer for next time
        creature.creatureThinkTimer = 10f;
    }
}
