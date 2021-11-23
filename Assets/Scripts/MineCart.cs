using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour, IResourceReciever
{
    public MinerPlate minerPlate;
    public MinerPlateOpponent minerPlateOpponent;

    // public void DecreaseRemainingRocks()
    // {
    //     if (minerPlateOpponent != null)
    //         minerPlateOpponent.DecreaseRemainingRocks();
    //     else
    //         minerPlate.DecreaseRemainingRocks();
    // }

    public void RecieveResources()
    {
        if (minerPlateOpponent != null)
            minerPlateOpponent.DecreaseRemainingRocks();
        else
            minerPlate.DecreaseRemainingRocks();
    }
}