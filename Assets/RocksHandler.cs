using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksHandler : MonoBehaviour
{
    public static RocksHandler Instance;

    public List<GameObject> rockParticles = new List<GameObject>();

    public Detection player;

    public List<GameObject> rocksStates = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNewRock(int currentRockStateID, Transform oldRockTransform)
    {
        var newRock = Instantiate(rocksStates[currentRockStateID + 1], oldRockTransform.position, oldRockTransform.rotation);
        for (int i = 0; i < rockParticles.Count; i++)
        {
            var newParticle = Instantiate(rockParticles[i], oldRockTransform.position, Quaternion.identity);

        }
    }

    public void RemoveRockFromUnitArrays(MiningRock miningRock)
    {
        player.RemoveFromNearbyArray(miningRock);
    }
}