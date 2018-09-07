using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
    [Serializable]
    public class BareboneResistance 
    {

        // Stat Name
        [SerializeField] private string name;
        // Progress
        protected float currentProgressCount;
        protected float nextProgressCount;
        //Stat Level
        protected int currentLevel;


        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public int CurrentLevel
        {
            get
            {
                return this.currentLevel;
            }
        }

        public void Initialize()
        {
            BuildStat();

        }
        // Level up the stats
        public virtual void IncreaseLevel()
        {
            if (currentLevel > currentLevel + 1) return;

            currentLevel++;
           
        }

        public virtual void Addprogress(float expAddition, string newName = null)
        {
            expAddition = expAddition / currentLevel;
          //  Debug.Log("QQQQQQ : PROGRESS INCREASE : " + expAddition + ":" + name);
            Debug.Log("Current Progress : " + currentProgressCount + "/" + nextProgressCount);
            // Check if Progress surpassed the nextProgressCount
            if (currentProgressCount + expAddition > nextProgressCount)
            {
                currentProgressCount = currentProgressCount + expAddition;
                currentProgressCount -= nextProgressCount;
                Debug.Log("Resistance : " + name + " Level up! [CURRENT LEVEL : " + (currentLevel + 1) + "]");
                IncreaseLevel();
            }
            else
            {
                currentProgressCount += expAddition;
            }
        }

        // Build Stats creates a stat if one doesnt exists
        public virtual void BuildStat()
        {
            // Safety Nets (incase values doesnt make sense)
            // Level
            if (currentLevel <= 0)
            {
                currentLevel = 1;
            }
            if(nextProgressCount <= 0)
            {
              //  Debug.Log("NPC @ 100");
                nextProgressCount = 100;
            }
            // Progress Count
            if (currentProgressCount > nextProgressCount)
            {
                while (currentProgressCount >= nextProgressCount)
                {
                    currentProgressCount -= nextProgressCount;
                    // Subject to Change, Increment should be balance
                    nextProgressCount += nextProgressCount;
                    IncreaseLevel();
                }
            }
        }
    }
}
