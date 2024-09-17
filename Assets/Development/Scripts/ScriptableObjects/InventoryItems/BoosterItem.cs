using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item/Booster Item")]
public class BoosterItem : InventoryItem
{
    public BondsCharacter requiredBondsCharacter; //Name of the bonds character that needs the item
    public string requiredPartyMember; //Name of the party member that the item needs to be recieved from
    public int bondsExpAmount; //The amount of bonds exp thats given for that character
}
