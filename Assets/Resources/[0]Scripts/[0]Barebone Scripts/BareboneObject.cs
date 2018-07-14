using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

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
    public class BareboneObject : AnimationManager
    {
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
    }
}
