using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barebones.Characters;

using EventSystem;
using Utilities;

namespace Barebones.Minigame
{
    public class MGColliderHelper : MonoBehaviour {

        [SerializeField] private BareboneCharacter playerInteracting;
        [SerializeField] private List<BareboneCharacter> playersWithinRange = new List<BareboneCharacter>();

        [Header("Switch")]
        [SerializeField] public bool effectsOn;

        [Header("Particle Effects")]
        [SerializeField] public ParticleSystem fx;
        // Parameter Default
        Parameters p = new Parameters();

        public void Start()
        {
            p.AddParameter<MGColliderHelper>("Checker", this);
            SwitchFX(0);
        }

        public void SwitchFX(int forceSwitch = -1)
        {
            if (forceSwitch == -1)
            {
                effectsOn = !effectsOn;
            }
            else
            {
                effectsOn = (forceSwitch == 1) ? true : false;
            }
            if(fx != null)
            {

            if (effectsOn)
                fx.Play();
            else
                fx.Stop();
            }
            else
            {
                Debug.Log(StringUtils.YellowString("Special Effects is Invalid! are you sure you added a Reference?"));
            }
            
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
                    BareboneCharacter player = collision.transform.GetComponent<BareboneCharacter>();
                    playersWithinRange.Add(collision.transform.GetComponent<BareboneCharacter>());

                    Parameters p = new Parameters();
                    p.AddParameter<MGColliderHelper>("Checker", this);
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
                p.AddParameter<MGColliderHelper>("Checker", this);
                EventBroadcaster.Instance.PostEvent(EventNames.NOTIFY_PLAYER_INTERACTION, p);

                if (playersWithinRange != null)
                {
                    if (!playersWithinRange.Contains(collidingPlayer))
                    {
                        return;
                    }
                    playersWithinRange.Remove(collidingPlayer);
                }
            }
        }
    }
}
