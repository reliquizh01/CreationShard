using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventFunctionSystem;
using Utilities;

using PlayerControl;

namespace CameraBehaviour
{
    [Serializable]
    public class CameraPosition
    {
       [SerializeField]public string posName;
       [SerializeField] public Vector3 rotation;
    }

    public class CameraControl : MonoBehaviour {

        public float cameraMoveSpeed = 120.0f;

        [Header("Camera")]
        public GameObject camera;

        [Header("Camera Hosts")]
        public GameObject hostPlayer;
        public Transform objectToFollow;
        public Transform objectToFocus;

        [Header("Camera Rotation")]
        public float inputSensitivity = 150.0f;
        public float rotX;
        public float rotY;

        [Header("Camera System")]
        public float clampAngle = 30.0f;
        public float mouseX;
        public float mouseY;
        public float finalInputX;
        public float finalInputZ;

        [Header("CameraPositions")]
        public CameraPosition[] cameraPositions;

        [Header("Switch")]
        public bool EnableCursor = false;
        public bool inMinigame = false;

        private void Start()
        {
            if(hostPlayer != null)
            {
                transform.position = hostPlayer.transform.position;
            }
            EventBroadcaster.Instance.AddObserver(EventNames.CAMERA_CHANGE_FOCUS, ChangeFocus);
            EventBroadcaster.Instance.AddObserver(EventNames.CAMERA_CLEAR_FOCUS, ClearFocus);
            EventBroadcaster.Instance.AddObserver(EventNames.CAMERA_MOUSE_SWITCH, CursorVisibilitySwitch);
            EventBroadcaster.Instance.AddObserver(EventNames.CAMERA_VIEWMODE_MINIGAME, SwitchToMinigameViewMode);
            hostPlayer.GetComponent<CharacterControl>().SetCamera(this.gameObject);

            Vector3 rot = transform.localRotation.eulerAngles;
            rotX = rot.x;
            rotY = rot.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.CAMERA_CHANGE_FOCUS, ChangeFocus);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.CAMERA_CLEAR_FOCUS, ClearFocus);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.CAMERA_MOUSE_SWITCH, CursorVisibilitySwitch);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.CAMERA_VIEWMODE_MINIGAME, SwitchToMinigameViewMode);
        }
        public void Update()
        {
            if(Input.GetButtonDown("LeftControl"))
            {
                CursorVisibilitySwitch();
            }
        }

        private void SwitchToMinigameViewMode(Parameters p = null)
        {
            Quaternion offset = Quaternion.Euler(0, cameraPositions[0].rotation.y - hostPlayer.transform.rotation.eulerAngles.y, 0);
            StartCoroutine(ToMinigameView(offset, 3.0f));
        }
        private void CursorVisibilitySwitch(Parameters p = null)
        {
            if(p != null)
            {
                EnableCursor = (p.HasParameter("Switch")) ? p.GetWithKeyParameterValue<bool>("Switch", false) : !EnableCursor;
            }
            else
            {
                EnableCursor = !EnableCursor;
            }
            if (EnableCursor)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = EnableCursor;
        }

        public void FixedUpdate()
        {
            if (!inMinigame)
            {
                if (!EnableCursor)
                {
                    if (objectToFocus)
                    {
                        FocusOnTarget();
                    }
                }
            }
        }
        private void LateUpdate()
        {
            if(!inMinigame)
            {
                CameraUpdater();
                if (!EnableCursor)
                {
                    if (!objectToFocus)
                    {
                        BaseMovement();
                    }
                }
            }
        }

        public void CameraUpdater()
        {
            // set Target
            Transform target = objectToFollow;

            // Move Towards
            float step = cameraMoveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

        public void BaseMovement()
        {
            float inputX = Input.GetAxis("RightStickHorizontal");
            float inputZ = Input.GetAxis("RightStickVertical");

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            finalInputX = inputX + mouseX;
            finalInputZ = inputZ + mouseY;

            rotY += finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;

            //Stop it from going around and around
            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
        }

        public void FocusOnTarget()
        {
            if(objectToFocus == null)
            {
                return;
            }
            float inputX = Input.GetAxis("RightStickHorizontal");
            float inputZ = Input.GetAxis("RightStickVertical");

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            finalInputX = objectToFollow.rotation.y;
            finalInputZ = inputZ + mouseY;


            rotY += finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;

            //Stop it from going around and around
            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            
            Quaternion localRotation = Quaternion.Euler(rotX, objectToFollow.eulerAngles.y, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, 10f * Time.fixedDeltaTime);
            
        }

        public void ChangeFocus(Parameters param = null)
        {
            if(param == null)
            {
                return;
            }
            objectToFocus = param.GetWithKeyParameterValue<Transform>("FocusTo", null);
        }
        public void ClearFocus(Parameters param = null)
        {
            if(objectToFocus == null)
            {
                return;
            }
            objectToFocus = null;
        }
        
        public IEnumerator ToMinigameView(Quaternion target, float overTime)
        {
            float startTime = Time.time;
            inMinigame = true;
            while (Time.time < startTime + overTime)
            {
                Debug.Log("Rotating!");
                transform.rotation = Quaternion.Slerp(transform.rotation,target, (Time.time - startTime) / overTime);
                yield return null;
            }
            transform.rotation = target;
        }
    }
}
