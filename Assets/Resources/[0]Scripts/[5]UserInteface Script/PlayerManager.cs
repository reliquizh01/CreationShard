using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Utilities;
using EventFunctionSystem;

namespace UserInterface
{
    public class PlayerManager : BaseManager {

        private static PlayerManager instance = null;
        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    PlayerManager tmp = FindObjectOfType<PlayerManager>();
                    instance = tmp;
                }
                return instance;
            }
        }

        [Header("Player Information")]
        public Image healthBar;
        public Image staminaBar;

        [Header("Growth Bars")]
        public Image lifeSkill;
        public Image equipment;

        [Header("Local Information")]
        public float maxhealth, curHealth, maxStamina, curStamina;


        public void OnEnable()
        {
            EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_PLAYER_STAMINA, UpdateStamina);
            EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_PLAYER_HEALTH, UpdateHealth);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_PLAYER_STAMINA, UpdateStamina);
            EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_PLAYER_HEALTH, UpdateHealth);

        }
        public override void SetPlayerReference(Parameters param = null)
        {
            base.SetPlayerReference(param);
            
            // Failsafe
            if(playerStats == null)
            {
                return;
            }

            List<float> stat = new List<float>();

            stat.Add(playerStats.CurrentHealth);
            stat.Add(playerStats.MaximumHealth);
            stat.Add(playerStats.CurrentStamina);
            stat.Add(playerStats.MaximumStamina);
            InitializeLocalVariables(stat);
        }

        public void InitializeLocalVariables(List<float> playerStats)
        {
            curHealth = playerStats[0];
            maxhealth = playerStats[1];
            curStamina = playerStats[2];
            maxStamina = playerStats[3];
        }

        public void UpdateHealth(Parameters p = null)
        {
            float newHealth = playerStats.CurrentHealth;
            float newMaxHealth = playerStats.MaximumHealth;
            bool changeMax = false;
            if (newMaxHealth != 0)
            {
                changeMax = true;
            }

            curHealth = newHealth;
            maxhealth = (changeMax) ? newMaxHealth : maxhealth;

            float fill = Mathf.Clamp((curHealth / maxhealth), 0, 1);

            healthBar.fillAmount = fill;
        }

        public void UpdateStamina(Parameters p = null)
        {
            float newStamina = playerStats.CurrentStamina;
            float newMaxStamina = playerStats.MaximumStamina;

            bool changeMax = false;
            if(newMaxStamina != 0)
            {
                changeMax = true;
            }

            curStamina = newStamina;

            maxStamina = (changeMax) ? newMaxStamina : maxStamina;

            float fill = Mathf.Clamp((curStamina / maxStamina), 0, 1);

            staminaBar.fillAmount = fill;
        }
    }
}
