using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
        public enum LivingState
        {
            IDLE = 0,
            MOVING = 1, // Walking, Sneaking, Running, jumping (Non-combat)
            INTERACTING = 2, // WoodCutting, 
            COMBAT = 3, // Weapon Behaviour-based
            DISABLED = 4, // Stunned, Knocked-out, Rooted
            DEAD = 5,
            CAUTIOUS = 6,
        };

        public enum ActionType
        {
        // FUTURE IMPROVEMENT : COMBINATION OF 2 ACTIONTYPES THAT COULD IMPROVE STATS.
            IDLE = 0,
            WALKING = 1,
            PATROL = 2,
            RUNNING = 3, // Stamina
            DODGING = 4, // Agility, Stamina
            LIFTING = 5, // Strength 
            WOODCUTTING = 6,//WoodCutting
            ATTACKING = 7, // Weapon-Based
            ATTACKED = 8,  // Resistance
            BLOCKING = 9, // Defense, Stamina
            STUNNED = 10,  // Disables Player
            SHEATHED = 11, // When Weapon is not toggled
            OBSERVING = 12, // Aware of Everything
            SLEEPING = 13, // All Abilities to detect is gone.
        };
}
