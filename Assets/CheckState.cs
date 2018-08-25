using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : StateMachineBehaviour {

    private bool hasMultipleIdleAnim;
    private float idleTimer;
    private float prevIdle;
    private float curIdle;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(animator.GetFloat("IdleInput") != 0)
        {
            animator.SetFloat("IdleInput", 0);
        }
        if (animator.GetBool("MultiIdle"))
        {
            hasMultipleIdleAnim = true;
        }
        idleTimer = 0;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if(hasMultipleIdleAnim)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer >= 5)
            {
                idleTimer = 0;
                FlickIdle(animator);
            }
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
    public void FlickIdle(Animator animator)
    {
        if (curIdle == 0)
        {
            curIdle += 1;
        }
    }
    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
