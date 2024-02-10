using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SildeFinsh : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().isSlide = false;
        animator.GetComponent<CapsuleCollider2D>().size = new Vector2(animator.GetComponent<CapsuleCollider2D>().size.x, animator.GetComponent<PlayerController>().capsuleCollider2Dy);
        animator.GetComponent<CapsuleCollider2D>().offset = new Vector2(animator.GetComponent<CapsuleCollider2D>().offset.x, animator.GetComponent<PlayerController>().capsuleCollider2DOffsety);
        //Debug.Log("stateInfo:"+ stateInfo.length);
    }

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
