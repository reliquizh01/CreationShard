using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones;
using Barebones.Characters;
using EventFunctionSystem;
using Utilities;

[RequireComponent(typeof(Animator))]
public class AnimationHelper : MonoBehaviour {

    [SerializeField] private AnimationManager parent;
    [SerializeField] private bool isMainBody;
    [SerializeField] private bool isHand;
    [SerializeField] private List<BaseSlot> naturalSlots;
    public List<BaseSlot> GetNaturalSlots
    {
        get
        {
            return this.naturalSlots;
        }
    }
    public void Start()
    {
        parent = this.transform.parent.GetComponent<AnimationManager>();
    }
    
    public void Evolve()
    {
        parent.Evolve();
    }

    public void ToEvolve()
    {
        parent.ToEvolve();
    }
    public void FinishEvolve()
    {
          Debug.Log("Evolve Finish : " + this.gameObject.name);
          parent.FinishEvolve(this.GetComponent<Animator>());
        if(isHand)
        {
            if(parent.GetComponent<BareboneCharacter>())
            {
                parent.GetComponent<BareboneCharacter>().HandsAvailable = true;
            }
        }
    }
    public void FinishPickUp()
    {
        Debug.Log("Pick Up Finish!");
        parent.CurrentState = CharacterStateMachine.CharacterStates._IDLE;
        EventBroadcaster.Instance.PostEvent(EventNames.PLAYER_PICKUP_ITEM);
    }

}
