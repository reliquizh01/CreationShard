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
            p.AddParameter<bool>("Entering", false);
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
                    fx.gameObject.SetActive(true);
                else
                    fx.gameObject.SetActive(false);
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
                    SwitchFX(1);
                    Parameters p = new Parameters();
                    p.AddParameter<MGColliderHelper>("Checker", this);
                    p.UpdateParameter<bool>("Entering", true);
                    Debug.Log("Enter");
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
                p.UpdateParameter<bool>("Entering", false);
                if (playersWithinRange != null)
                {
                    if (!playersWithinRange.Contains(collidingPlayer))
                    {
                        return;
                    }
                    SwitchFX(0);
                    Debug.Log("Exit");
                    EventBroadcaster.Instance.PostEvent(EventNames.NOTIFY_PLAYER_INTERACTION, p);
                    playersWithinRange.Remove(collidingPlayer);
                }
            }
        }
    }
}
