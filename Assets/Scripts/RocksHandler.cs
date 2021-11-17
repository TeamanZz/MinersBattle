using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocksHandler : MonoBehaviour
{
    public static RocksHandler Instance;

    public List<MiningRock> miningRocks = new List<MiningRock>();

    public List<GameObject> rockParticles = new List<GameObject>();

    public List<Detection> minersDetections = new List<Detection>();

    public List<GameObject> rocksStates = new List<GameObject>();

    public GameObject spineRockParentPrefab;
    public GameObject spineRockPrefab;

    public BackPack backPack;

    [Header("Rock Settings")]
    public float flyDuration = 0.5f;

    [Header("Rotate Settings")]
    public float rotateDuration;
    public float stregn;
    public float randomness;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < miningRocks.Count; i++)
        {
            // miningRocks[i].transform.rotation = Quaternion.Euler(0, miningRocks[i].transform.rotation.y + Random.Range(0, 361), 0);

            // float xOffset = miningRocks[i].transform.position.x + Random.Range(-0.3f, 0.3f);
            // float yOffset = miningRocks[i].transform.position.y + Random.Range(-0.6f, 0f);
            // float zOffset = miningRocks[i].transform.position.z + Random.Range(-0.3f, 0.3f);
            // miningRocks[i].transform.position = new Vector3(xOffset, yOffset, zOffset);
        }
    }

    public void SpawnNewRock(int currentRockStateID, MiningRock oldRock)
    {
        var newRock = Instantiate(rocksStates[currentRockStateID + 1], oldRock.transform.position, oldRock.transform.rotation);

        for (int i = 0; i < rockParticles.Count; i++)
        {
            var newParticle = Instantiate(rockParticles[i], oldRock.transform.position, Quaternion.Euler(-90, 0, 0));
        }

        //Если камень истощается
        if (currentRockStateID == rocksStates.Count - 2 && oldRock.currentMiner != null)
        {
            oldRock.currentMiner.OnRockDestroyed();
        }
        else
        {
            //Указываем свежепоявившийся камень как цель
            //Если камень был чьей то целью, переопределяем значения 
            if (oldRock.currentMiner != null)
            {
                newRock.GetComponent<MiningRock>().currentMiner = oldRock.currentMiner;
                newRock.GetComponent<MiningRock>().currentMiner.targetRock = newRock.GetComponent<MiningRock>();
            }
        }
    }

    public void SpawnHitParticles(Transform miningRock)
    {
        var newParticle = Instantiate(rockParticles[0], miningRock.position, Quaternion.Euler(-90, 0, 0));
    }

    public void SpawnSpineRocks(Transform miningRock, BackPack backPack)
    {
        int rocksCount = Random.Range(1, 2);
        for (int i = 0; i < rocksCount; i++)
        {
            var newRockParent = Instantiate(spineRockParentPrefab, miningRock.position, Quaternion.identity);
            Destroy(newRockParent, 2);
            var newRock = Instantiate(spineRockPrefab, newRockParent.transform);

            Vector3 targetPosition = new Vector3(newRock.transform.position.x + Random.Range(-2, 2), newRock.transform.position.y + 2, newRock.transform.position.z + Random.Range(-2, 2));

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                newRock.transform.DOLocalMove(targetPosition - newRock.transform.position, flyDuration);
                newRock.transform.DOShakeRotation(rotateDuration, stregn, randomness: randomness);
                newRock.transform.DOScale(0.22f, flyDuration);
            });

            sequence.AppendInterval(flyDuration);

            sequence.AppendCallback(() =>
            {
                newRock.GetComponent<SpineRock>().targetTransform = backPack.generalSpineRocksTransforms[backPack.rocksCount];
                backPack.rocksCount++;
                if (backPack.rocksCount >= backPack.maxRocksCount)
                {
                    Miner miner;
                    if (backPack.TryGetComponent<Miner>(out miner))
                    {
                        Debug.Log("Need Move To Storage");
                        miner.MoveToStorage();
                    }
                }
            });
        }
    }

    public void RemoveRockFromUnitArrays(MiningRock miningRock)
    {
        for (int i = 0; i < minersDetections.Count; i++)
        {
            minersDetections[i].RemoveFromNearbyArray(miningRock);
        }

        miningRocks.Remove(miningRock);
    }
}