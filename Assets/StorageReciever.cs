using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageReciever : MonoBehaviour, IResourceReciever
{
    public Storage storage;

    public void RecieveResources()
    {
        storage.RecieveResources();
    }
}
