using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;


public class Dash : NetworkBehaviour{
    private CoolDown driver;
    public Vector3 moveDirection;
 
    public const float maxDashTime = 1.0f;
    public float dashDistance = 10;
    public float dashStoppingSpeed = 0.1f;
    public SpecialItem dashItem;
    private bool isOnCoolDown = false;
    float currentDashTime = maxDashTime;
    float dashSpeed = 12;

    CharacterController characterController;
    PlayerMovement pMove;

    void Start(){
        //driver = GameObject.Find("Canvas").GetComponent<CoolDown>();
        characterController = this.gameObject.GetComponent<CharacterController>();
        pMove = GetComponent<PlayerMovement>();
    }

    //UPDATE CHECK FOR MOVEMENT ONLY WHEN DASHING
    void FixedUpdate(){
        if(pMove.pStats.HasDash) DashPlayer();
    }

    void DashPlayer(){
        //if (!IsLocalPlayer) { return; }
        if(characterController.enabled == true){
            if ((Input.GetKeyDown(KeyCode.R) || Input.GetAxis("Dash") != 0) && isOnCoolDown == false)
            {
                currentDashTime = 0;
                StartCoroutine(startCoolDown());
            }
            if(currentDashTime < maxDashTime)
            {
                moveDirection = transform.forward * dashDistance;
                currentDashTime += dashStoppingSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
            }
            characterController.Move(moveDirection * Time.deltaTime * dashSpeed);
        }
    }

    private IEnumerator startCoolDown(){
        Debug.Log("start corotine");
        isOnCoolDown = true;
        driver.startUICooldown(dashItem.name);
        yield return new WaitForSeconds(dashItem.cooldownM);
        isOnCoolDown = false;
        Debug.Log("end corotine");
    }

}