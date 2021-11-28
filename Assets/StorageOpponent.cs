using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StorageOpponent : MonoBehaviour, IResourceReciever
{
    public static StorageOpponent Instance;

    public int currentRocksCount;

    public TextMeshPro rocksRemainingText;
    public Transform popupImage;
    public float popupNewScale;
    public float popupDefaultScale;
    public Transform storageCover;
    public float newStorageCoverRotation;

    public Transform plateImage;
    public float plateNewScale;
    public float plateDefaultScale;

    public Transform chest;

    public List<Transform> minersNearby = new List<Transform>();

    public bool playerInStorage;

    public Coroutine flyToPlayer;

    public GameObject flyingRockPrefab;
    public AudioSource source;

    private void Start()
    {
        popupDefaultScale = popupImage.localScale.x;
        plateDefaultScale = plateImage.localScale.x;
        rocksRemainingText.text = currentRocksCount.ToString();
    }

    public void RecieveResources()
    {
        source.PlayOneShot(SoundsManager.Instance.stackingSound);
        currentRocksCount++;

        rocksRemainingText.text = currentRocksCount.ToString();
    }

    public void GiveResources()
    {
        currentRocksCount--;
        source.PlayOneShot(SoundsManager.Instance.stackingSound);

        rocksRemainingText.text = currentRocksCount.ToString();
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator StartFlyToPlayer(PlayerOpponent player)
    {
        while (playerInStorage)
        {
            yield return new WaitForSeconds(0.1f);
            if ((player.backPack.rocksCount < player.backPack.maxRocksCount) && currentRocksCount > 0)
            {
                var flyingRock = Instantiate(flyingRockPrefab, transform.position, Quaternion.identity, player.backPack.generalSpineRocksTransforms[player.backPack.rocksCount]);
                flyingRock.GetComponent<SpineRock>().targetTransform = player.backPack.generalSpineRocksTransforms[player.backPack.rocksCount];
                GiveResources();
                player.backPack.rocksCount++;
            }
            else
            {
                player.SetNewRandomActivityAfterLoading();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PathChecker.Instance.canBuyUnits)
            return;
        Miner miner;
        if (other.TryGetComponent<Miner>(out miner))
        {
            minersNearby.Add(miner.transform);
            BackPack backPack = miner.GetComponent<BackPack>();
            backPack.rocksFlyTarget = chest;
            storageCover.DOLocalRotate(new Vector3(newStorageCoverRotation, 0, 0), 0.6f).SetEase(Ease.InOutBack);
            popupImage.DOScale(popupNewScale, 0.5f);
            backPack.StartBackPackUnloading();
            miner.agent.isStopped = true;

        }
        PlayerOpponent player;

        if (other.TryGetComponent<PlayerOpponent>(out player))
        {
            plateImage.DOScale(plateNewScale, 0.5f);
            storageCover.DOLocalRotate(new Vector3(newStorageCoverRotation, 0, 0), 0.6f).SetEase(Ease.InOutBack);
            popupImage.DOScale(popupNewScale, 0.5f);
            playerInStorage = true;
            player.ChangeState(PlayerOpponentState.UnloadingOnStorage);
            flyToPlayer = StartCoroutine(StartFlyToPlayer(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Miner miner;
        if (other.TryGetComponent<Miner>(out miner))
        {
            minersNearby.Remove(miner.transform);
            if (minersNearby.Count == 0)
            {
                storageCover.DOLocalRotate(new Vector3(0, 0, 0), 0.6f).SetEase(Ease.InOutBack);
                popupImage.DOScale(popupDefaultScale, 0.5f);
            }
        }
        PlayerOpponent player;
        if (other.TryGetComponent<PlayerOpponent>(out player))
        {
            plateImage.DOScale(plateDefaultScale, 0.5f);
            if (minersNearby.Count == 0)
            {
                storageCover.DOLocalRotate(new Vector3(0, 0, 0), 0.6f).SetEase(Ease.InOutBack);
                popupImage.DOScale(popupDefaultScale, 0.5f);
            }
            StopCoroutine(flyToPlayer);
            playerInStorage = false;
        }
    }
}
