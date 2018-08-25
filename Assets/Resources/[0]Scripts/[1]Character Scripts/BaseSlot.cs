using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barebones.Items;

public class BaseSlot : MonoBehaviour {
    
    public List<EquipType> holdItemType;
    public GameObject currentItem;

    public bool PlaceItemToSlot(GameObject thisItem)
    {
        if(currentItem != null)
        {
            return false;
        }
        
        currentItem = thisItem;
        SetCurrentItemParent();
        return true;
    }
    
    public void SetCurrentItemParent()
    {
        currentItem.transform.parent = this.transform;
        currentItem.transform.rotation = this.transform.rotation;
        currentItem.transform.localPosition = Vector3.zero;
    }

    public void NewSetOfHoldItem(EquipType thisType, bool toAdd = true)
    {
        if(toAdd)
        {
            holdItemType.Add(thisType);
        }
        else
        {
            if (holdItemType.Contains(thisType))
            {
                holdItemType.Remove(thisType);
            }
        }
    }

    public void AdjustSlotPosition(Vector3 newLocalVector)
    {
        if(newLocalVector == null)
        {
            return;
        }
        this.transform.localPosition = newLocalVector;
    }
}
