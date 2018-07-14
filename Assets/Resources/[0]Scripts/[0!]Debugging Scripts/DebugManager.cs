using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Barebones;
using Barebones.Characters;
using CameraBehaviour;

namespace UserInterface
{
[ExecuteInEditMode]
public class DebugManager : BaseManager {
        public CameraControl playerCamera;
        public Transform selectedObject;


        [Header("Object Debug Controls")]
        public Button objectInfoButton;
        public Button animationButton;
        public Button reactionButton;

        [Header("Object Debug Panels")]
        public GameObject ObjectInfotab;
        public ObjectInformation infoTab;

        public void OnEnable()
        {
            playerCamera = FindObjectOfType<CameraControl>();
            if(playerCamera == null)
            {
                return;
            }
            if(playerCamera.objectToFocus != null)
            {
                selectedObject = playerCamera.objectToFocus;
            }
        }
        public void ShowObjectInfoTab()
        {
            if(!ObjectInfotab.activeSelf)
            {
                ObjectInfotab.SetActive(true);
                if (selectedObject != null)
                {
                    Debug.Log("Selected Object is Valid! " + selectedObject.name);
                    infoTab.thisObject = selectedObject.gameObject;
                }
                else Debug.Log("Cant Send reference due to not completely finding one!");
            }
        }
        public void OnDisable()
        {
            ObjectInfotab.SetActive(false);
            AdjustButtons(-1);
        }

        // Buttons Turns off and On depending on what you clicked
        public void AdjustButtons(int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    objectInfoButton.interactable = false;
                    animationButton.interactable = true;
                    reactionButton.interactable = true;
                    break;
                case 1:
                    animationButton.interactable = false;
                    objectInfoButton.interactable = true;
                    reactionButton.interactable = true;
                    break;
                case 2:
                    reactionButton.interactable = false;
                    animationButton.interactable = true;
                    objectInfoButton.interactable = true;
                    break;
                case -1:
                    reactionButton.interactable = true;
                    animationButton.interactable = true;
                    objectInfoButton.interactable = true;
                    break;
            }
        }
    }
}
