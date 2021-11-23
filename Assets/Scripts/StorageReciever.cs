using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageReciever : MonoBehaviour, IResourceReciever
{
    public Storage storage;
    public StorageOpponent storageOpponent;

    public void RecieveResources()
    {
        if (storageOpponent != null)
            storageOpponent.RecieveResources();

        else
            storage.RecieveResources();
    }
}