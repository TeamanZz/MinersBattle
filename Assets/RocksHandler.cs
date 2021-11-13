using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocksHandler : MonoBehaviour
{
    public static RocksHandler Instance;

    public List<GameObject> rockParticles = new List<GameObject>();

    public Detection player;

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

    public void SpawnNewRock(int currentRockStateID, Transform oldRockTransform)
    {
        var newRock = Instantiate(rocksStates[currentRockStateID + 1], oldRockTransform.position, oldRockTransform.rotation);
        for (int i = 0; i < rockParticles.Count; i++)
        {
            var newParticle = Instantiate(rockParticles[i], oldRockTransform.position, Quaternion.Euler(-90, 0, 0));
        }
    }

    public void SpawnHitParticles(Transform miningRock)
    {
        var newParticle = Instantiate(rockParticles[0], miningRock.position, Quaternion.Euler(-90, 0, 0));
    }

    public void SpawnSpineRocks(Transform miningRock)
    {
        int rocksCount = Random.Range(1, 2);
        for (int i = 0; i < rocksCount; i++)
        {
            var newRockParent = Instantiate(spineRockParentPrefab, miningRock.position, Quaternion.identity);
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
                newRock.GetComponent<SpineRock>().targetTransform = backPack.freeSpineRocksTransforms[backPack.rocksCount];
                backPack.rocksCount++;
            });
        }
    }

    public void RemoveRockFromUnitArrays(MiningRock miningRock)
    {
        player.RemoveFromNearbyArray(miningRock);
    }
}