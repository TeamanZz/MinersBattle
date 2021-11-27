using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    public List<Transform> generalSpineRocksTransforms = new List<Transform>();

    public int maxRocksCount = 32;
    public int rocksCount = 0;

    public Transform rocksFlyTarget;
    public Transform generalRocksHolder;
    public Coroutine flyCoroutine;

    public bool isUnloading;

    public bool canRockRandomActivity = true;
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     flyCoroutine = StartCoroutine(IERocksStartFlyToTargetPoint());
        // }
    }

    public void StartBackPackUnloading()
    {
        isUnloading = true;
        flyCoroutine = StartCoroutine(IERocksStartFlyToTargetPoint());
        Miner miner;
        if (TryGetComponent<Miner>(out miner))
        {
            miner.isMovingToStorage = false;
        }
    }

    public void StopBackPackUnload()
    {
        if (flyCoroutine != null)
        {
            StopCoroutine(flyCoroutine);
        }

        RandomNewActivityForOpponent();
    }

    private void RandomNewActivityForOpponent()
    {
        PlayerOpponent playerOpponent;
        if (TryGetComponent<PlayerOpponent>(out playerOpponent))
        {
            playerOpponent.SetNewRandomActivityAfterUnloading();
        }
    }

    public IEnumerator IERocksStartFlyToTargetPoint()
    {
        int lastRockIndex = rocksCount - 1;
        PlayerOpponent playerOpponent;
        TryGetComponent<PlayerOpponent>(out playerOpponent);

        while (true)
        {
            if (IsHaveRocksInBackpack(lastRockIndex))
            {
                Transform rock = generalRocksHolder.GetChild(lastRockIndex).GetChild(0);
                rock.parent = rocksFlyTarget;
                rock.GetComponent<SpineRock>().targetTransform = rocksFlyTarget;
                rock.GetComponent<SpineRock>().isFlyingToBuild = true;
                lastRockIndex--;
                rocksCount--;

                if (playerOpponent != null)
                {
                    int randomChance = Random.Range(0, 25);
                    if (randomChance == 0)
                    {
                        if (canRockRandomActivity)
                        {
                            playerOpponent.rocksRandomActivityWasInvoked = true;
                            playerOpponent.SetNewRandomActivityAfterLoading();
                            StartCoroutine(AllowRockRandomActivity());
                        }
                    }
                }
            }
            else
            {
                isUnloading = false;
                StopBackPackUnload();
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AllowRockRandomActivity()
    {
        canRockRandomActivity = false;
        yield return new WaitForSeconds(2);
        canRockRandomActivity = true;
    }

    private bool IsHaveRocksInBackpack(int index)
    {
        return index >= 0 && generalRocksHolder.GetChild(index).childCount != 0;
    }
}