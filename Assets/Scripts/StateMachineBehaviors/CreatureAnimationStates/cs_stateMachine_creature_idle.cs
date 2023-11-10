using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_stateMachine_creature_idle : StateMachineBehaviour
{
    float idleTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        creature.creatureNavMeshAgent.isStopped = true;
        animator.SetBool("isIdle", true);
        idleTimer = Random.Range(3, 11);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isThinking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
