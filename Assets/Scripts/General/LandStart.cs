using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.gameObject.GetComponent<PlayerController>().effectManager.LandDust(animator.gameObject.transform.position);
        //animator.gameObject.GetComponent<PlayerController>().rippleEffect.GetComponent<RippleEffect>().Ripple(animator.gameObject.transform.position, animator.gameObject.transform.localScale,new Vector2(0,0));
        animator.gameObject.GetComponent<ShapeTransform>().transformTimer = animator.gameObject.GetComponent<ShapeTransform>().transformDuration;


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
