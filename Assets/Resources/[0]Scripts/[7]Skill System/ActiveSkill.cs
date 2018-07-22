using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

namespace Barebones.Skill
{
    public enum Buttons
    {
        None = 0,
        Forward = 1,
        Backward = 2,
        Left = 3,
        Right = 4,
        Jump = 5,
        Run = 6,
        LeftClick = 7,
        RightClick = 8,
    }
    [Serializable]
    public class ComboKey
    {
        public bool tapped = false;
        public Buttons key;
    }
    [Serializable]
    public class ActiveSkill : BaseSkill {

        [Header("Additional Checker")]
        [SerializeField] private bool noComboTrigger;
        [SerializeField] private bool isDoubleTap;

        [Header("Combo System")]
        [SerializeField] private List<ComboKey> comboKeys;
        [SerializeField] private ComboKey previousKey;

        [Header("Combo Checker")]
        [SerializeField] private bool initialButtonTapped;
        [SerializeField] private float nextTapTimer;
        [SerializeField] private float curTapTimer;
        [SerializeField] private int currentComboKey;

        public ComboKey GetInitialKey
        {
            get
            {
                if(comboKeys != null && comboKeys.Count > 0)
                {
                    return this.comboKeys[0];
                }
                else
                {
                    Debug.Log(Utilities.StringUtils.YellowString("Skill Initial Combo Key is Invalid, you might've forgotten to initialize it!"));
                    ComboKey newButton = new ComboKey();
                    newButton.key = Buttons.None;
                    return newButton;
                }
            }
        }


        public override void CheckCondition(string buttonTapped = "")
        {
            // Failsafe
            if(buttonTapped == "")
            {
                return;
            }

            if (isDoubleTap)
            {
                CheckAllKeys(buttonTapped);
            }
            else
            {
                if(!initialButtonTapped)
                {
                    if(GetInitialKey.key.ToString() == buttonTapped)
                    {
                        initialButtonTapped = true;
                    }
                }
                else
                {

                }
            }
        }
        
        public void CheckNextKey()
        {
            // Time Checker
            if(curTapTimer >= nextTapTimer)
            {
                return;
            }
            curTapTimer = 0;
        }
        // Intended for the use of flexible skill
        public void CheckAllKeys(string buttonTapped)
        {
            // if there's no previousKey
            if (!initialButtonTapped && previousKey == null)
            {
                for (int i = 0; i < comboKeys.Count; i++)
                {
                    if (comboKeys[i].key.ToString() == buttonTapped)
                    {
                        initialButtonTapped = true;
                        previousKey = comboKeys[i];
                        currentComboKey = i;
                    }
                }
            }
        }
        
        public void Reset()
        {
            initialButtonTapped = false;
            previousKey = null;
            currentComboKey = 0;
        }

        public override void Initialize()
        {

        }
        public override void Activate()
        {


        }
    }
}
