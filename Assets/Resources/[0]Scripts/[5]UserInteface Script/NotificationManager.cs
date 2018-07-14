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
        public List<MGColliderHelper> queueMinigames;
        public BareboneCharacter npcInteracting;
        public List<BareboneCharacter> queueCharacter;
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
                NotifyMinigame(p.GetWithKeyParameterValue<MGColliderHelper>("Checker", null));
            }
            else if (p.HasParameter("Character"))
            {
                NotifyNearNPC(p.GetWithKeyParameterValue<BareboneCharacter>("Character", null));
            }
        }

        public void NotifyNearNPC(BareboneCharacter thisCharacter = null)
        {
            if(thisCharacter == null)
            {
                return;
            }
            npcInteracting = (npcInteracting == thisCharacter) ? null : thisCharacter;
            interactNotif.SetActive(npcInteracting != null);

            minigameNotif.SetActive(npcInteracting != null);
            
        }


        public void NotifyMinigame(MGColliderHelper thisChecker = null)
        {
            if(thisChecker == null)
            {
                return;
            }
            minigameFromThis = (minigameFromThis == thisChecker) ? null : thisChecker;
            minigameNotif.SetActive(minigameFromThis != null);
        }
        
    }
}
