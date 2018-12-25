using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Barebones.Characters;
using Utilities;
using EventFunctionSystem;

namespace Barebones.Items
{
    public enum ItemType
    {
        CONSUMABLE = 0,
        EQUIPPABLE = 1,
        INGREDIENT = 2,
        QUEST = 3
    }

    public enum EquipType
    {
        Gauntlet = 0,
        Helmet = 1,
        Tool = 2,
        CoinPurse = 3,
        belt = 4,
    }

    public class ItemBase : BareboneObject {

        [Header("Item Movement")]
        public float yUpDownValue = 0.5f;
        public float rotateSpeed = 0.5f;
        public Vector3 upVector, downVector;
        private int floorLayer;
        [Header("Checker")]
        public bool floorHit = false;
        public bool effectsOn = false;
        public bool requiresHand = false;
        public bool pickedUp = false;
        [Header("Particle Effects")]
        [SerializeField] public ParticleSystem fx;

        [Header("Player Interaction")]
        public BareboneCharacter playerInteracting;
        [SerializeField] private List<BareboneCharacter> playersWithinRange = new List<BareboneCharacter>();

        [Header("Item Information")]
        public EquipType equipType;

        public void Start()
        {
            floorLayer = LayerMask.NameToLayer("Terrain");
        }
        public void OnEnable()
        {
            InitializeItem();
        }
        public void InitializeItem()
        {
            upVector = new Vector3(transform.position.x, transform.position.y + yUpDownValue, transform.position.z);
            downVector = new Vector3(transform.position.x, transform.position.y - yUpDownValue, transform.position.z);
        }

        public void Update()
        {
            ItemMovement();
        }

        public void ItemMovement()
        {
            if(pickedUp)
            {
                return;
            }
            if(!CheckFloatingPosition())
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 0.125f, transform.position.z), 5.5f * Time.deltaTime);
                return;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotateSpeed);
        }

        public void PickUpItem(BareboneCharacter picker)
        {
            
            if(!playersWithinRange.Contains(picker))
            {
                return;
            }
            playerInteracting = picker;
            pickedUp = true;
            /*
            if(requiresHand && !playerInteracting.HandsAvailable)
            {
                Debug.Log("Player has no hands, This item requires hands to be picked up!");
                playerInteracting = null;
                return;
            } */
        }

        public void DropItem()
        {
            gameObject.transform.parent = null;
            pickedUp = false;
            playerInteracting = null;
        }

        public void OnDrawGizmos()
        {
            Vector3 downward = transform.TransformDirection(Vector3.forward * 0.35f);
            Gizmos.DrawRay(transform.position, downward);

        }
        public bool CheckFloatingPosition()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward * 0.05f), out hit))
            {
                if(hit.transform.gameObject.layer == floorLayer)
                {
                    if(hit.distance < 0.35f)
                    {
                        //Debug.Log("I hit the floor!");
                        floorHit = true;
                    }
                }
                else
                {
                    floorHit = false;
                }
            }

            return floorHit;
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
                    SwitchFX(1);
                    Parameters p = new Parameters();
                    p.AddParameter<ItemBase>("Item", this);
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
                p.AddParameter<ItemBase>("Item", this);
                p.UpdateParameter<bool>("Entering", false);
                if (playersWithinRange != null)
                {
                    if (!playersWithinRange.Contains(collidingPlayer))
                    {
                        return;
                    }
                    SwitchFX(0);
                    //Debug.Log("Exit");
                    EventBroadcaster.Instance.PostEvent(EventNames.NOTIFY_PLAYER_INTERACTION, p);
                    playersWithinRange.Remove(collidingPlayer);
                }
            }
        }

        /// <summary>
        /// Special FX is the Particle system showing (green particle) when item is dropped
        /// </summary>
        /// <param name="forceSwitch"></param>
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
            if (fx != null)
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
    }
}
