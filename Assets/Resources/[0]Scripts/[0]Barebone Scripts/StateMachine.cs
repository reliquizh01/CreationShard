using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


namespace CharacterStateMachine
{

    public class CharacterStateHandler
    {
        /// <summary>
        /// String Key is the 
        /// </summary>
        public static Dictionary<string, CharacterState> characterState = new Dictionary<string, CharacterState>();

        public static Dictionary<string, List<string>> allowedTransitions = new Dictionary<string, List<string>>();

        /// <summary>
        ///  By using this function, you're allowing one state to another to have a transition, ensuring no State going anywhere else.
        /// </summary>
        /// <param name="fromState"> Coming from State</param>
        /// <param name="toState"> Going To State</param>
        public static void CreateTransition(string fromState, string toState)
        {
            if (allowedTransitions.ContainsKey(fromState))
            {
                allowedTransitions[fromState].Add(toState);
            }
            allowedTransitions.Add(fromState, new List<string>());
            allowedTransitions[fromState].Add(toState);
        }
        public static bool IsTransitionAllowed(string currentStateKey, string toStateKey)
        {
            if (allowedTransitions.ContainsKey(currentStateKey))
            {
                if(allowedTransitions[currentStateKey].Contains(toStateKey))
                {
                    return true;
                }
                else
                {

                    Debug.Log(StringUtils.YellowString("Origin StateKey is not allowed to go to [" + toStateKey + " check if you added it correctly!"));
                    return false;
                }
            }
            Debug.Log(StringUtils.YellowString("Transition does not contain the Origin StateKey : You might've forgot to add them initially!"));
            return false;
        }
       public void AddState(string stateKey, CharacterState thisState)
        {
            if(characterState.ContainsKey(stateKey))
            {
                Debug.Log(StringUtils.YellowString("Trying to Add [" + stateKey + " to dictionary but it already contains the said key"));
                return;
            }
        }
    }
    /// <summary>
    ///  Statemachine handles all the allowed transitions for the characters
    /// </summary>
    public static class StateMachine{
       
        public static Dictionary<string, CharacterStateHandler> CharacterStateTransitions = new Dictionary<string, CharacterStateHandler>();

        /// <summary>
        /// Used to add a state to a certain Character, ensuring a centralize output of Statemachine
        /// </summary>
        /// <param name="StateName"> Requires a Seperator [CharacterName]_[StateToBeAdded]</param>
        /// <param name="thisState"></param>
        public static void CreateStateDictionary(string StateName, CharacterState thisState)
        {
            string[] split = new string[StateName.Split('_').Length];
            if(split.Length < 1)
            {
                Debug.Log(StringUtils.RedString("Trying to Add [" + StateName + "] to CharacterState with NO '_'  seperator"));
                return;
            }
            string characterName = split[0];
            string characterStateKey = split[1];

            if(CharacterStateTransitions.ContainsKey(characterName)){
                CharacterStateTransitions[characterName].AddState(characterStateKey, thisState);
            }
        }
    }
}
