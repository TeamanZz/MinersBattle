using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsReciever : MonoBehaviour, IResourceReciever
{
    public WarriorsPlate warriorsPlate;

    public void RecieveResources()
    {
        warriorsPlate.DecreaseRemainingRocks();
    }
}
