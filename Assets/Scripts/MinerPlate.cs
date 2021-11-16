using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MinerPlate : MonoBehaviour
{
    public float costPopupNewScale;
    private float costPopupDefaultScale;

    public float newScale;
    private float defaultScale;

    public int rocksRemaining;
    public TextMeshPro rocksRemainingText;

    public Transform plateImage;
    public Transform costPopupImage;

    public List<int> minersCosts = new List<int>();
    public int lastSpawnedMinerIndex;

    private void Awake()
    {
        defaultScale = plateImage.localScale.x;
        costPopupDefaultScale = costPopupImage.localScale.x;
    }

    private void Start()
    {
        rocksRemaining = minersCosts[lastSpawnedMinerIndex];
        rocksRemainingText.text = rocksRemaining.ToString();
    }

    public void DecreaseRemainingRocks()
    {
        rocksRemaining--;

        if (rocksRemaining <= 0)
        {
            lastSpawnedMinerIndex++;
            if (lastSpawnedMinerIndex >= minersCosts.Count)
                rocksRemaining = Random.Range(30, 100);
            else
                rocksRemaining = minersCosts[lastSpawnedMinerIndex];

            Debug.Log("spawn unit");
        }

        rocksRemainingText.text = rocksRemaining.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            plateImage.DOScale(newScale, 0.5f);
            costPopupImage.DOScale(costPopupNewScale, 0.5f);
            other.GetComponent<BackPack>().StartBackPackReset();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            plateImage.DOScale(defaultScale, 0.5f);
            costPopupImage.DOScale(costPopupDefaultScale, 0.5f);
            other.GetComponent<BackPack>().StopBackPackReset();
        }
    }
}