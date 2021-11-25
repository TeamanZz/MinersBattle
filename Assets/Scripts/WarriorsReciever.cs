using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsReciever : MonoBehaviour, IResourceReciever
{
    public WarriorsPlate warriorsPlate;
    public WarriorsPlateOpponent warriorsPlateOpponent;

    public void RecieveResources()
    {
        if (warriorsPlateOpponent != null)
            warriorsPlateOpponent.DecreaseRemainingRocks();
        else
            warriorsPlate.RecieveResources();

    }

}
