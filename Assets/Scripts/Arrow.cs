using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody arrowRB;
    private MeleeWeaponTrail arrowTrail;
    public int teamIndex;

    private void Awake()
    {
        arrowRB = GetComponent<Rigidbody>();
        arrowTrail = GetComponent<MeleeWeaponTrail>();
        Destroy(gameObject, 10);
    }

    private void Start()
    {
        SoundsManager.Instance.PlayArrowHitSound();
    }
    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit enemyUnit;
        if (other.TryGetComponent<ICrowdUnit>(out enemyUnit))
        {
            if (enemyUnit.TeamIndex != teamIndex)
            {
                StartCoroutine(CollideArrowAfterDelay(enemyUnit, other));
            }
        }
    }

    private IEnumerator CollideArrowAfterDelay(ICrowdUnit enemyUnit, Collider other)
    {
        yield return new WaitForSeconds(0.02f);
        if (other != null)
        {
            enemyUnit.DecreaseHP(1);
            transform.SetParent(other.transform);
            Destroy(arrowRB);
            Destroy(arrowTrail);
        }
    }
}