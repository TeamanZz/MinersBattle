using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    public Transform shootTarget;
    // public float shootPower;
    public float torque;

    public Coroutine shootCoroutine;

    private void Update()
    {
        transform.LookAt(shootTarget);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyUnitBase enemyUnit;
        if (other.TryGetComponent<EnemyUnitBase>(out enemyUnit))
        {
            if (shootTarget != null)
                return;

            Debug.Log("New Target");
            shootTarget = enemyUnit.transform;
            GetComponent<Animator>().SetBool("HaveTarget", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyUnitBase enemyUnit;
        if (other.TryGetComponent<EnemyUnitBase>(out enemyUnit))
        {
            if (enemyUnit == shootTarget.GetComponent<EnemyUnitBase>())
            {
                Debug.Log("Вышел");
                shootTarget = null;
                GetComponent<Animator>().SetBool("HaveTarget", false);
            }
        }
    }

    private void Shoot()
    {
        if (shootTarget == null)
            return;

        var newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z + 120));
        var arrowRB = newArrow.GetComponent<Rigidbody>();
        arrowRB.isKinematic = false;
        var spawnPosition = (shootTarget.position - transform.position).normalized;
        var randomShootPower = Random.Range(7, 12);
        var randomYDirection = Random.Range(0.1f, 1f);
        arrowRB.AddForce(new Vector3(spawnPosition.x, spawnPosition.y + randomYDirection, spawnPosition.z) * randomShootPower, ForceMode.Impulse);
        arrowRB.AddTorque(transform.right * torque);
        transform.SetParent(null);
        newArrow.GetComponent<MeleeWeaponTrail>()._base = newArrow.transform;
    }
}