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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flyCoroutine = StartCoroutine(FlyToPoint());
        }
    }

    public void StartBackPackReset()
    {
        isUnloading = true;
        flyCoroutine = StartCoroutine(FlyToPoint());
        Miner miner;
        if (TryGetComponent<Miner>(out miner))
        {
            miner.isMovingToStorage = false;
        }
    }

    public void StopBackPackReset()
    {
        if (flyCoroutine != null)
        {
            StopCoroutine(flyCoroutine);
        }

        PlayerOpponent playerOpponent;
        if (TryGetComponent<PlayerOpponent>(out playerOpponent))
        {
            playerOpponent.ChangeState(PlayerOpponentState.Mining);
        }

    }

    public IEnumerator FlyToPoint()
    {
        int index = rocksCount - 1;
        // int index = rocksCount;
        while (true)
        {
            if (index >= 0 && generalRocksHolder.GetChild(index).childCount != 0)
            {
                Transform rock = generalRocksHolder.GetChild(index).GetChild(0);
                rock.parent = rocksFlyTarget;
                rock.GetComponent<SpineRock>().targetTransform = rocksFlyTarget;
                rock.GetComponent<SpineRock>().isFlyingToBuild = true;
                index--;
                rocksCount--;
            }
            else
            {
                isUnloading = false;
                StopBackPackReset();
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}