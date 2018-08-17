using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones.Characters;
using Barebones.DamageSystem;
using Utilities;
using EventFunctionSystem;

namespace Barebones.Skill
{
    public enum ComboKey
    {
        forward = 0,
        backward = 1,
        right = 2,
        left = 3,
        jump = 4,
        none = 5
    }

    [Serializable]
    public class BaseSkill
    {
        private BareboneObject owner;
        private BareboneCharacter ownerStat;

        public string skillName;
        public string skillKey;
        public ComboKey[] combo;
        public int currentComboIndex;
        public ActionType actionType;

        public bool skillEnabled;

        public bool actionBased;
        public bool combatSkill;
        public bool isDoubleTap;
        public bool checkAllKeys;
        public bool inChecking;
        public bool activate;
        public bool inCooldown;
        public bool simultaneousCast;

        public ComboKey prevKey;

        [Header("Skill Costs")]
        public List<BareboneDamage> skillCost;

        [Header("Cooldown")]
        public float maxCooldown;
        public float curCooldown;

        [Header("Temp Fix [Animation Duration]]")]
        public float skillDuration = 0.5f;
        public float curDuration = 0.5f;
        
        /// <summary>
        /// Checks Incrementing Combo keys
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool CheckKey(string check)
        {
            if(inCooldown)
            {
                return false;
            }

            if(check == "")
            {
                if(!inChecking)
                prevKey = ComboKey.none;
                return false;
            }

            if(!checkAllKeys)
            {
                return ComboBased(check);
            }
            else
            {
                return CheckAll(check);
            }
        }

        public void SetOwner(BareboneObject newOwner)
        {
            owner = newOwner;
            ownerStat = (owner.GetComponent<BareboneCharacter>()) ? owner.GetComponent<BareboneCharacter>() : null;
            
        }

        private bool ComboBased(string check)
        {
            if (!inChecking)
            {
                if (combo[0].ToString() == check)
                {
                    prevKey = combo[currentComboIndex];
                    currentComboIndex += 1;
                    inChecking = true;
                    return true;
                }
                return false;
            }

            if (combo[currentComboIndex].ToString() == check)
            {
                if(currentComboIndex + 1 == combo.Length)
                {
                    Activate();
                }
                prevKey = combo[currentComboIndex];
                currentComboIndex++;
                return true;
            }
            else
            {
                currentComboIndex = 0;
                inChecking = false;
                prevKey = ComboKey.none;
                return false;
            }
        }

        private bool CheckAll(string check)
        {
            for (int i = 0; i < combo.Length; i++)
            {
                if(combo[i].ToString().ToLower() == check)
                {
                    inChecking = true;
                    if (prevKey == ComboKey.none) prevKey = combo[i];
                    else if (isDoubleTap) return DoubleTap(combo[i]);

                    return true;
                }
            }
            inChecking = false;
            prevKey = ComboKey.none;
            return false;
        }
        
        private bool DoubleTap(ComboKey nextKey)
        {
            if(prevKey == nextKey)
            {
                activate = true;
                Activate(nextKey.ToString().ToLower());
                return true;
            }
            else
            {
                prevKey = nextKey;
                return false;
            }
        }
        
        public virtual void Activate(string checker = "")
        {
            // Check if Skill is a combat skill
            if(combatSkill)
            {
                if(owner.GetComponent<BareboneCharacter>().LivingState != LivingState.COMBAT)
                {
                    //Debug.Log(Utilities.StringUtils.YellowString("Trying to Activate : " + skillName + " but it is a combat skill!"));
                    return;
                }
            }
            Debug.Log("Activating Skill : " + skillName);
            // Animation Call Here
            // MoveTowards is a FORCE MOVE to make the object move to a specific direction
            owner.MoveTowards(checker);
            owner.SkillActivate = true;
            if(skillCost.Count != 0)
            {
                    Parameters p = new Parameters();
                for (int i = 0; i < skillCost.Count; i++)
                {
                    string dmgName = "DamageType" + i;
                    p.AddParameter<BareboneDamage>(dmgName, skillCost[i]);
                }
                owner.ReceiveDamage(p);
            }
            skillEnabled = true;
            if(actionBased)
            {
                ownerStat.PreviousAction = ownerStat.CurrentAction;
                ownerStat.ChangeAction(actionType);
            }
        }

        public void SkillFinish()
        {
            prevKey = ComboKey.none;
            inChecking = false;
            activate = false;
            owner.SkillActivate = false;
            inCooldown = true;
            currentComboIndex = 0;
            curDuration = skillDuration;
            if (actionBased)
            {
                ownerStat.ChangeAction(ActionType.IDLE);
            }
        }

        public void ResetSkill()
        {
            prevKey = ComboKey.none;
            inChecking = false;
            activate = false;
            currentComboIndex = 0;
            curDuration = skillDuration;
        }
        
        public void ReduceCooldown(float increment)
        {
            if(curCooldown <= 0)
            {
                inCooldown = false;
                curCooldown = maxCooldown;
                return;
            }

            curCooldown -= increment;
        }

        public void ReduceSkillDuration(float increment)
        {
          if(curDuration > 0)
            {
                curDuration -= increment;
                return;
            }

            SkillFinish();
        }

    }
}
