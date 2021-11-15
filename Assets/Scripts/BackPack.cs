using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    public List<Transform> generalSpineRocksTransforms = new List<Transform>();

    public int maxRocksCount = 32;
    public int rocksCount = 0;

    public Transform mineCartTransform;
    public Transform generalRocksHolder;
    public Coroutine flyCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flyCoroutine = StartCoroutine(FlyToPoint());
        }
    }

    public void StartBackPackReset()
    {
        flyCoroutine = StartCoroutine(FlyToPoint());
    }

    public void StopBackPackReset()
    {
        if (flyCoroutine != null)
            StopCoroutine(flyCoroutine);
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
                rock.parent = mineCartTransform;
                rock.GetComponent<SpineRock>().targetTransform = mineCartTransform;
                rock.GetComponent<SpineRock>().goingToMine = true;
                index--;
                rocksCount--;
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}