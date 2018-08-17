using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using EventFunctionSystem;
using Barebones;
using Barebones.Characters;

 namespace UserInterface
{
    public class BaseManager : MonoBehaviour {

        [Header("Managerial Stuff")]
        public GameObject player;
        public BareboneCharacter playerStats;
        public void Awake()
        {
            EventBroadcaster.Instance.AddObserver(EventNames.SET_UI_PLAYER_REFERENCE, SetPlayerReference);
        }

        public virtual void OnDestroy()
        {
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.SET_UI_PLAYER_REFERENCE, SetPlayerReference);
        }

        public virtual void SetPlayerReference(Parameters param = null)
        {
            if(param == null)
            {
                return;
            }
            if (!param.HasParameter("player"))
            {
                return;
            }

            player = param.GetWithKeyParameterValue<GameObject>("player", null);

            if(player != null)
            {
                playerStats = player.GetComponent<BareboneCharacter>();
            }
        }
    }
}
