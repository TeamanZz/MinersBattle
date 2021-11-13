using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpineRock : MonoBehaviour
{
    public Transform targetTransform;
    public float flySpeed;

    public bool scaleRestored;

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * flySpeed);
            if (!scaleRestored)
                RestoreScale();
        }

    }

    public void RestoreScale()
    {
        transform.DOScale(new Vector3(0.11f, 0.11f, 0.11f), 0.5f);
        scaleRestored = true;
    }
}