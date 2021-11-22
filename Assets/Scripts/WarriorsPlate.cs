using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class WarriorsPlate : MonoBehaviour, IResourceReciever
{
    public float costPopupNewScale;
    private float costPopupDefaultScale;

    public float newScale;
    private float plateDefaultScale;

    public int rocksRemaining;
    public TextMeshPro rocksRemainingText;

    public Transform plateImage;
    public Transform costPopupImage;

    public List<int> unitsCosts = new List<int>();
    public int lastSpawnedUnitIndex;

    public GameObject unitPrefab;
    public Transform unitsSpawnPoint;
    public Transform rocksFlyTarget;

    public BattleCrowdController crowdController;

    private void Start()
    {
        costPopupDefaultScale = costPopupImage.localScale.x;
        plateDefaultScale = plateImage.localScale.x;

        rocksRemaining = unitsCosts[lastSpawnedUnitIndex];
        rocksRemainingText.text = rocksRemaining.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var newWarrior = Instantiate(unitPrefab, unitsSpawnPoint.position, Quaternion.identity);
            // crowdController.playerCrowdTransforms.Add(newWarrior.transform);
        }
    }

    public void DecreaseRemainingRocks()
    {
        rocksRemaining--;

        if (rocksRemaining <= 0)
        {
            lastSpawnedUnitIndex++;
            if (lastSpawnedUnitIndex >= unitsCosts.Count)
                rocksRemaining = Random.Range(30, 100);
            else
                rocksRemaining = unitsCosts[lastSpawnedUnitIndex];

            var newWarrior = Instantiate(unitPrefab, unitsSpawnPoint.position, Quaternion.identity);
            // crowdController.playerCrowdTransforms.Add(newWarrior.transform);
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
            plateImage.DOScale(plateDefaultScale, 0.5f);
            costPopupImage.DOScale(costPopupDefaultScale, 0.5f);
            other.GetComponent<BackPack>().StopBackPackReset();
        }
    }

    public void RecieveResources()
    {
    }
}
