using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UserInterface
{
    public class UIManager : BaseManager {

        public GameObject debugManager;
        public bool DebugActive = false;

    #if UNITY_EDITOR
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DebugActive = !DebugActive;
                debugManager.SetActive(DebugActive);
            }
        }
    #endif
    }
}
