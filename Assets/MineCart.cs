using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    public MinerPlate minerPlate;

    public void DecreaseRemainingRocks()
    {
        minerPlate.DecreaseRemainingRocks();
    }
}