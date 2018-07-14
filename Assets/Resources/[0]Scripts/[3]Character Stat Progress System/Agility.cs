using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Barebones.Characters
{
    public class Agility : BareboneStats
    {
      
        public override void BuildStat()
        {
            base.BuildStat();
            statProgressType.Add(ActionType.DODGING);
        }
    }
}

