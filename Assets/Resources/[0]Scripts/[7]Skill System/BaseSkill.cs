using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

using Barebones.Characters;

namespace Barebones.Skill
{
    public enum SkillType
    {
        ACTIVE = 0,
        PASSIVE = 1,
    }

    [Serializable]
    public class BaseSkill{

        [Header("Skill Information")]
        [SerializeField] private string skillName;
        [SerializeField] private List<BareboneStats> statDependent;
        [SerializeField] private SkillType skillType;

        [Header("Checker")]
        [SerializeField] private bool hasCooldown;
        [SerializeField] private bool canActivate;

        [Header("Adjustable Attributes")]
        [SerializeField] private float cooldownTime;

        public virtual void Initialize()
        {

        }

        public virtual void CheckCondition(string buttonTapped = "")
        {

        }

        public virtual void Activate()
        {

        }

        public virtual IEnumerator cooldown()
        {
            canActivate = false;
            yield return new WaitForSeconds(cooldownTime);
            canActivate = true;
        }
    }
}
