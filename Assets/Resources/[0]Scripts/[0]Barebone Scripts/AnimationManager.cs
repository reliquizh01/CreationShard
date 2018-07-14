using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStateMachine;
using Barebones.Characters;

using Utilities;
/// <summary>
/// Manages all Animation related to an Object
/// </summary>
[Serializable]
public class AnimationManager : MonoBehaviour {
    
    [SerializeField] protected CharacterStates currentState;
    private CharacterState[] characterStates = new CharacterState[(int)CharacterStates.NumberofTypes];

    [Header("Evolution Animations")]
    [SerializeField] protected List<Animator> currentAnimators;
    [SerializeField] protected int animatorIndex;
    
    public virtual  void Initialize()
    {

    }

    public virtual void UpdateAnimator(string parameterName, Parameters p = null)
    {
        if (p == null)
        {
            return;
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
        for (int i = 0; i < objectAnimators.Length; i++)
        {
            currentAnimators.Add(objectAnimators[i].GetComponent<Animator>());
        }

    }
}
