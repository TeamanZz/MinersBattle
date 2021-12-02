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

    public GameObject minerPrefab;
    public Transform minersSpawnPoint;
    public Transform rocksFlyTarget;

    public AudioSource source;

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
        if (!PathChecker.Instance.canBuyUnits)
            return;
        rocksRemaining--;
        source.PlayOneShot(SoundsManager.Instance.stackingSound);
        if (rocksRemaining <= 0)
        {
            lastSpawnedMinerIndex++;
            if (lastSpawnedMinerIndex >= minersCosts.Count)
                rocksRemaining = Random.Range(30, 100);
            else
                rocksRemaining = minersCosts[lastSpawnedMinerIndex];

            Instantiate(minerPrefab, minersSpawnPoint.position, Quaternion.identity);
        }

        rocksRemainingText.text = rocksRemaining.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PathChecker.Instance.canBuyUnits)
            return;
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            BackPack backPack = other.GetComponent<BackPack>();
            backPack.rocksFlyTarget = rocksFlyTarget;
            plateImage.DOScale(newScale, 0.5f);
            costPopupImage.DOScale(costPopupNewScale, 0.5f);
            other.GetComponent<BackPack>().StartBackPackUnloading();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            plateImage.DOScale(defaultScale, 0.5f);
            costPopupImage.DOScale(costPopupDefaultScale, 0.5f);
            other.GetComponent<BackPack>().StopBackPackUnload();
        }
    }
}