using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Barebones.Characters
{
    [System.Serializable]
    public class Strength : BareboneStats
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void BuildStat()
        {
            base.BuildStat();
            statProgressType.Add(ActionType.LIFTING);
        }
    }
}

