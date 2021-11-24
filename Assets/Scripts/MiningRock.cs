using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiningRock : MonoBehaviour
{
    public float delayToNextHit;
    public float currentDelayValue;

    private Vector3 punchVector = new Vector3(1.5f, 0.3f, -2f);
    private float punchDuration = 0.3f;
    private int vibrato = 10;
    private float elastic = 1;

    public IAIMiner currentMiner;

    public int currentHp;

    public int currentStateID = 0;

    public BackPack lastHitFromBackpack;

    public bool CanSetMiner(Miner miner)
    {
        if (currentMiner == null)
        {
            currentMiner = miner;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        if (currentDelayValue > 0)
        {
            currentDelayValue -= Time.deltaTime;
        }
    }

    public void HitRock(Pickaxe pickaxe)
    {
        Debug.Log("hitted");
        if (currentDelayValue > 0)
            return;

        lastHitFromBackpack = pickaxe.backPack;
        transform.DOPunchRotation(punchVector, punchDuration, vibrato, elastic);
        currentDelayValue = delayToNextHit;
        RocksHandler.Instance.SpawnHitParticles(transform);

        if (pickaxe.backPack.rocksCount < pickaxe.backPack.maxRocksCount)
        {
            RocksHandler.Instance.SpawnSpineRocks(transform, lastHitFromBackpack);
            currentHp--;
        }

        if (currentHp <= 0)
        {
            RocksHandler.Instance.SpawnNewRock(currentStateID, this);
            RocksHandler.Instance.RemoveRockFromUnitArrays(this);

            if (currentStateID == 1)
                BattleCrowdController.Instance.CheckPath();
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class RockState
    {
        public Vector3 scale;
        public Mesh mesh;
        public int hp;
    }
}