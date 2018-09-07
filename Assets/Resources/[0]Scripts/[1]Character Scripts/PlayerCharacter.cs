using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones.Characters;
using Barebones.Skill;

[Serializable]
public class PlayerCharacter : BareboneCharacter {

    [Header("PLAYER INFORMATION")]
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
        CheckInput();
    }
    private bool IsGrounded
    {
        get
        {
            isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.51f);
            return this.isGrounded;
        }
    }

    private void CheckInput()
    {
        if (!skillActivated)
        {
            foreach (BaseSkill skill in skills)
            {
                if (!skill.inCooldown)
                {
                    skill.CheckKey(GetKeyClicked());
                }
                else skill.ReduceCooldown(Time.deltaTime);
            }
        }
        else
        {
            // NOTE : HAVE TO REMOVE THIS ONCE ANIMATION FOR EVERY SKILL IS IMPLEMENTED
            foreach (BaseSkill skill in skills)
            {
                if (skill.skillEnabled)
                {
                    skill.ReduceSkillDuration(Time.deltaTime);
                }
            }
        }
    }
    private void CheckFacing(float deltaTime)
    {
       // targetRotation = Quaternion.AngleAxis(rotationVel * turnInput * Time.deltaTime, Vector3.up);
        //Debug.Log(targetRotation + "RotVel : " + rotationVel + " turnInput : " + turnInput + " deltaTime: " + deltaTime);
        transform.rotation *= targetRotation;
    }

    private void ResetAllSkills(BaseSkill exception)
    {
        foreach(BaseSkill skill in skills)
        {
            Debug.Log("Resetting : " + skill.skillName);
            if(skill != exception)
            {
                if(!skill.simultaneousCast)
                {
                    skill.ResetSkill();
                }
            }
        }
    }
    /// <summary>
    /// Checks if player is not moving At ALL
    /// </summary>
    /// <returns>Return if player is confirmed in Idle State</returns>
    public string GetKeyClicked()
    {
        string noValue= "";
        foreach(string name in movementKeys.Keys)
        {
            if(Input.GetKeyDown(movementKeys[name]))
            {
                //Debug.Log("Key : " + name + " is clicked!");
                return name;
            }
        }

        return noValue;
    }
    public override void Combat()
    {
        base.Combat();
        if (LivingState != LivingState.DEAD && LivingState != LivingState.COMBAT)
        {
            LivingState = LivingState.COMBAT;
            Parameters p = new Parameters();
            p.AddParameter<bool>("Combat", true);
            UpdateAnimator("Combat", p);
        }
        else
        {
            LivingState = LivingState.IDLE;
            Parameters p = new Parameters();
            p.AddParameter<bool>("Combat", false);
            UpdateAnimator("Combat", p);
        }
    }
    public void GenerateGenericInputKeys()
    {
        movementKeys.Add("forward", KeyCode.W);
        movementKeys.Add("backward", KeyCode.S);
        movementKeys.Add("left", KeyCode.A);
        movementKeys.Add("right", KeyCode.D);
        movementKeys.Add("strafe", KeyCode.LeftShift);
        movementKeys.Add("jump", KeyCode.Space);

        combatKeys.Add("sheathe", KeyCode.E);
    }
}
