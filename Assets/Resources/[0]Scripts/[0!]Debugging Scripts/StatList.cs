using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

using Barebones.Characters;

public class StatList : MonoBehaviour {

    public List<StatPanel> statPanels;
    public GameObject statPanel;

    // Adds StatPanel to statPanels
    private void AddPanel()
    {
        GameObject tmpPanel = Instantiate(statPanel, this.transform);
        statPanels.Add(tmpPanel.GetComponent<StatPanel>());
    }

    public void AddStat(Parameters param = null)
    {
        if(param == null)
        {
            Debug.Log("Parameter is invalid! Check parameters!");
            return;
        }
        // Check if Panel is less than 0 (usually occurs when initializing
        if(statPanels.Count <= 0)
        {
            AddPanel();
        }
        // Find a StatPanel that is not Filled
        int panelIdx = statPanels.FindIndex(x => x.filled == false);
        if (statPanels[panelIdx].CheckFilled())
        {
            Debug.Log("All StatPanels are filled and thus have returned an invalid reference, Adding a New Panel! Attempting Again!");
            AddPanel();
            AddStat();
            return;
        }
        statPanels[panelIdx].SetStatTab(param);
    }

    public void ClearStatPanels()
    {
        for (int i = 0; i < statPanels.Count; i++)
        {
            Destroy(statPanels[i].gameObject);
        }
        statPanels.Clear();
    }
}
