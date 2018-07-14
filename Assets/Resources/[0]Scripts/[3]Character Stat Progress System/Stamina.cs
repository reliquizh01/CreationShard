using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
    public class Stamina : BareboneStats
    {
        public override void BuildStat()
        {
            base.BuildStat();
            statProgressType.Add(ActionType.RUNNING);
            statProgressType.Add(ActionType.DODGING);
        }
    }
}

