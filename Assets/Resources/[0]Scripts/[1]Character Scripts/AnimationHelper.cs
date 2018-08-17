using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones;
using Barebones.Characters;
[RequireComponent(typeof(Animator))]
public class AnimationHelper : MonoBehaviour {

    [SerializeField] private BareboneObject parent;
    [SerializeField] private bool isMainBody;
    [SerializeField] private bool isHand;
    public void Start()
    {
        parent = this.transform.parent.GetComponent<BareboneObject>();
    }
    
    public void Evolve()
    {
        parent.Evolve();
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
}
