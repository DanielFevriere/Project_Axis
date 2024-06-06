using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondsTab : GameTab
{
    BondsManager bondsManager;

    private void Awake()
    {
        bondsManager = BondsManager.Instance;
    }

}