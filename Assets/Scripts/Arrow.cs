using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody arrowRB;
    private MeleeWeaponTrail arrowTrail;

    private void Awake()
    {
        arrowRB = GetComponent<Rigidbody>();
        arrowTrail = GetComponent<MeleeWeaponTrail>();
        Destroy(gameObject, 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyUnitBase enemyUnit;
        if (other.TryGetComponent<EnemyUnitBase>(out enemyUnit))
        {
            transform.SetParent(enemyUnit.transform);
            Destroy(arrowRB);
            Destroy(arrowTrail);
            // arrowRB.isKinematic = true;
            // arrowRB.useGravity = false;
        }
    }
}