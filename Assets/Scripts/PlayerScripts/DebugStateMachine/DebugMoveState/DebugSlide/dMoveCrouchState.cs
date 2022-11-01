using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dMoveCrouchState : dMoveBaseState
{

    //Slide Variables
    RaycastHit slideRay; // slide raycast

    public override void EnterState(dMoveStateManager mSM, dMoveBaseState previousState){

        //if not coming from slide state then rotate player and adjust height
        if(previousState != mSM.SlideState && previousState != mSM.CrouchWalkState){
            mSM.pStats.CurVel = 0;
            mSM.moveController.height *= .5f;
            mSM.moveController.center = new Vector3(0,mSM.moveController.center.y - mSM.moveController.height * .5f,0);
            //mSM.moveController.Move(new Vector3(0,-mSM.moveController.height * .5f,0));
        }
        mSM.animator.SetBool("isCrouching", true);
    }
    
    public override void ExitState(dMoveStateManager mSM, dMoveBaseState nextState){
        
        //if next state isn't crouch walk revert rotation, speed, and height
        if(nextState != mSM.CrouchWalkState){
            mSM.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            mSM.pStats.CurVel = mSM.calculatedCurVel;
            mSM.moveController.height *= 2.0f;
            mSM.moveController.center = new Vector3(0,mSM.moveController.center.y + mSM.moveController.height * .25f,0);
        }
        mSM.animator.SetBool("isCrouching", false);

    }

    public override void UpdateState(dMoveStateManager mSM){

        //If player isn't pressing either Q or the joystick button they stop crouching if nothing is above them
        if((!Input.GetKey(KeyCode.JoystickButton1) && !Input.GetKey(KeyCode.Q))){

            if ((Physics.Raycast(mSM.gameObject.transform.position + new Vector3(0,1f,0), Vector3.up, out slideRay, 2f, mSM.layerMask) == false)){

                mSM.SwitchState(mSM.IdleState);
            }
            else{

                Debug.Log(slideRay.collider.name);
            }
        }
        else if((Input.GetAxis("Vertical") > 0.0f || Input.GetAxis("Horizontal") > 0.0f)){
            mSM.SwitchState(mSM.CrouchWalkState);
        }

        //Debug.DrawRay(mSM.gameObject.transform.position + new Vector3(0,1f,0), Vector3.up * 2f, Color.red);

        
        //If falling stop sliding and go to wasd states
        if(mSM.aSM.currentState == mSM.aSM.FallingState){
            mSM.SwitchState(mSM.IdleState);
        }
    
    }

    public override void FixedUpdateState(dMoveStateManager mSM){

        mSM.CrouchMovement();
    }
}
