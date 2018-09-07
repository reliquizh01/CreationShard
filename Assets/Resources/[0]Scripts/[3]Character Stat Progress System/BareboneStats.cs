using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    ///  Used by all character stats which contains the functionality such as its progress and other stuff related to it
    /// </summary>
namespace Barebones.Characters
{
    // StatType  branches out the kinds of stats and the way they progress by Identifying the Action they're related to.
    [System.Serializable]
    public class BareboneStats
    {
        // Stat Name
        [SerializeField]private string name;
        
        //Stat ActionType
        [SerializeField] protected List<ActionType> statProgressType;

        public virtual List<ActionType> GetProgressType
        {
            get
            {
                if(statProgressType.Count == 0)
                {
                    BuildStat();
                }
                return this.statProgressType;
            }
        }

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

        //Stat Level
        protected int currentLevel;

        public int CurrentLevel
        {
            get
            {
                if(currentLevel == 0)
                {
                    currentLevel = 1;
                }

                return this.currentLevel;
            }
        }
        // Progress
        protected  float currentProgressCount;
        protected float nextProgressCount;
        

        public float CurrentProgressCount
        {
            get
            {
                return currentProgressCount;
            }
            set
            {
                currentProgressCount = value;
            }
        }

        public virtual void Initialize()
        {
            statProgressType = new List<ActionType>();
            name = this.GetType().Name;
            //Debug.Log(this.GetType().Name + " stats is initializing");
            /*Check if there's a Save File
            *  if(SaveFile.Contains.ThisSpecificStat
            */
            if(currentLevel <= 1)
            {
                BuildStat();
            }
        }
        //Increases progress
        public virtual void Addprogress(float expAddition)
        {
            Debug.Log("Stat : " + name + " is increasing by : " + expAddition);
            // Check if Progress surpassed the nextProgressCount
            if (currentProgressCount+expAddition > nextProgressCount)
            {
                currentProgressCount = currentProgressCount + expAddition;
                currentProgressCount -= nextProgressCount;

                IncreaseLevel();
            }
            else
            {
                currentProgressCount += expAddition;
            }

        }

        // Level up the stats
        public virtual void IncreaseLevel()
        {
            if (currentLevel > currentLevel + 1) return;

            currentLevel++;
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
            // Progress Count
            if(currentProgressCount > nextProgressCount)
            {
                while(currentProgressCount >= nextProgressCount)
                {
                    currentProgressCount -= nextProgressCount;
                    // Subject to Change, Increment should be balance
                    nextProgressCount += nextProgressCount;
                    IncreaseLevel();
                }
            }
        }
        public virtual void UpdateStats()
        {

        }
    }

}
