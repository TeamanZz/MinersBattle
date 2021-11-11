using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            plateImage.localScale = new Vector3(newScale, newScale, 1);
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
            plateImage.localScale = new Vector3(defaultScale, defaultScale, 1);
    }
}