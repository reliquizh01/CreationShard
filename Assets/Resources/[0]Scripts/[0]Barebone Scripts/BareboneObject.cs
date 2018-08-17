using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones.Skill;

/// <summary>
/// BarebonesObject -
/// Used for all objects within the game world for generic information and actions.
/// 
/// - Emman Guardame
/// </summary>
namespace Barebones
{
    public enum ObjectType
    {
        Destructible = 0,
        Living = 1,
        Player = 2,
        Ingredient = 3,
        Item = 4,
    };
    public class BareboneObject : MonoBehaviour
    {
        [Header("GENERAL INFORMATION")]
        [Header("Global Name")]
        [SerializeField] private string itemId;

        [Header("Local Reference")]
        [SerializeField]private string generalName;
        [SerializeField] private ObjectType objectType;
        // Toggles
        [SerializeField] private bool canTakeDamage;
        // Allegiance
        [SerializeField] protected int team;
        [SerializeField] protected bool isInteractable;

        [SerializeField] protected List<BaseSkill> skills;
        [SerializeField] protected bool skillActivated;

        // Alive Stats
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float maximumHealth;
        [SerializeField] protected float currentStamina;
        [SerializeField] protected float maximumStamina;
        [SerializeField] protected float weight;

        public virtual void Awake()
        {
            InitializeSkill();
        }

        public float CurrentHealth
        {
            get
            {
                return this.currentHealth;
            }
            set
            {
                currentHealth = Mathf.Clamp(value, 0, maximumHealth);
            }
        }
        public float MaximumHealth
        {
            get
            {
                return this.maximumHealth;
            }
            set
            {
                maximumHealth = value;
            }
        }
        public float CurrentStamina
        {
            get
            {
                return this.currentStamina;
            }
            set
            {
                currentStamina = Mathf.Clamp(value, 0, maximumStamina);
            }
        }
        public float MaximumStamina
        {
            get
            {
                return this.maximumStamina;
            }
            set
            {
                maximumStamina = value;
            }
        }
        public bool CanTakeDamage
        {
            get
            {
                return this.canTakeDamage;
            }
            set
            {
                this.canTakeDamage = value;
            }
        }
        public bool SkillActivate
        {
            get
            {
                return this.skillActivated;
            }
            set
            {
                this.skillActivated = value;
            }
        }
        public string GeneralName
        {
            get
            {
                return generalName;
            }
        }
        public void ChangeName(string newName)
        {
            name = newName;
        }
        public ObjectType ObjectType
        {
            get
            {
                return objectType;
            }
        }
        public int Team
        {
            get
            {
                return this.team;
            }
            set
            {
                team = value;
            }
        }
        public virtual void InteractWithObject()
        {

        }
        public virtual void ReceiveDamage(Parameters param = null)
        {

        }
        public virtual void Evolve()
        {
            Debug.Log("Evolving!!!");
        }
        public virtual void FinishEvolve(Animator selfAnim = null)
        {

        }
        
        public virtual void InitializeSkill(bool allSkill = true, int skillIndex = 0)
        {
            if(allSkill)
            {
                foreach(BaseSkill skill in skills)
                {
                    skill.SetOwner(this);
                }
            }
        }

        public virtual void MoveTowards(string direction, bool forceFace = false)
        {
            // failsafe
            if (!GetComponent<Rigidbody>())
            {
                Debug.Log(Utilities.StringUtils.YellowString("Forcing Object to Move, but object has no Rigidbody."));
                return;
            }
            Debug.Log(direction);
            Rigidbody rb;
            rb = GetComponent<Rigidbody>();

            Vector3 velocity = Vector3.zero;
            if(direction == "forward")
            {
                velocity.z = 10;
            }
            else if (direction == "backward")
            {
                velocity.z -= 10;
            }
            else if (direction == "right")
            {
                velocity.x = 10;
            }
            else if (direction == "left")
            {
                velocity.x -= 10;
            }
            rb.velocity = transform.TransformDirection(velocity);
        }

        public virtual void PlayAnimation(string key)
        {
            string animKey = generalName + "_" + key;
           
        }
    }
}
