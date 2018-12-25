using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventFunctionSystem;
using Utilities;
using Barebones.Characters;
using Barebones.DamageSystem;
using Barebones.Items;

namespace Barebones.Characters
{
    /// <summary>
    ///  Used by all living Creatures in the world, contains  generic variables and functions that affects the living character.
    ///  
    /// - Emman Guardame
    /// </summary>
    public enum Buff
    {
        SLEEPY = 0,         // Not enough Sleep
        HUNGRY = 1,     // Food Meter touched the lowest 
        POISONED = 2, // Infected by some source of virus.
    }
    [Serializable]
    public class BareboneCharacter : AnimationManager
    {
        [Header("CHARACTER INFORMATION")]
        [SerializeField] private string characterName;
        // Interaction Related
        [SerializeField] private GameObject targetObject;

        public GameObject TargetObject
        {
            get
            {
                return this.targetObject;
            }
            set
            {
                this.targetObject = value;
                Parameters param = new Parameters();
                if(targetObject != null)
                {
                     param.AddParameter<Transform>("FocusTo", targetObject.transform);
            
                    EventBroadcaster.Instance.PostEvent(EventNames.CAMERA_CHANGE_FOCUS, param);
                }
            }
        }

        //toggle
        [SerializeField] private bool playerControlled;
        [SerializeField] protected bool isGrounded;
        [SerializeField] protected bool canGrow;
        [SerializeField] protected bool hasHands;
        [SerializeField] protected bool isEvolving;
        [SerializeField] protected List<BaseSlot> itemsWithSlots;
        //Evolution GameObjects
        [SerializeField] private bool hasEvolution = true;
        [SerializeField] protected int currentEvolution;
        [SerializeField] private CharacterEvolution[] evolutions;

        //Character Stats
        [SerializeField] private bool StatsProgression;
        [SerializeField] private List<BareboneStats> characterStats;
        [SerializeField] private Resistance resistance;
        
        [Header("Action Tab")]
        [SerializeField] protected ActionType currentAction;
        [SerializeField] private ActionType prevAction;
        [Header("Living State")]
        [SerializeField] protected LivingState currentLivingState;
        
        // Status
        [SerializeField] private List<Buff> buff;

        // Holds all Damage Overtime
        [SerializeField]private List<BareboneDamage> dotDamageDealers;
        
        // Movement
        [SerializeField] protected float movementSpeed;
        protected Quaternion targetRotation;
        
        public bool HandsAvailable
        {
            get
            {
                return this.hasHands;
            }
            set
            {
                this.hasHands = value;
            }
        }

        public bool IsEvolving
        {
            get
            {
                return isEvolving;
            }
        }
        public string CharacterName
        {
            get
            {
                return this.characterName;
            }
        }
        public Quaternion TargetRotation
        {
            get
            {
                return this.targetRotation;
            }
        }
        public LivingState LivingState
        {
            get
            {
                return this.currentLivingState;
            }
            set
            {
                this.currentLivingState = value;
            }
        }
        public List<BareboneStats> CharacterStats
        {
            get
            {
                return this.characterStats;
            }
        }
        public ActionType CurrentAction
        {
           get
            {
                return currentAction;
            }
        }
        public ActionType PreviousAction
        {
            get
            {
                return prevAction;
            }
            set
            {
                prevAction = value;
            }
        }
        public bool PlayerControlled
        {
            get
            {
                return this.playerControlled;
            }
            set
            {
                this.playerControlled = value;
            }
        }


        public override void Awake()
        {
            base.Awake();
            // facingDirection needs Improvement, if there's an AI that should be facing on a fixed direction upon contact
            targetRotation = this.transform.rotation;
            //characterController = GetComponent<CharacterController>();
            // Check if there's no Save File
            Initialize();
          //  stateMachine.SetCharacter(this);
        }

        /// <summary>
        /// Function to correct all wrong count (I.E : Minimum health > Max health)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            characterStats = new List<BareboneStats>();
            dotDamageDealers = new List<BareboneDamage>();

            // CHeck if health is the same as Maximumhealth (due to change upon the introduction of save/load system
            if (currentHealth > maximumHealth)
            {
                currentHealth = maximumHealth;
            }
            // If there's no Set Stats, will CreateGeneric Stats
            if(characterStats.Count == 0)
            {
                //Debug.Log("QQQQ : INITIALIZING STATS of " + name);
                CreateGenericCharacterStats();
            }

            //Set Current Evolution as Active
            if (hasEvolution)
            {
                ShowCurrentEvolution();
                //Set CurrentAnimators
                SetCurrentAnimators(evolutions[currentEvolution].EvolutionParts);
                //Obtain current ItemSlots
                InitializeGenericSlots(evolutions[currentEvolution].EvolutionParts);
            }
        }

        public override void Update()
        {
            base.Update();
            if(dotDamageDealers.Count > 0)
            {
                DoTChecker(Time.deltaTime);
            }
        }

        public override void ReceiveDamage(Parameters param)
        {
            //Debug.Log("QQQQQQQ : RECEIVING DAMAGE");
            // Check if Player can take Damage
            if (ObjectType == ObjectType.Living)
            {
                if (this.CanTakeDamage)
                {
                    ChangeAction(ActionType.ATTACKED);
                    // Now Check if its Single Damage or Multiple Damage
                    if (param.GetParameterCount > 1)
                    {
                        Debug.Log("QQQ : Multiple Damage!");
                        ReceiveMultipleDamage(param);
                    }
                    else
                    {
                        //Debug.Log("QQQ : Single Damage!");
                        ReceiveSingleDamage(param);
                    }
                }
            }
        }

        /// <summary>
        /// Single Damage Operates like:
        /// 1 Attack : 1 Damage Dealt [Normal]
        ///  CharacterA hits CharacterB with Slash (Normal damage)
        /// </summary>
        /// <param name="param"></param>
        public void ReceiveSingleDamage(Parameters param)
        {
            if (param == null) return;

            //Get The only value the param has
            BareboneDamage singleDamage = new BareboneDamage();
            if (param.HasParameter("DamageType0"))
            {
                singleDamage = param.GetWithKeyParameterValue<BareboneDamage>("DamageType0", new BareboneDamage());
                Debug.Log(singleDamage.Name);
            }
            else
            {
                Debug.Log("DamageTypes are unavailable!");
            }
            // Characters are Optional, since there can be obstruction that can deal damage
            BareboneCharacter dealer;
            if (param.HasParameter("DamageDealer"))
            {
                dealer = param.GetWithKeyParameterValue<BareboneCharacter>("DamageDealer", new BareboneCharacter());
                Debug.Log("Owner is : " + dealer.name);
                CalculateDamage(singleDamage, dealer);
            }
            else
            {
                //Debug.Log("Calculating Damage! Min Dmg: " + singleDamage.MinimumDamage + " Max Dmg: " + singleDamage.MaximumDamage);
                CalculateDamage(singleDamage);
            }
        }
        /// <summary>
        /// MultipleDamage Operates like:
        /// 1 Attack : 5 Damage Dealt [Normal / Fire / Ice / Water / Earth) 
        /// CharacterA hits CharacterB with ElementalSlash [Fire,Wind,Ice]
        /// </summary>
        /// <param name="param"></param>
        public void ReceiveMultipleDamage(Parameters param = null)
        {

        }

        /// <summary>
        /// DoT Checker handles all damage overtime
        /// </summary>
        public void DoTChecker(float deltaTime)
        {
            // DAMAGE OCCURS PER TICK
            /*
                for (int i = 0; i < dotDamageDealers.Count; i++)
                {
                    if(dotDamageDealers[i].DotTimer >= dotDamageDealers[i].TickCount)
                    {
                        this.CurrentHealth -= dotDamageDealers[i].Overtimedamage;
                        dotDamageDealers[i].DotTimer = 0;
                        dotDamageDealers[i].TickCount -= 1;
                    }
                    if (dotDamageDealers[i].TickCount > 0)
                    {
                        dotDamageDealers[i].DotTimer += deltaTime;
                    }
                    else
                    {
                        dotDamageDealers.RemoveAt(i);
                    }
                        EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_HEALTH);
                }
                */

            // DAMAGE OCCURS OVERTIME
            for (int i = 0; i < dotDamageDealers.Count; i++)
            {
                if(dotDamageDealers[i].DotTimer < dotDamageDealers[i].TickCount)
                {
                    this.CurrentHealth -= dotDamageDealers[i].Overtimedamage;
                    dotDamageDealers[i].DotTimer += deltaTime;
                }
                // if dot is past the tickCount
                if (dotDamageDealers[i].DotTimer > dotDamageDealers[i].TickCount)
                {
                    dotDamageDealers.RemoveAt(i);
                }
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_HEALTH);
            }
        }
        /// <summary>
        /// CalculateDamage computes damage with the cooperation of characterStats
        /// </summary>
        public void CalculateDamage(BareboneDamage bareboneDamage, BareboneCharacter Damagedealer = null)
        {
            if (bareboneDamage.DamageTo == DamageTo.health)
            {
                float dotExp = 0;
                float randDmg = UnityEngine.Random.Range(bareboneDamage.MinimumDamage, bareboneDamage.MaximumDamage);
                float resistanceReduction = 0;
                if (resistance.GetResistance(bareboneDamage.GetDamageType.ToString()) != null)
                {
                    resistanceReduction = randDmg * 0.02f;
                }
                this.CurrentHealth -= randDmg - resistanceReduction;
                if (bareboneDamage.GetDamageProcess == BareboneDamage.Process.Overtime)
                {
                    dotDamageDealers.Add(bareboneDamage);
                    for (int i = 0; i < bareboneDamage.TickCount; i++)
                    {
                        dotExp += bareboneDamage.Overtimedamage;
                    }
                }
                Debug.Log("Improving Resistance!!");
                resistance.Addprogress(randDmg, bareboneDamage.GetDamageType.ToString());
                resistance.Addprogress(dotExp, bareboneDamage.GetDamageType.ToString());
                // Updates UI Health
                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_HEALTH);
            }
            else if(bareboneDamage.DamageTo == DamageTo.stamina)
            {
                float randDmg = UnityEngine.Random.Range(bareboneDamage.MinimumDamage, bareboneDamage.MaximumDamage);
                this.CurrentStamina -= randDmg;

                //TODO : Add Stamina Damage Overtime

                EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_PLAYER_STAMINA);
            }
        }

        // MOVEMENTS
        public virtual void Movement(float time)
        {
            // SAFETY NETS
            // Character Cannot move when stunned
            if(currentAction == ActionType.STUNNED || currentAction == ActionType.SLEEPING || currentLivingState == LivingState.DEAD)
            {
                return;
            }
        }
        public void ChangeAction(ActionType newAction)
        {
            if(currentAction == newAction)
            {
                Debug.Log(StringUtils.YellowString("Action is trying to change to its present state!"));
                return;
            }


            currentAction = newAction;
            UpdateStats();
        }

        public void UpdateStats()
        {
            foreach(BareboneStats stat in characterStats)
            {
                if(stat.GetProgressType.Contains(currentAction))
                {
                    stat.Addprogress(15);
                }
            }
        }
        public virtual void Combat()
        {
            if(currentLivingState == LivingState.DEAD)
            {
                return;
            }
        }

        /// <summary>
        /// Creates typical stats a character would have (Agility / Strength / Stamina)
        /// Manually Add them to the list of character stats
        /// </summary>
        public virtual void CreateGenericCharacterStats()
        {
            Strength strength = new Strength();
            strength.Initialize();
            characterStats.Add(strength);
            Agility agility = new Agility();
            agility.Initialize();
            characterStats.Add(agility);
            Stamina stamina = new Stamina();
            stamina.Initialize();
            characterStats.Add(stamina);

            resistance = new Resistance();
            resistance.Initialize();
        }

        public void DropAllItems()
        {
            foreach(BaseSlot itemSlot in itemsWithSlots)
            {
                itemSlot.DropCurrentItem();
            }
        }

        public override void ToEvolve()
        {
            DropAllItems();
            ChangeAction(ActionType.IDLE);
            isEvolving = true;
        }
        public override void Evolve()
        {
            // DISABLE ALL CURRENT EVOLUTION
            for (int i = 0; i < evolutions[currentEvolution].EvolutionParts.Length; i++)
            {
                evolutions[currentEvolution].EvolutionParts[i].SetActive(false);
            }

            // ENABLE NEW EVOLUTION
            currentEvolution += 1;

            for (int i = 0; i < evolutions[currentEvolution].EvolutionParts.Length; i++)
            {
                evolutions[currentEvolution].EvolutionParts[i].SetActive(true);
            }
            // SET THEM AS NEW ANIMATORS
            SetCurrentAnimators(evolutions[currentEvolution].EvolutionParts);

            //  stateMachine.UpdateAnimator();
            // Starts  Growth
            Parameters param = new Parameters();
            param.AddParameter<bool>("FinishGrowing", false);
            // TODO: INSERT ANIMATION CALLER HERE for the FINISH GROWTH
            UpdateAnimator("FinishGrowing", param);
        }

        public override void FinishEvolve(Animator selfAnim = null)
        {
            //Debug.Log("Evolve Finish : " + this.gameObject.name);
            // Finishes Growth
            Parameters param = new Parameters();
            param.AddParameter<bool>("FinishGrowing", true);
             //stateMachine.SetAnimatorCondition(param);
             if(selfAnim != null)
            {
                UpdateAnimator("FinishGrowing", param);
            }
            else
            {
                UpdateAnimator("FinishGrowing", param);
            }

            isEvolving = false;
            // Re-establish the natural Slots of the new formed body after evolution.
            InitializeGenericSlots(evolutions[currentEvolution].EvolutionParts);
        }

        // NPC INTERACTION
        #region NPC_INTERACTION

        #endregion
        ///   ITEM SLOTS
        #region ITEM_SLOTS
        public void AddStats(BareboneStats stats)
        {
            if(stats == characterStats.Find(x => x == stats))
            {
                Debug.Log(StringUtils.RedString("Player already has the said stats!"));
                return;
            }
            characterStats.Add(stats);
        }
         
        public bool CheckAllSlotWithItems()
        {
            if(itemsWithSlots != null)
            {
                if (itemsWithSlots.Contains(itemsWithSlots.Find(x => x.IsSlotOccupied == true)))
                {
                    return true;
                }
                else return false;
            }

            return false;
        }

        public void PickupItem(ItemBase thisItem)
        {
            UpdateAnimator("PickUp");
            if (!thisItem)
            {
                Debug.Log("Item trying to pick up has no ItemBase script!");
            }

            QuickPlaceToSlot(thisItem);

        }
        public void InitializeGenericSlots(GameObject[] bodyPartsWithSlots)
        {
            itemsWithSlots.Clear();

            foreach(GameObject bodyPart in bodyPartsWithSlots)
            {
                AnimationHelper partHelper = bodyPart.GetComponent<AnimationHelper>();
                if (partHelper)
                {
                    if(partHelper.GetNaturalSlots != null)
                    {
                        foreach(BaseSlot natSlot in partHelper.GetNaturalSlots)
                        {
                            itemsWithSlots.Add(natSlot);
                        }
                    }
                }
            }
        }

        public void QuickPlaceToSlot (ItemBase thisItem)
        {
            
            if (itemsWithSlots == null)
            {
                return;
            }

            foreach (BaseSlot slot in itemsWithSlots)
            {
                if (slot.currentItem != null)
                {
                    continue;
                }
                if (slot.holdItemType.Contains(thisItem.equipType))
                {
                    slot.PlaceItemToSlot(thisItem);
                }
            }
        }
        #endregion

        /// EVOLUTIONS
        #region EVOLUTIONS
        public override void ShowCurrentEvolution()
        {

            for (int i = 0; i <  evolutions.Length; i++)
            {
                if(i != currentEvolution)
                {
                    foreach(GameObject evoPart in evolutions[i].EvolutionParts)
                    {
                        evoPart.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject evoPart in evolutions[i].EvolutionParts)
                    {
                        evoPart.SetActive(true);
                    }
                }
            }
        }
        #endregion
    }
}
