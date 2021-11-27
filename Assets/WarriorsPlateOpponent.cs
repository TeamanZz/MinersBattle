using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WarriorsPlateOpponent : MonoBehaviour, IResourceReciever
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
        if (!PathChecker.Instance.canBuyUnits)
            return;
        PlayerOpponent player;
        if (other.TryGetComponent<PlayerOpponent>(out player) && player.currentState == PlayerOpponentState.RunningToWarriorsPlate)
        {
            BackPack backPack = other.GetComponent<BackPack>();
            backPack.rocksFlyTarget = rocksFlyTarget;
            plateImage.DOScale(newScale, 0.5f);
            costPopupImage.DOScale(costPopupNewScale, 0.5f);
            other.GetComponent<BackPack>().StartBackPackUnloading();
            player.ChangeState(PlayerOpponentState.UnloadingOnWarriors);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerOpponent player;
        if (other.TryGetComponent<PlayerOpponent>(out player))
        {
            plateImage.DOScale(plateDefaultScale, 0.5f);
            costPopupImage.DOScale(costPopupDefaultScale, 0.5f);
            other.GetComponent<BackPack>().StopBackPackUnload();
        }
    }

    public void RecieveResources()
    {
    }
}
