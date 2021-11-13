using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;

    private bool isRunning;
    private Animator animator;

    public MeleeWeaponTrail weaponTrail;
    public Collider pickaxeCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Animation event
    public void EndHit()
    {
        pickaxeCollider.enabled = false;
        weaponTrail.Emit = false;
    }

    //Animation event
    public void StartHit()
    {
        pickaxeCollider.enabled = true;
        weaponTrail.Emit = true;
        weaponTrail._emitTime = 0.2f;
    }

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        // rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        if (floatingJoystick.Horizontal != 0 || floatingJoystick.Vertical != 0)
        {
            rb.velocity = (direction.normalized * speed);
            var angle = Mathf.Atan2(-floatingJoystick.Horizontal, floatingJoystick.Vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, -angle, 0)), Time.deltaTime * 10);

            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            rb.velocity /= 1.1f;
        }
    }
}