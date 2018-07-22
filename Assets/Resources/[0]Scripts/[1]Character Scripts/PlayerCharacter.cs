using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones.Characters;

[Serializable]
public class PlayerCharacter : BareboneCharacter {

    [SerializeField] private bool CamerabasedControls;
    private Dictionary<string, KeyCode> movementKeys;
    private Dictionary<string, KeyCode> combatKeys;

    public float ForwardInput;
    public float turnInput;

    private KeyCode doubleTapKeyCode;
    private int tapCounter;
    private float tapTimer;

    protected Rigidbody rig;
    private float distToGround;
    private Vector3 movement;
    
    public override void Awake()
    {
        base.Awake();
        isGrounded = true;
        //CamerabasedControls = true;
        ForwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        movementSpeed = 18;
        distToGround = 1.0f;
        rig = this.GetComponent<Rigidbody>();
        movementKeys = new Dictionary<string, KeyCode>();
        combatKeys = new Dictionary<string, KeyCode>();
        //Check if There's a Save File for its InputKeys
        GenerateGenericInputKeys();
    }
    public override void Update()
    {
        ForwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        base.Update();
        if (isGrounded)
        {
            Movement(Time.deltaTime);
        }
        Combat();
    }
    private bool IsGrounded
    {
        get
        {
            isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.51f);
            return this.isGrounded;
        }
    }
    private void CheckFacing(float deltaTime)
    {
       // targetRotation = Quaternion.AngleAxis(rotationVel * turnInput * Time.deltaTime, Vector3.up);
        //Debug.Log(targetRotation + "RotVel : " + rotationVel + " turnInput : " + turnInput + " deltaTime: " + deltaTime);
        transform.rotation *= targetRotation;
    }
    private void DoubleTap(float deltaTime, KeyCode input)
    {
        if (doubleTapKeyCode != input)
        {
            doubleTapKeyCode = input;
            tapCounter = 0;
        }
        //Debug.Log("Tapped");
        tapCounter += 1;
        tapTimer = 0.2f;
    }
    /// <summary>
    /// Checks if player is not moving At ALL
    /// </summary>
    /// <returns>Return if player is confirmed in Idle State</returns>
    public bool CheckIfMoving()
    {
        bool isMoving = false;
       foreach(KeyCode btns in movementKeys.Values)
       {
            if (Input.GetKey(btns))
            {
                isMoving = true;
                return isMoving;
            }
       }

        return isMoving;
    }
    public override void Combat()
    {
        base.Combat();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (LivingState != LivingState.DEAD && LivingState != LivingState.COMBAT)
            {
                LivingState = LivingState.COMBAT;
            }
            else
            {
                LivingState = LivingState.IDLE;
            }
        }
    }
    public void GenerateGenericInputKeys()
    {
        movementKeys.Add("Forward", KeyCode.W);
        movementKeys.Add("Backward", KeyCode.S);
        movementKeys.Add("Left", KeyCode.A);
        movementKeys.Add("Right", KeyCode.D);
        movementKeys.Add("Strafe", KeyCode.LeftShift);
        movementKeys.Add("Jump", KeyCode.Space);

        combatKeys.Add("Sheathe", KeyCode.E);
    }

    public void CheckSkillInput()
    {

    }

}
