using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherReciever : MonoBehaviour, IResourceReciever
{
    public ArchersPlate archersPlate;
    public ArchersOpponentPlate archersOpponentPlate;

    public void RecieveResources()
    {
        if (archersOpponentPlate != null)
            archersOpponentPlate.DecreaseRemainingRocks();
        else
            archersPlate.DecreaseRemainingRocks();
    }
}