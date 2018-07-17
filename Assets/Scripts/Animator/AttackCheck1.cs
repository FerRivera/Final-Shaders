﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck1 : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var hero = animator.gameObject.GetComponent<HeroModel>();
        hero.damage = hero.damageSecondHit;
        hero.swordCollider.enabled = false;
        if (hero.hitsAmount.Count > 0)
        {
            hero.hitsAmount.Pop();
            animator.SetTrigger("Attack2");
            
            //animator.gameObject.GetComponent<HeroView>()._animator.SetTrigger("Attack2");
            //animator.Play("Attack2");
        }
        else
        {
            animator.gameObject.GetComponent<HeroView>().attacking = false;
            hero.attacking = false;
            hero.swordTrail.SetActive(false);
        }

        hero.actualTimeToTriggerCombo = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
