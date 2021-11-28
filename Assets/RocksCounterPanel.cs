using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocksCounterPanel : MonoBehaviour
{
    public TextMeshProUGUI rocksCount;
    public BackPack playerBackpack;
    private void FixedUpdate()
    {
        rocksCount.text = playerBackpack.rocksCount.ToString();
    }
}
