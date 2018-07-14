using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Characters
{
    public class WoodCutting : BareboneStats {

        public override void Initialize()
        {
            base.Initialize();
            statProgressType.Add(ActionType.WOODCUTTING);
        }
    }
}
