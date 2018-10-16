using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStateMachine;
using Barebones.Characters;

using Utilities;
using Barebones;
/// <summary>
/// Manages all Animation related to an Object
/// </summary>
[Serializable]
public class AnimationManager : BareboneObject {
    
    [Header("ANIMATIONS")]
    [SerializeField] protected CharacterStates currentState;
    private CharacterState[] characterStates = new CharacterState[(int)CharacterStates.NumberofTypes];
    [SerializeField] private bool multipleIdleState;
    private float idleTimer;
    private float idleIndex = 0;

    [Header("Evolution Animations")]
    [SerializeField] protected List<Animator> currentAnimators;
    [SerializeField] protected int animatorIndex;
    
    public CharacterStates CurrentState
    {
        get
        {
            return this.currentState;
        }
        set
        {
            this.currentState = value;
        }
    }
    
    public virtual  void Initialize()
    {
        if(multipleIdleState)
        {
            Parameters p = new Parameters();
            p.AddParameter<bool>("MultiIdle", true);
            UpdateAnimator("MultiIdle", p);
        }
    }
    public virtual void Update()
    {

    }

    public virtual void ToEvolve()
    {

    }

    public virtual void UpdateAnimator(string parameterName, Parameters p = null)
    {
        if (p == null)
        {
            Debug.Log(StringUtils.YellowString("Animators are Being Updated without sufficient Parameters! Are you sure about this?"));
        }
        List<AnimatorControllerParameter[]> animatorParam;
        animatorParam = SetAnimationParam(currentAnimators);
        

        for (int i = 0; i < animatorParam.Count; i++)
        {
            for(int x = 0; x < animatorParam[i].Length; x++)
            {
                if (animatorParam[i][x].name == parameterName)
                {
                    if (animatorParam[i][x].type == AnimatorControllerParameterType.Bool)
                    {
                         currentAnimators[i].SetBool(parameterName, p.GetWithKeyParameterValue<bool>(parameterName, false));
                    }
                    else if (animatorParam[i][x].type == AnimatorControllerParameterType.Float)
                    {
                        currentAnimators[i].SetFloat(parameterName, p.GetWithKeyParameterValue<float>(parameterName, 0));
                    }
                    else if (animatorParam[i][x].type == AnimatorControllerParameterType.Int)
                    {
                        currentAnimators[i].SetInteger(parameterName, p.GetWithKeyParameterValue<int>(parameterName, 0));
                    }
                    else if(animatorParam[i][x].type == AnimatorControllerParameterType.Trigger)
                    {
                        currentAnimators[i].SetTrigger(parameterName);
                    }
                }
            }
        }
    }
    //SET NEW LIST OF ANIMATORS
    private List<AnimatorControllerParameter[]> SetAnimationParam(List<Animator> animators)
    {
        List<AnimatorControllerParameter[]> tmpAnimatorParam = new List<AnimatorControllerParameter[]>();
        for (int i = 0; i < animators.Count; i++)
        {
            // Create a Temporary Parameter
            AnimatorControllerParameter[] tmpParams = new AnimatorControllerParameter[animators[i].parameters.Length];
            tmpParams = animators[i].parameters;

            // Add them to a List of Parameters
            tmpAnimatorParam.Add(tmpParams);
        }
        return tmpAnimatorParam;
    }


    public virtual void SetCurrentAnimators(GameObject[] objectAnimators)
    {
        if(objectAnimators == null)
        {
            Debug.Log("Animator for Evolution is invalid! Check your List!");
            return;
        }
        currentAnimators.Clear();

        for (int i = 0; i < objectAnimators.Length; i++)
        {
            currentAnimators.Add(objectAnimators[i].GetComponent<Animator>());
        }

    }

    public virtual void ShowCurrentEvolution()
    {

    }
    public virtual void UpdateCurrentState(CharacterStates newState,  Parameters p = null)
    {
        currentState = newState;

        string[] keyNames = p.GetAllKeys();

        for (int i = 0; i < keyNames.Length; i++)
        {
            UpdateAnimator(keyNames[i], p);
        }
    }
}
