using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cs_stateMachine_autotroph_photosynthesis : StateMachineBehaviour
{
    float eatingTimer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        creature.creatureNavMeshAgent.isStopped = true;
        eatingTimer = 20f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cs_creatureData creature = animator.GetComponent<cs_creatureData>();
        eatingTimer -= Time.deltaTime * creature.creatureSTA;
        if (eatingTimer <= 0f && animator.GetBool("isEating"))
        {
            //This affects ALL creatures, do not
            //creature.creatureHungerMeterCurrent = creature.creatureHungerMeterCurrent + creature.photosynthesisHungerFill;
            creature.AutotrophPhotosynthesis();
            animator.SetBool("isThinking", true);
            animator.SetBool("isEating", false);
        }
        //Future code if the plant gets interupted
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset
        eatingTimer = 20f;
    }
}
