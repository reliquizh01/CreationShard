using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
    [Serializable]
    public class StatsDictionary{

        public string _key;
        public BareboneStats bareboneStats;

        public virtual void Initialize(BareboneStats thisStats)
        {
            bareboneStats = thisStats;
            bareboneStats.Initialize();

            _key = bareboneStats.GetType().ToString();
        }
    }
}
