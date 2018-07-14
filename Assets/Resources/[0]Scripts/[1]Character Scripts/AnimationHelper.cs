using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Barebones;

[RequireComponent(typeof(Animator))]
public class AnimationHelper : MonoBehaviour {

    [SerializeField] private BareboneObject parent;
    [SerializeField] private bool isMainBody;
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
        if (isMainBody)
        {
            Debug.Log("Evolve Finish : " + this.gameObject.name);
            parent.FinishEvolve(this.GetComponent<Animator>());
        }
    }
}
