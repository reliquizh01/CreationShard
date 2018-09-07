using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using EventFunctionSystem;

namespace UserInterface
{
    public class PlayerTabManager : BaseManager {

        public enum Tabs
        {
            SKILLTAB = 0,
            CHARACTERTAB = 1
        }

        private static PlayerTabManager instance = null;
        public static PlayerTabManager Instance
        {
            get
            {
                if(instance == null)
                {
                    PlayerTabManager tmp = FindObjectOfType<PlayerTabManager>();
                    instance = tmp;
                }
                return instance;
            }
        }
        public bool isActive = false;
        [SerializeField]private Tabs currentTab;
        public Tabs CurrentTab
        {
            get
            {
                return this.currentTab;
            }
            set
            {
                this.currentTab = value;
            }
        }
        [Header("Tab Buttons")]
        public ButtonManager btnManager;

        [Header("Tabs")]
        public TabManager tabManager;

        [Header("Animation System")]
        public Animation animation;


        public void Update()
        {
            if(Input.GetButtonDown("SkillTab"))
            {
                Debug.Log("SkillTab clicked!");
                SwitchTab(Tabs.SKILLTAB);
            }
        }

        public void SwitchTab(Tabs newTab)
        {
            if(currentTab == newTab)
            {
                isActive = !isActive;
                AnimateTab();
            }

        }
        public void AnimateTab()
        {
            if(isActive)
            {
                animation.Play("PlayerTab_Open");
            }
            else if (!isActive)
            {
                animation.Play("PlayerTab_Close");
            }
        }
    }
}
