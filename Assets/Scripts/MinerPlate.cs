using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinerPlate : MonoBehaviour
{
    public float newScale;

    private float defaultScale;
    public Transform plateImage;

    private void Awake()
    {
        defaultScale = plateImage.localScale.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            plateImage.DOScale(newScale, 0.5f);
            other.GetComponent<BackPack>().StartBackPackReset();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            plateImage.DOScale(defaultScale, 0.5f);
            other.GetComponent<BackPack>().StopBackPackReset();
        }
    }
}