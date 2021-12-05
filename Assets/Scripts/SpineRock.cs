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
    public AudioSource source;

    // public Transform TargetTransform
    // {
    //     get => targetTransform;
    //     set
    //     {

    //         targetTransform = value;

    //     }

    // }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

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
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.1f)
            {
                transform.parent = targetTransform;
                if (isFlyingToBuild)
                {
                    Debug.Log("builded");
                    transform.parent.GetComponent<IResourceReciever>().RecieveResources();
                    isFlyingToBuild = false;
                    Destroy(gameObject, 3);
                }
                else
                {
                    // targetTransform = null;
                    // Debug.Log(transform.parent);
                }
            }
            if (!scaleRestored)
            {
                hasNoParent = false;
                RestoreScale();
                // Debug.Log(targetTransform);

            }

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.001f)
            {
                GeneralRocksTransforms transforms;
                if (targetTransform.parent.TryGetComponent<GeneralRocksTransforms>(out transforms) == true)
                    source.PlayOneShot(SoundsManager.Instance.rockInBackpackSounds[Random.Range(0, SoundsManager.Instance.rockInBackpackSounds.Count)]);
                targetTransform = null;
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