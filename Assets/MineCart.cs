using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour, IResourceReciever
{
    public MinerPlate minerPlate;

    public void DecreaseRemainingRocks()
    {
        minerPlate.DecreaseRemainingRocks();
    }

    public void RecieveResources()
    {
        minerPlate.DecreaseRemainingRocks();
    }
}