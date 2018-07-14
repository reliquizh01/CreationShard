using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones;
using Barebones.Characters;

namespace Barebones.Minigame
{
    public enum TriggerType
    {
       COLLIDER = 0,
       ITEM = 1,
       EVENTS = 2,
    };
    public class MinigameBase : MonoBehaviour {

        [SerializeField] private BareboneCharacter playerInteracting;
        [SerializeField] protected string statName;
        [SerializeField] protected TriggerType triggerType;
        public TriggerType TriggerType
        {
            get
            {
                return this.triggerType;
            }
        }
        public string GetStatKey
        {
            get
            {
                return statName;
            }
        }
        // Check if Player still doesnt have the said stats for this specific Minigame
       public void PlayerHasMinigameStats()
        {
            if(playerInteracting == null)
            {
                Debug.Log(StringUtils.RedString("Object Trying to Interact has no BareboneCharacter Script!"));
                return;
            }
            BareboneStats checkStats = playerInteracting.CharacterStats.Find(x => x.Name == statName);
            if(checkStats == null)
            {
                Debug.Log(StringUtils.YellowString("Object has no stats with the following name : " + statName + " Will Add Stats to Player"));
                WoodCutting woodCutting = new WoodCutting();
                woodCutting.Initialize();
            }
        }
        // Starts minigame
        public virtual void StartMinigame()
        {
            
        }
    }
}
