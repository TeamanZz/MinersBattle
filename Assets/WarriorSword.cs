using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSword : MonoBehaviour
{
    public Warrior warrior;
    public ICrowdUnit unitComponent;

    private void Awake()
    {
        unitComponent = warrior.GetComponent<ICrowdUnit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            if (crowdUnit.TeamIndex != unitComponent.TeamIndex)
            {
                warrior.GiveDamageToEnemy();
            }
        }
    }
}