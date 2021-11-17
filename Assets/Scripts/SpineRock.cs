using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpineRock : MonoBehaviour
{
    public Transform targetTransform;
    public bool isFlyingToBuild;
    public float flySpeed;

    public bool hasNoParent = true;
    public bool scaleRestored;

    private void Start()
    {
        StartCoroutine(CheckFallOnGround());
    }

    public IEnumerator CheckFallOnGround()
    {
        yield return new WaitForSeconds(1);
        if (hasNoParent)
        {
            transform.parent = null;
            transform.DOLocalMove(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), 1).SetEase(Ease.InBack);
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * flySpeed);
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.5f)
            {
                transform.parent = targetTransform;

                if (isFlyingToBuild)
                {
                    transform.parent.GetComponent<IResourceReciever>().RecieveResources();
                    isFlyingToBuild = false;
                    Destroy(gameObject, 3);
                }
            }
            if (!scaleRestored)
            {
                hasNoParent = false;
                RestoreScale();
            }
        }
    }

    public void RestoreScale()
    {
        transform.DOScale(new Vector3(0.11f, 0.11f, 0.11f), 0.5f);
        scaleRestored = true;
    }
}

public interface IResourceReciever
{
    void RecieveResources();
}