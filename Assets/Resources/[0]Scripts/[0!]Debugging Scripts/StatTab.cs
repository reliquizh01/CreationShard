using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTab : MonoBehaviour {

    public Text headerText;
    public Text infoText;
	

    public void SetInfoText(int curCount)
    {
        string tmpText = curCount.ToString();
        infoText.text = tmpText;
    }

    public void SetInfoText(int curCount, int maxCount)
    {
        string tmpText = curCount + " / " + maxCount;
        infoText.text = tmpText;
    }
}
