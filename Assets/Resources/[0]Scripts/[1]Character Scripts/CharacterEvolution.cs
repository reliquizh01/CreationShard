using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
    [Serializable]
    public class CharacterEvolution {

        [SerializeField]private string evolutionName;
        [SerializeField] private GameObject[] evolutionParts;


        public string EvolutionName
        {
            get
            {
                return evolutionName;
            }
        }
        public GameObject[] EvolutionParts
        {
            get
            {
                return evolutionParts;
            }
        }
    }
}
