using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterStateMachine
{
    public enum CharacterStates
    {
        _IDLE = 0,
        _MOVING = 1,
        _JUMPING = 2,
        _STUNNED = 3,
        _DEAD = 4,
        _MINIGAME = 5,
        _INTERACTING = 6,
        _EVOLVING = 7,

        NumberofTypes
    };

    public class CharacterState{

        public string stateKey;

        public virtual void SetStateKey(string creatureName)
        {
            stateKey = creatureName + stateKey;
        }
        /// <summary>
        /// OnEnter > Play Animation
        /// </summary>
        public virtual void OnEnter()
        {

        }

        /// <summary>
        /// After Playing Animation > Exit > Play Next State if Possible > if Not return to Previous State.
        /// </summary>
        public virtual void OnExit()
        {
        
        }
    

        public static void NextState(string StateName)
        {

        }
    }
}
