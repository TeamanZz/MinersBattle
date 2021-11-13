using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Detection : MonoBehaviour
{
    public Transform axeTransform;

    Tween doOneScale;
    Tween doZeroScale;

    private void OnTriggerEnter(Collider other)
    {
        MiningRock miningRock;
        if (other.TryGetComponent<MiningRock>(out miningRock))
        {
            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", true);
            doZeroScale.Kill();
            doOneScale = axeTransform.DOScale(Vector3.one * 1.3f, 0.9f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MiningRock miningRock;
        if (other.TryGetComponent<MiningRock>(out miningRock))
        {
            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", false);
            doOneScale.Kill();
            doZeroScale = axeTransform.DOScale(Vector3.zero, 0.6f);
        }
    }
}