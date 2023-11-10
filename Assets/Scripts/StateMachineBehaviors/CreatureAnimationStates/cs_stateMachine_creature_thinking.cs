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
        if (creature.creatureThinkTimer <= 0f) animator.SetBool("isThinking", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        int possibleDecisions = 2; // 0 counts as a decision
        int chooseDecision = Random.Range(0, possibleDecisions);
        switch (chooseDecision)
        {
            case 0: //Do nothing
                break;
            case 1: //Walk in a random direction
                creature.CreatureRandomMovementRandomization();
                break;
        }
        //Reset timer for next time
        creature.creatureThinkTimer = 10f;
        
        
    }
}
