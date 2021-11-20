using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, ICrowdUnit
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SendToEndPoint(Vector3 endPointPosition)
    {
        GetComponent<NavMeshAgent>().SetDestination(endPointPosition);
        animator.SetBool("IsRunning", true);
    }
}

public interface ICrowdUnit
{
    void SendToEndPoint(Vector3 endPointPosition);
}