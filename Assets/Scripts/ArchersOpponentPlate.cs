using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ArchersOpponentPlate : MonoBehaviour, IResourceReciever
{
    public float costPopupNewScale;
    private float costPopupDefaultScale;

    public float plateNewScale;
    private float plateDefaultScale;

    [HideInInspector] public int rocksRemaining;
    public TextMeshPro rocksRemainingText;

    public Transform plateImage;
    public Transform costPopupImage;

    public List<int> unitsCosts = new List<int>();
    public int lastSpawnedUnitIndex;

    public GameObject unitPrefab;
    public Transform unitsSpawnPoint;
    public Transform rocksFlyTarget;

    private void Start()
    {
        rocksRemaining = unitsCosts[lastSpawnedUnitIndex];
        rocksRemainingText.text = rocksRemaining.ToString();

        costPopupDefaultScale = costPopupImage.localScale.x;
        plateDefaultScale = plateImage.localScale.x;
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

            Instantiate(unitPrefab, unitsSpawnPoint.position, Quaternion.identity);
        }

        rocksRemainingText.text = rocksRemaining.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PathChecker.Instance.canBuyUnits)
            return;
        PlayerOpponent player;
        if (other.TryGetComponent<PlayerOpponent>(out player) && player.currentState == PlayerOpponentState.RunningToArchersPlate)
        {
            BackPack backPack = other.GetComponent<BackPack>();
            backPack.rocksFlyTarget = rocksFlyTarget;
            plateImage.DOScale(plateNewScale, 0.5f);
            costPopupImage.DOScale(costPopupNewScale, 0.5f);
            other.GetComponent<BackPack>().StartBackPackUnloading();
            player.ChangeState(PlayerOpponentState.Unloading);
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
