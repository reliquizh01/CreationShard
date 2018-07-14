using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones.DamageSystem;
/// <summary>
/// Unlike the other Stats, Resistance can only level up when a player is taking damage
/// New type of damage creates a new type of resistance.
/// </summary>
namespace Barebones.Characters
{
    [Serializable]
    public class Resistance{

        [SerializeField]private List<BareboneResistance> attackResistance;
        [SerializeField]private List<ActionType> statProgressType;
       
        public BareboneResistance GetResistance(string thisName)
        {
            return attackResistance.Find(x => x.Name == thisName);
        }
        
        public void Initialize()
        {
            attackResistance = new List<BareboneResistance>();
            statProgressType = new List<ActionType>();
            if (!statProgressType.Contains(ActionType.ATTACKED))
            {
                statProgressType.Add(ActionType.ATTACKED);
            }
        }

        public void Addprogress(float expAddition, string name = null)
        {
            Debug.Log("Adding [" + name + "] to my list of things I can try to resist");
            BareboneResistance bareboneResistance = attackResistance.Find(x => x.Name == name);
            if (bareboneResistance != null)
            {
                bareboneResistance.Addprogress(expAddition);
            }
            else
            {
                if(name == null)
                {
                    Debug.Log("QQQQQQQQ : Trying to add an invalid Attack name, Please Check Error!!");
                }
                else
                {
                    bareboneResistance = new BareboneResistance();
                    bareboneResistance.Initialize();
                    bareboneResistance.Name = name;
                    attackResistance.Add(bareboneResistance);
                    bareboneResistance.Addprogress(expAddition);
                }
            }
        }
    }
}
