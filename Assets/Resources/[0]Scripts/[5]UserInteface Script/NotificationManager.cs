using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Utilities;
using EventSystem;
using Barebones.Minigame;
using Barebones.Characters;

namespace UserInterface
{
    public class NotificationManager : BaseManager {

        [Header("References")]
        public GameObject minigameNotif;
        public GameObject interactNotif;
        //public CharacterNotification interactNotif;

        [Header("Notification System")]
        public MGColliderHelper minigameFromThis;
        public BareboneCharacter npcInteracting;
        public void Start()
        {
            EventBroadcaster.Instance.AddObserver(EventNames.NOTIFY_PLAYER_INTERACTION, NotifyPlayer);
        }

        public void NotifyPlayer(Parameters p = null)
        {
            if(p == null)
            {
                return;
            }
            if (p.HasParameter("Checker")) 
            {
                MGColliderHelper tmp = p.GetWithKeyParameterValue<MGColliderHelper>("Checker", null);
                NotifyMinigame(tmp, p.GetWithKeyParameterValue<bool>("Entering", false));
            }
            else if (p.HasParameter("Character"))
            {
                BareboneCharacter tmp = p.GetWithKeyParameterValue<BareboneCharacter>("Character", null);
                NotifyNearNPC(tmp, p.GetWithKeyParameterValue<bool>("Entering", false));
            }
        }

        public void NotifyNearNPC(BareboneCharacter thisCharacter = null, bool isEntering = false)
        {
            if(thisCharacter == null)
            {
                return;
            }
            npcInteracting = (npcInteracting == thisCharacter) ? null : thisCharacter;
            interactNotif.SetActive(npcInteracting != null);
            
        }


        public void NotifyMinigame(MGColliderHelper thisChecker = null, bool isEntering = false)
        {
            if(thisChecker == null)
            {
                return;
            }
            if (isEntering)
            {
                minigameFromThis = thisChecker;
            }
            else
            {
                if(minigameFromThis == thisChecker)
                {
                    minigameFromThis = null;
                }
            }
             minigameNotif.SetActive(isEntering);
        }
        
    }
}
