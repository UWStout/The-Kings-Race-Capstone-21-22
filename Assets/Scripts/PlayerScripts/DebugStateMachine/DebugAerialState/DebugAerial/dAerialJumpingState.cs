using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dAerialJumpingState : dAerialBaseState
{

    private int previousJumpNum;
    public override void EnterState(dAerialStateManager aSM, dAerialBaseState previousState){
        previousJumpNum = aSM.curJumpNum;
        if (aSM.curJumpNum % 2 == 1)
        {
            aSM.GetComponent<Animator>().SetBool("isJumpingLeft", true);
        }
        //if even, do right jump
        else if (aSM.curJumpNum % 2 == 0)
        {
            aSM.GetComponent<Animator>().SetBool("isJumpingRight", true);
        }
    }

    public override void ExitState(dAerialStateManager aSM, dAerialBaseState nextState){
 
       aSM.GetComponent<Animator>().SetBool("isJumpingLeft", false);
       aSM.GetComponent<Animator>().SetBool("isJumpingRight", false);
    }

    public override void UpdateState(dAerialStateManager aSM){

        //if grav vel < 0 falling
        if(aSM.pStats.GravVel < 0 || aSM.mSM.currentState == aSM.mSM.RagdollState){
            aSM.SwitchState(aSM.FallingState);
        }

        //if is grounded then grounded
        else if(aSM.isGrounded){
            aSM.SwitchState(aSM.GroundedState);
        }
        
        //if is wall running and in a state that allows it wallrun
        else if(aSM.isWallRunning && (aSM.isWallRunning && (aSM.mSM.currentState != aSM.mSM.SlideState && aSM.mSM.currentState != aSM.mSM.CrouchState && aSM.mSM.currentState != aSM.mSM.CrouchWalkState && aSM.mSM.currentState != aSM.mSM.RagdollState && aSM.mSM.currentState != aSM.mSM.RecoveringState))){
            aSM.SwitchState(aSM.WallRunState);
        }

        //if can grapple and in a state that allows it grapple
        else if(aSM.CheckGrapple() && (aSM.CheckGrapple() && (aSM.mSM.currentState != aSM.mSM.SlideState && aSM.mSM.currentState != aSM.mSM.CrouchState && aSM.mSM.currentState != aSM.mSM.CrouchWalkState && aSM.mSM.currentState != aSM.mSM.RagdollState && aSM.mSM.currentState != aSM.mSM.RecoveringState))){
            aSM.SwitchState(aSM.GrappleAirState);
        }
    }

    public override void FixedUpdateState(dAerialStateManager aSM){

        //default gravity calculations
        aSM.GravityCalculation(aSM.pStats.PlayerGrav);

        //if grapple release then apply grapple release force
        if(aSM.pStats.HasGrapple){
            aSM.GrappleReleaseForce();
        }  
        
        if(aSM.curJumpNum != previousJumpNum)
        {
            previousJumpNum = aSM.curJumpNum;
            aSM.GetComponent<Animator>().SetBool("isJumpingLeft", false);
            aSM.GetComponent<Animator>().SetBool("isJumpingRight", false);

            if (aSM.curJumpNum % 2 == 1)
            {
                aSM.GetComponent<Animator>().SetBool("isJumpingLeft", true);
            }
            //if even, do right jump
            else if (aSM.curJumpNum % 2 == 0)
            {
                aSM.GetComponent<Animator>().SetBool("isJumpingRight", true);
            }
        }
    }
}
