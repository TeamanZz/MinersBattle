using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    public MeleeWeaponTrail weaponTrail;
    public Collider weaponCollider;

    //Animation event
    public void EndHit()
    {
        weaponCollider.enabled = false;
        weaponTrail.Emit = false;
    }

    //Animation event
    public void StartHit()
    {
        weaponCollider.enabled = true;
        weaponTrail.Emit = true;
        weaponTrail._emitTime = 0.2f;
    }
}