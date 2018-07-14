using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Barebones;
using Barebones.Characters;
using Utilities;

public class ObjectInformation : MonoBehaviour {

    private bool initialize = false;
    [Header("GameObject's Information")]
    public GameObject thisObject;
    public BareboneObject thisObjectBase;
    public BareboneCharacter thisObjectCharacter;
    [Header("Information Texts")]
    public Text nameText;
    public Text typeText;
    public Text stateText;
    public Text healthText;

    [Header("Stats Panel(Character Only)")]
    public bool StatInitialize = false;
    public StatList  statList;


    public void Start()
    {
        if (!initialize)
        {
            Initialize();
            initialize = true;
        }
    }

    public void Update()
    {
        Initialize();
    }
    public void Initialize()
    {
        if(thisObject == null)
        {
            Debug.Log("Debug Reference Invalid!");
            return;
        }
        thisObjectCharacter = thisObject.GetComponent<BareboneCharacter>();
        thisObjectBase = thisObject.GetComponent<BareboneObject>();
        // IF PLAYER HAS CHARACTER SCRIPT
        if (thisObjectCharacter != null)
        {
            nameText.text = thisObjectCharacter.CharacterName;
            typeText.text = thisObjectCharacter.ObjectType.ToString();
            healthText.text = thisObjectCharacter.CurrentHealth.ToString() + " / " + thisObjectCharacter.MaximumHealth.ToString();
            if (!StatInitialize)
            {
                CreateStats();
            }
        }
        // IF PLAYER HAS NO CHARACTER SCRIPT
        else if (thisObjectBase != null)
        {
            nameText.text = thisObjectBase.GeneralName;
            typeText.text = thisObjectBase.ObjectType.ToString();
        }
        else
        {
            Debug.Log("Cannot Comprehend Object Selected, Object has no Barebone Script!!");
        }

    }

    public void CreateStats()
    {
        statList.ClearStatPanels();
        for (int i = 0; i < thisObjectCharacter.CharacterStats.Count; i++)
        {
            Parameters param = new Parameters();
            param.AddParameter<string>("StatName", thisObjectCharacter.CharacterStats[i].Name);
            param.AddParameter<int>("curLevel", thisObjectCharacter.CharacterStats[i].CurrentLevel);

            statList.AddStat(param);
        }
        StatInitialize = true;
    }

}
