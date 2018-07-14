using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Minigame
{
    public class MGWoodCutting : MinigameBase {

        [SerializeField]private Collider playerChecker;

        public void Initialize()
        {
            triggerType = TriggerType.COLLIDER;
            statName = "WoodCutting";
        }
    }
}
