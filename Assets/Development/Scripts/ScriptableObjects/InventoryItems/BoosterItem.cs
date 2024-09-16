using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item/Booster Item")]
public class BoosterItem : InventoryItem
{
    public BondsCharacter character; //Name of the bonds character that needs the item
    public string partyMember; //Name of the party member that the item needs to be recieved from
}
