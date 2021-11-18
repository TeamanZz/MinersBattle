using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class WarriorsPlate : MonoBehaviour
{
    public float costPopupNewScale;
    private float costPopupDefaultScale;

    public float newScale;
    private float defaultScale;

    public int rocksRemaining;
    public TextMeshPro rocksRemainingText;

    public Transform plateImage;
    public Transform costPopupImage;

    public List<int> warriorsCosts = new List<int>();
    public int lastSpawnedWarriorIndex;

    public GameObject warriorPrefab;
    public Transform warriorsSpawnPoint;
    public Transform rocksFlyTarget;

    private void Start()
    {
        rocksRemaining = warriorsCosts[lastSpawnedWarriorIndex];
        rocksRemainingText.text = rocksRemaining.ToString();
    }

    public void DecreaseRemainingRocks()
    {
        rocksRemaining--;

        if (rocksRemaining <= 0)
        {
            lastSpawnedWarriorIndex++;
            if (lastSpawnedWarriorIndex >= warriorsCosts.Count)
                rocksRemaining = Random.Range(30, 100);
            else
                rocksRemaining = warriorsCosts[lastSpawnedWarriorIndex];

            Instantiate(warriorPrefab, warriorsSpawnPoint.position, Quaternion.identity);
        }

        rocksRemainingText.text = rocksRemaining.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            BackPack backPack = other.GetComponent<BackPack>();
            backPack.rocksFlyTarget = rocksFlyTarget;
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
