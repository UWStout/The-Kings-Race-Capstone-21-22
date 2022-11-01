using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dAerialWallRunState : dAerialBaseState
{
    bool spaceHeld = true;

    public override void EnterState(dAerialStateManager aSM, dAerialBaseState previousState){

        aSM.pStats.GravVel = 0; // on entering reset grav vel
        spaceHeld = true; // prevent accidental wall jumping

        //if left
        if(aSM.CalculateSide() < 0)
        {
            aSM.GetComponent<Animator>().SetBool("isLEFTWallRunning", true);
        }
        //if right
        else if (aSM.CalculateSide() > 0)
        {
            aSM.GetComponent<Animator>().SetBool("isRIGHTWallRunning", true);
        }

    }

    public override void ExitState(dAerialStateManager aSM, dAerialBaseState nextState){
        aSM.GetComponent<Animator>().SetBool("isLEFTWallRunning", false);
        aSM.GetComponent<Animator>().SetBool("isRIGHTWallRunning", false);
    }

    public override void UpdateState(dAerialStateManager aSM){

        //if not wallrunning or are ragdolling then falling
        if(!aSM.isWallRunning || (aSM.mSM.currentState == aSM.mSM.RagdollState)){
            aSM.SwitchState(aSM.FallingState);
        }        

        //if space is pressed then jumping
        else if(Input.GetButton("Jump") && !spaceHeld && !aSM.pStats.IsPaused){
            aSM.SwitchState(aSM.JumpingState);
        }
        else if(!Input.GetButton("Jump") && spaceHeld){
            spaceHeld = false;
        }

        //if able to grapple then grapple
        else if(aSM.CheckGrapple()){
            aSM.SwitchState(aSM.GrappleAirState);
        }

    }

    public override void FixedUpdateState(dAerialStateManager aSM){

        //Modified gravity calculation for wallrun
        aSM.GravityCalculation(2);

    }
}
