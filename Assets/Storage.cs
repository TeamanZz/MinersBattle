using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Storage : MonoBehaviour, IResourceReciever
{
    public static Storage Instance;
    public int currentRocksCount;
    public TextMeshPro rocksRemainingText;
    public Transform popupImage;
    public float popupNewScale;
    public float popupDefaultScale;

    public Transform chest;

    public List<Transform> minersNearby = new List<Transform>();

    private void Start()
    {
        popupDefaultScale = popupImage.localScale.x;
    }

    public void RecieveResources()
    {
        currentRocksCount++;

        rocksRemainingText.text = currentRocksCount.ToString();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Miner miner;
        if (other.TryGetComponent<Miner>(out miner))
        {
            minersNearby.Add(miner.transform);
            BackPack backPack = miner.GetComponent<BackPack>();
            backPack.rocksFlyTarget = chest;
            // plateImage.DOScale(newScale, 0.5f);
            popupImage.DOScale(popupNewScale, 0.5f);
            backPack.StartBackPackReset();
            miner.agent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Miner miner;
        if (other.TryGetComponent<Miner>(out miner))
        {
            minersNearby.Remove(miner.transform);
            if (minersNearby.Count == 0)
                popupImage.DOScale(popupDefaultScale, 0.5f);

        }
    }
}