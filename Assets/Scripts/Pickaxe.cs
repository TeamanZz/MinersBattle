using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public BackPack backPack;

    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            if (crowdUnit.TeamIndex != 0)
            {
                SoundsManager.Instance.PlaySwordHitSound();
                crowdUnit.DecreaseHP(1);
            }
        }
    }
}