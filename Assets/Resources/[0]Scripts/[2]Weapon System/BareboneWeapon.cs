using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;
using Barebones.Characters;
/// <summary>
/// Barebone script for weapons, they can be used to
/// </summary>
namespace Barebones.DamageSystem
{
    public enum WeaponEffect
    {
        Normal = 0,
        Pushback = 1,
        Knockdown = 2,
        Stunned = 3,
    }
    public class BareboneWeapon : BareboneObject {

        // Weapon can deal damage if its activated
        public bool environmentWeapon;
        public bool active;
        public WeaponEffect weaponEffect;

        // Obtain Owner
        [SerializeField] private BareboneCharacter Owner;

        // Implemented as List to allow multiple types of damage.
        [SerializeField] private List<BareboneDamage> bareboneDamages;


        public void OnEnable()
        {
            if (Owner == null)
            {
                // This allows weapons to be carried as gameobjects
                BareboneCharacter parent = transform.root.GetComponent<BareboneCharacter>();
                if (parent == null)
                {
                    // if the weapon is active, it means its an damaging obstacle.
                    team = 10;
                }
                else
                {
                   Owner = parent;
                    team = Owner.Team;
                }
            }
            else
            {
                team = Owner.Team;
            }
            Initialize();
        }
        public void Initialize()
        {
            for (int i = 0; i < bareboneDamages.Count; i++)
            {
                bareboneDamages[i].Initialize();
            }
        }
        // this is used to Get one damageType
        public BareboneDamage GetOneBareboneDamage(Parameters param)
        {
            if (param == null) return null;

            if (bareboneDamages == null) return null;

            if (param.HasParameter("index"))
            {
                int index = param.GetWithKeyParameterValue<int>("index", bareboneDamages.Count);
                if(index == bareboneDamages.Count)
                {
                    return null;
                }
                return bareboneDamages[index];
            }

            return null;
        }
        
        public List<BareboneDamage> GetAllBareboneDamages
        {
            get
            {
                return this.bareboneDamages;
            }
        }

        public void AddBareboneDamage(BareboneDamage bareboneDamage)
        {
            if (bareboneDamage == null) return;

            bareboneDamages.Add(bareboneDamage);
        }

        // Collision to Check who's getting hit.
        public void OnTriggerEnter(Collider other)
        {
            BareboneObject receiver = other.transform.GetComponent<BareboneObject>();
            // if Weapon is not active
            if (!this.active)
            {
                return;
            }
            //Check if the hit item is an Object
            if(!receiver)
            {
                return;
            }
            if(receiver.Team != team)
            {
                // if Collision isLiving
                if (receiver.ObjectType == ObjectType.Living)
                {
                    Parameters parameters = new Parameters();
                    parameters.Initialize();
                    // Adds all the Damage the weapon can send
                    for (int i = 0; i < bareboneDamages.Count; i++)
                    {
                    BareboneDamage damage = new BareboneDamage();
                    parameters.AddParameter<BareboneDamage>("DamageType" + i,new BareboneDamage().Copy(bareboneDamages[0]));
                    }
                    // Checks if the weapon has an owner, if so, the owner is also passed as a parameter.
                    if(Owner != null)
                    {
                        if (Owner.GetComponent<BareboneCharacter>())
                        {
                            parameters.AddParameter<BareboneCharacter>("DamageDealer", Owner);
                        }
                    }
                    // Add Effects
                    receiver.ReceiveDamage(parameters);
                    active = false;
                }
            }
            if (environmentWeapon)
            {
                StartCoroutine(ResetActive());
            }
        }
        public bool IsDamageTypeMultiple()
        {
            if(bareboneDamages.Count > 1)
            {
                return true;
            }
            return false;
        }

        public IEnumerator ResetActive()
        {
            yield return new WaitForSeconds(5);
            active = true;
        }
    }
}
