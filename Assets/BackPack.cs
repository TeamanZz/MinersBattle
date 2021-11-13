using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    public List<Transform> generalSpineRocksTransforms = new List<Transform>();
    public List<Transform> freeSpineRocksTransforms = new List<Transform>();

    public int rocksCount = 0;

    private void Start()
    {
        freeSpineRocksTransforms = generalSpineRocksTransforms;
    }

}