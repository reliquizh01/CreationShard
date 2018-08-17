using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
namespace UserInterface
{
    public class UIManager : BaseManager {

        public GameObject debugManager;
        public bool DebugActive = false;

        [Header("UI Managers")]
        public BaseManager uiHealthStamina;
        
        public void Update()
        {
    #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DebugActive = !DebugActive;
                debugManager.SetActive(DebugActive);
            }
    #endif
        }
    }
}
