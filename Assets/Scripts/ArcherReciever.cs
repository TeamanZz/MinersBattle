using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherReciever : MonoBehaviour, IResourceReciever
{
    public ArchersPlate archersPlate;

    public void RecieveResources()
    {
        archersPlate.DecreaseRemainingRocks();
    }
}
