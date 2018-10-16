using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

using EventFunctionSystem;
using Utilities;
using Barebones;
using Barebones.Characters;
using CharacterStateMachine;
using CameraBehaviour;
using UserInterface;

namespace PlayerControl
{
    public class CharacterControl : MonoBehaviour
    {

        [System.Serializable]
        public class MoveSettings
        {
            [SerializeField] public float forwardVel = 25.0f;
            [SerializeField] public float rotateVel = 300;
            [SerializeField] public float jumpVel = 20.0f;
            [SerializeField] public float disToGrounded = 0.5f;
            [SerializeField] public LayerMask ground;
            [SerializeField] public bool touchedTrigger = false;
        }

        [System.Serializable]
        public class PhysSettings
        {
            public float downAccel = 0.75f;
        }

        [System.Serializable]
        public class InputSettings
        {
            [SerializeField] public float inputDelay = 0.1f;
            [SerializeField] public string fwd_Axis = "Vertical";
            [SerializeField] public string turn_Axis = "Horizontal";
            [SerializeField] public string Jump_Axis = "Jump";
        }

        [Header("Debugging")]
        public bool debugMode = false;

        [Header("Player Mode Checker")]
        [SerializeField] protected BareboneCharacter character;
        [SerializeField] private bool inMinigame = false;
        [SerializeField] private bool combatMode = false;
        [SerializeField] private bool cameraBasedFacing = false;

        public bool CombatMode
        {
            get
            {
                return this.combatMode;
            }
        }

        [Header("Player Jump Limiter")]
        [SerializeField] private Vector3[] highGroundRaycast;
        //private Vector3 backGroundRaycast = new Vector3(1, -1, -1);
        //private Vector3 midGroundraycast = new Vector3(1, -1, 0);
        //private Vector3 adjustedTransform;
        private CapsuleCollider characterCenter;

        [Header("Movement Settings")]
        public MoveSettings moveSettings = new MoveSettings();
        public PhysSettings physSettings = new PhysSettings();
        public InputSettings inputSettings = new InputSettings();
        float rotX = 0;
        float rotY = 0;

        [Header("References")]
        [SerializeField] private GameObject camera;
        Vector3 velocity = Vector3.zero;
        Quaternion targetRotation;
        Vector3 combatRotation;
        [SerializeField] private Rigidbody rBody;
        [SerializeField] private float forwardInput, turnInput, jumpInput;
        [SerializeField] private bool justJumped, tooHigh;

        public BareboneCharacter GetCharacter
        {
            get
            {
                return this.character;
            }
        }
        public GameObject GetTarget
        {
            get
            {
                return this.character.TargetObject;
            }
        }
        public GameObject SetTarget
        {
            set
            {
                this.character.TargetObject = value;
            }
        }
        public void OnDrawGizmos()
        {
            //Gizmos.DrawSphere(transform.position, characterCenter.radius);

        }
        // Checks if 
        public bool IsGrounded
        {
            get
            {
                // Lower Surface
                RaycastHit hit;
                float distToStuff = 0;
                Vector3 center = transform.position + transform.forward + transform.up;
                if (Physics.SphereCast(transform.position, characterCenter.radius, Vector3.down, out hit))
                {
                    if (!hit.collider.isTrigger)
                    {
                        distToStuff = hit.distance;
                    }
                    else
                    {
                        distToStuff = 10.0f;
                    }
                }
                if (distToStuff < 0.1f)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool TooHigh
        {
            get
            {
                RaycastHit hit;
                // if player is already Jumping Check if he's already spamming jump
                if (jumpInput > 0)
                {
                    for (int i = 0; i < highGroundRaycast.Length; i++)
                    {
                        if (Physics.Raycast(transform.position, transform.TransformDirection(highGroundRaycast[i] * 0.5f), out hit))
                        {
                            tooHigh = true;
                            //Debug.Log("Hitting : " + hit.transform.gameObject.name);
                            Debug.DrawRay(transform.position, transform.TransformDirection(highGroundRaycast[i]) * 0.5f, Color.red);
                        }
                        else Debug.DrawRay(transform.position, transform.TransformDirection(highGroundRaycast[i]) * 0.5f, Color.yellow);
                    }
                }
                else
                {
                    tooHigh = false;
                }
                return tooHigh;
            }
        }

        public Quaternion TargetRotation
        {
            get
            {
                return targetRotation;
            }
        }

        public virtual void Start()
        {
            Parameters p = new Parameters();
            p.AddParameter<GameObject>("player", this.gameObject);
            EventBroadcaster.Instance.PostEvent(EventNames.SET_UI_PLAYER_REFERENCE, p);
            characterCenter = GetComponent<CapsuleCollider>();
            justJumped = false;
            targetRotation = transform.rotation;
            if (GetComponent<Rigidbody>())
            {
                rBody = GetComponent<Rigidbody>();
            }
            else Debug.LogError("Character Needs Rigidbody!");

            if (GetComponent<BareboneCharacter>())
            {
                character = GetComponent<BareboneCharacter>();
            }
            else Debug.LogError("Character has no BareboneCharacter! Cant Check Current States!!");
            forwardInput = turnInput = jumpInput = 0;

            // ANIMATION

        }

        public void GetInput()
        {
            forwardInput = Input.GetAxis(inputSettings.fwd_Axis);
            turnInput = Input.GetAxis(inputSettings.turn_Axis);
            jumpInput = Input.GetAxisRaw(inputSettings.Jump_Axis);
        }

        public void Update()
        {
            if (!inMinigame)
            {
                UserInput();
                if (character.CurrentAction != ActionType.STUNNED)
                {
                    if (IsGrounded)
                    {
                        GetInput();
                        if(character.LivingState == LivingState.COMBAT)
                        {
                            Turn();
                        }
                    }
                }
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    debugMode = !debugMode;
                }
                if (debugMode)
                {
                    //Debug.Log("Character Control of :" + this.gameObject.name + " is in Debug Mode!");
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        //Debug.Log("Growing!");
                        // ANIMATION ADJUSTMENT HERE!
                        Parameters param = new Parameters();
                        param.AddParameter<bool>("Grow", true);
                        param.AddParameter<int>("EvolutionIndex", +1);
                        // TODO: INSERT ANIMATION CALLER HERE for the FINISH GROWTH
                        character.UpdateAnimator("Grow", param);
                    }
                }
#endif
            }
        }

        public void FixedUpdate()
        {
            if(!character.SkillActivate && !character.IsEvolving)
            {
                if (character.LivingState == LivingState.IDLE)
                {
                    RotateAndMoveWithDirection();
                }
                Jump();
                rBody.velocity = transform.TransformDirection(velocity);
            }

        }

        public void LateUpdate()
        {
            if(!character.IsEvolving)
            {
                if(character.LivingState == LivingState.COMBAT)
                {
                    RotateWithAandD();
                }
            }
        }
        public void Turn()
        {
            // PLAYER FACE ROTATION.
            if (!cameraBasedFacing)
            {
                //RotateWithAandD();
                
            }
                // CAMERA BASED ROTATION
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, camera.transform.eulerAngles.y, 0.0f);
            }
        }

        private void RotateWithAandD()
        {
            Parameters param = new Parameters();
            //   Debug.Log("RotateWithAandD");
            if (character.LivingState == LivingState.IDLE)
            {
                if (turnInput > inputSettings.inputDelay)
                {
                    targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * turnInput * Time.deltaTime, Vector3.up);
                }
                transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y, 0);
                // Turn
                if (turnInput > inputSettings.inputDelay || turnInput < -inputSettings.inputDelay)
                {
                    velocity.x = moveSettings.forwardVel * turnInput;
                    param.AddParameter<bool>("Yaxis", true);
                }
                else
                {
                    velocity.x = 0;
                    param.AddParameter<bool>("Yaxis", false);
                }
                // Forward
                if (forwardInput > inputSettings.inputDelay || forwardInput < -inputSettings.inputDelay)
                {
                    velocity.z = moveSettings.forwardVel * forwardInput;
                    param.AddParameter<bool>("Zaxis", true);
                }
                else
                {
                    // Debug.Log("IDLE!");
                    velocity.z = 0;
                    param.AddParameter<bool>("Zaxis", false);
                }
           }
           else if(character.LivingState == LivingState.COMBAT)
            {
                if (turnInput > inputSettings.inputDelay)
                {
                    //  Debug.Log("Forward");
                    param.AddParameter<bool>("Yaxis", true);
                    velocity.x = moveSettings.forwardVel * turnInput;
                }
                else if(turnInput < inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Yaxis", false);
                    velocity.x = moveSettings.forwardVel * turnInput;
                }
                else
                {
                    param.AddParameter<bool>("Yaxis", false);
                    velocity.x = 0;
                }
                if (Mathf.Abs(forwardInput) > inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Zaxis", true);
                    velocity.z = moveSettings.forwardVel * forwardInput;
                }
                else
                {
                    param.AddParameter<bool>("Zaxis", false);
                    velocity.z = 0;
                }
                if (character.LivingState == LivingState.IDLE && IsGrounded)
                {
                    // Debug.Log("IDLE!");
                    param.UpdateParameter<bool>("Zaxis", false);
                    param.UpdateParameter<bool>("Yaxis", false);
                    character.UpdateCurrentState(CharacterStates._IDLE, param);
                }
                if (character.TargetObject != null)
                {
                    // Mathf.Abs is not Appropriate because distance sometimes return wrong results
                    //Vector3 xPlayer = new Vector3(transform.position.x, 0, character.TargetObject.transform.position.z);
                    //Vector3 xTarget = new Vector3(character.TargetObject.transform.position.x, 0, character.TargetObject.transform.position.z);
                    Vector3 xPlayer = transform.position;
                    Vector3 xTarget = character.TargetObject.transform.position;

                    float distance = Vector3.Distance(xPlayer, xTarget);

                   // Debug.Log("Distance To Target : " + distance);
                    if (distance > 0.1f)
                    {

                        Vector3 targetDir = character.TargetObject.transform.position - transform.position;
                        targetDir.y = 0.0f;

                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.time * 10f);
                    }
                }
            }
           
            param.AddParameter<float>("ForwardInput", forwardInput);

            if (turnInput == 0 && forwardInput == 0)
            {
                param.AddParameter<bool>("Zaxis", false);
                param.AddParameter<bool>("Yaxis", false);
                character.UpdateCurrentState(CharacterStates._IDLE, param);
            }
            else
            {
                character.UpdateCurrentState(CharacterStates._MOVING, param);
            }
        }

        private void RotateAndMoveWithDirection()
        {
            float camY = new float();
            Parameters param = new Parameters();
            camY = camera.transform.eulerAngles.y;
            // Forward
            if (forwardInput > inputSettings.inputDelay)
            {
                param.AddParameter<bool>("Zaxis", true);
                // Right
                if (turnInput > inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Yaxis", true);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY + 45, 0), 10.0f * Time.deltaTime);
                }
                //Left
                else if (turnInput < -inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Yaxis", true);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY - 45, 0), 10.0f * Time.deltaTime);
                }
                // Forward
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY, 0), 10.0f * Time.deltaTime);
                }
                velocity.z = moveSettings.forwardVel * forwardInput;
            }
            //Backward
            else if (forwardInput < -inputSettings.inputDelay)
            {
                param.AddParameter<bool>("Zaxis", true);
                camY = camera.transform.eulerAngles.y;
                // Right
                if (turnInput > inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Yaxis", true);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY + 145, 0), 10.0f * Time.deltaTime);
                }
                //Left
                else if (turnInput < -inputSettings.inputDelay)
                {
                    param.AddParameter<bool>("Yaxis", true);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY - 145, 0), 10.0f * Time.deltaTime);
                }
                // Backward
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY + 180, 0), 10.0f * Time.deltaTime);
                }
                velocity.z = moveSettings.forwardVel * -forwardInput;
            }
            else if(turnInput > inputSettings.inputDelay)
            {
                param.AddParameter<bool>("Yaxis", true);
                param.AddParameter<bool>("Zaxis", false);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY + 90, 0), 10.0f * Time.deltaTime);
                velocity.z = moveSettings.forwardVel * turnInput;
            }
            else if(turnInput < -inputSettings.inputDelay)
            {
                param.AddParameter<bool>("Yaxis", true);
                param.AddParameter<bool>("Zaxis", false);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camY - 90, 0), 10.0f * Time.deltaTime);
                velocity.z = moveSettings.forwardVel * -turnInput;
            }
            param.AddParameter<float>("ForwardInput", forwardInput);

            // IDLE
            if (turnInput == 0 && forwardInput == 0)
            {
               
                param.AddParameter<bool>("Zaxis", false);
                param.AddParameter<bool>("Yaxis", false);
                velocity.z = 0;
                velocity.x = 0;
                if (character)
                {
                        character.UpdateCurrentState(CharacterStates._IDLE, param);
                }
            }
            else
            {
                character.UpdateCurrentState(CharacterStates._MOVING, param);
            }

        }
        private void ResetMovement()
        {
            velocity.z = 0;
            velocity.x = 0;
        }
        public void Jump()
        {
            Parameters param = new Parameters();
            if (jumpInput > 0 && IsGrounded)
            {
                param.AddParameter<bool>("Jump", true);
                justJumped = true;
                //jump
                velocity.y = moveSettings.jumpVel;
                character.UpdateCurrentState(CharacterStates._JUMPING, param);
            }
            else if (jumpInput == 0 && IsGrounded)
            {
                param.AddParameter<bool>("Jump", false);
                //0 out our velocity.y
                velocity.y = 0;
                justJumped = false;
                character.UpdateCurrentState(CharacterStates._IDLE, param);
            }
            else
            {
                //Debug.Log("Still Falling");
                //decrease velocity.y
                velocity.y -= physSettings.downAccel;
            }
            //Debug.Log("JUMP INPUT : " + jumpInput + " IsGrounded : " + IsGrounded);
        }

        public void UserInput()
        {
            // Selecting BareboneObjects
            SelectObject();
            Combat();
            GatherResources();
            GetItemOnGround();
        }

        private void Combat()
        {
            if (Input.GetButtonDown("SwitchCombat"))
            {
                ResetMovement();
                character.Combat();
                cameraBasedFacing = !cameraBasedFacing;
            }
        }
        private void GatherResources()
        {
            if (Input.GetButtonDown("GatherResource"))
            {
                if (NotificationManager.Instance.minigameFromThis != null)
                {
                    bool checkNotif = NotificationManager.Instance.CheckMinigameNotification();
                    inMinigame = true;
                    character.TargetObject = NotificationManager.Instance.minigameFromThis.gameObject;
                    EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_VIEWMODE_MINIGAME);
                }
            }
        }
        private void SelectObject()
        {
            if (Input.GetMouseButtonDown(0) && camera.GetComponent<CameraControl>().EnableCursor)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                RaycastHit hitInfo;
                Parameters param = new Parameters();
                var layerMask = ~(1 << 11 & 10);
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hitInfo, 1000.0f, layerMask);
                if (hit)
                {
                    // if Click hits Own Character
                    if(hitInfo.transform == this.transform)
                    {
                        return;
                    }
                    // if Click hits Something Else
                    if (hitInfo.transform.GetComponent<BareboneObject>())
                    {
                        character.TargetObject = hitInfo.transform.gameObject;
                        param = new Parameters();
                        param.AddParameter<bool>("Switch", false);
                        EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_MOUSE_SWITCH, param);
                        return;
                    }
                }
                // if there's no Hit at all
                else
                {
                    character.TargetObject = null;
                    EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_CLEAR_FOCUS);
                }
                param.AddParameter<bool>("Switch", false);
                EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_MOUSE_SWITCH, param);
                character.TargetObject = null;
                EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_CLEAR_FOCUS);
            }
        }
        
        private void GetItemOnGround()
        {
            if(Input.GetButtonDown("ItemNpcInteract"))
            {
                if (NotificationManager.Instance.itemNearby != null)
                {
                    bool checkNotif = NotificationManager.Instance.CheckItemNpcNotification();
                    NotificationManager.Instance.itemNearby.PickUpItem(character);
                    character.PickupItem(NotificationManager.Instance.itemNearby);
                    character.CurrentState = CharacterStateMachine.CharacterStates._INTERACTING;
                }
            }
        }
        public void SetCamera(GameObject cameraBase)
        {
            camera = cameraBase;
        }
    }
}