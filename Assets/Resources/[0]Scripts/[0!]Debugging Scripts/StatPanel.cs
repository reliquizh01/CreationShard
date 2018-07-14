using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


/// <summary>
///  All StatPanel has a maximum number of 5 StatTabs
///  
/// StatTabs contains the stats [ AGI : 15 ] which then can be changed or altered depending on the gameObject 
/// that is refered by the ObjectInformation script
/// </summary>
public class StatPanel : MonoBehaviour {

    public List<StatTab> statTabs;
    public int filledTab = 0;
    public bool filled = false;

    public void SetStatTab(Parameters param = null)
    {
        if (filledTab < statTabs.Count)
        {
            Debug.Log("Opening Tab: " + filledTab);
            statTabs[filledTab].gameObject.SetActive(true);
            string statName = param.GetWithKeyParameterValue("StatName", " ") + " :";
            if (statName != " ")
            {
                statTabs[filledTab].headerText.text = statName;
            }
            else
            {
                Debug.Log("Name has no Value! Stat is Invalid!"); return;
            }
            
            int statCurCount = param.GetWithKeyParameterValue("curLevel", 0);
            int statMaxCount = param.GetWithKeyParameterValue("maxLevel", 0);

            if (statMaxCount != 0)
            {
                statTabs[filledTab].SetInfoText(statCurCount, statMaxCount);
            }
            else
            {
                statTabs[filledTab].SetInfoText(statCurCount);
            }

            filledTab += 1;
            if(filledTab > 4)
            {
                filled = true;
            }
        }
        else
        {
            Debug.Log("Index Provided is more than 5! Please Check Input!");
        }
    }
    public bool CheckFilled()
    {
        if(filledTab >= 4)
        {
            filled = true;
            return true;
        }
        return false;
    }
}
