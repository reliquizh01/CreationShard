using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Utilities;
using EventFunctionSystem;
using Barebones.Minigame;
using Barebones.Characters;
using Barebones.Items;
using PlayerControl;

namespace UserInterface
{
    public class NotificationManager : BaseManager {

        [Header("References")]
        public GameObject minigameNotif;
        public GameObject interactNotif;
        //public CharacterNotification interactNotif;

        [Header("Notification System")]
        public MGColliderHelper minigameFromThis;
        public BareboneNpcBase npcInteracting;
        public ItemBase itemNearby;

        #region Const, Static, and Singleton
        private static NotificationManager instance = null;
        public static NotificationManager Instance
        {
            get
            {
                if(instance == null)
                {
                    NotificationManager tmp = FindObjectOfType<NotificationManager>();
                    instance = tmp;
                }
                return instance;
            }
        }

        public bool CheckMinigameNotification()
        {
            if (instance == null)
            {
                Debug.Log(StringUtils.RedString("Instance of Notification Manager is null!"));
                return false;
            }

            if (instance.minigameNotif.activeSelf)
            {
                instance.minigameNotif.SetActive(false);
                return true;
            }
            Debug.Log("Notifs are not active! Check if notifications receives the activation notice!");
            return false;
        }

        public bool CheckItemNpcNotification()
        {

            if (instance.interactNotif.activeSelf)
            {
                instance.interactNotif.SetActive(false);
                return true;
            }

            Debug.Log("Notifs are not active! Check if notifications receives the activation notice!");
            return false;
        }
        #endregion

        public void Start()
        {
            EventBroadcaster.Instance.AddObserver(EventNames.NOTIFY_PLAYER_INTERACTION, NotifyPlayer);
            EventBroadcaster.Instance.AddObserver(EventNames.PLAYER_PICKUP_ITEM, PlayerPickUpItem);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.NOTIFY_PLAYER_INTERACTION, NotifyPlayer);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.PLAYER_PICKUP_ITEM, PlayerPickUpItem);
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
                NotifyNearOtherPlayer(tmp, p.GetWithKeyParameterValue<bool>("Entering", false));
            }
            else if(p.HasParameter("Item"))
            {
                ItemBase tmp = p.GetWithKeyParameterValue<ItemBase>("Item", null);
                NotifyNearItem(tmp, p.GetWithKeyParameterValue<bool>("Entering", false));
            }
            else if(p.HasParameter("Npc"))
            {
                BareboneNpcBase tmp = p.GetWithKeyParameterValue<BareboneNpcBase>("Npc", null);
                NotifyNearNPC(tmp, p.GetWithKeyParameterValue<bool>("Entering", false));
            }
        }


        public void NotifyNearOtherPlayer(BareboneCharacter thisCharacter  = null, bool isEntering = false)
        {

        }
        public void NotifyNearNPC(BareboneNpcBase thisCharacter = null, bool isEntering = false)
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
        
        public void NotifyNearItem(ItemBase thisItem = null, bool isEntering = false)
        {
            if(thisItem == null)
            {
                Debug.Log("Item Null!!");
                return;
            }
            if(isEntering)
            {
                itemNearby = thisItem;
            }
            else
            {
                if(itemNearby == thisItem)
                {
                    itemNearby = null;
                }
            }
            interactNotif.SetActive(isEntering);
        }

        public void PlayerPickUpItem(Parameters p = null)
        {
            playerStats.QuickPlaceToSlot(itemNearby);
        }
    }
}
