using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Barebones;
using Barebones.Characters;
using Utilities;
using EventFunctionSystem;

public class BareboneNpcBase : BareboneObject
{
    [Header("Player Interaction")]
    public BareboneCharacter playerInteracting;
    [SerializeField] private List<BareboneCharacter> playersWithinRange = new List<BareboneCharacter>();
    


    public void StartPlayerConversation(BareboneCharacter thisPlayer)
    {
       
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (!collision.isTrigger)
        {

            if (playerInteracting != null)
            {
                return;
            }

            if (collision.transform.GetComponent<BareboneCharacter>())
            {
                //Debug.Log("Player Nearby!");
                BareboneCharacter player = collision.transform.GetComponent<BareboneCharacter>();
                playersWithinRange.Add(collision.transform.GetComponent<BareboneCharacter>());
                
                Parameters p = new Parameters();
                p.AddParameter<BareboneNpcBase>("Npc", this);
                p.UpdateParameter<bool>("Entering", true);
                // Debug.Log("Enter");
                EventBroadcaster.Instance.PostEvent(EventNames.NOTIFY_PLAYER_INTERACTION, p);
            }
        }
    }
    public void OnTriggerExit(Collider collision)
    {
        if (!collision.isTrigger)
        {

            BareboneCharacter collidingPlayer = (collision.transform.GetComponent<BareboneCharacter>()) ? collision.transform.GetComponent<BareboneCharacter>() : null;

            if (collidingPlayer == null)
            {
                return;
            }

            Parameters p = new Parameters();
            p.AddParameter<BareboneNpcBase>("Npc", this);
            p.UpdateParameter<bool>("Entering", false);
            if (playersWithinRange != null)
            {
                if (!playersWithinRange.Contains(collidingPlayer))
                {
                    return;
                }
                //Debug.Log("Exit");
                EventBroadcaster.Instance.PostEvent(EventNames.NOTIFY_PLAYER_INTERACTION, p);
                playersWithinRange.Remove(collidingPlayer);
            }
        }
    }
}

