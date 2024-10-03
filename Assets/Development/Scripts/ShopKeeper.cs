using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public Conversation normalConvo;
    public Conversation shopConvo;
    public Conversation buyConvo;
    public Conversation failBuyConvo;

    public List<ShopItem> shopItemList;
}
