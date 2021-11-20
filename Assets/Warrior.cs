using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, ICrowdUnit
{
    private Animator animator;
    private NavMeshAgent agent;

    public float minX;
    public float maxX;

    public float minZ;
    public float maxZ;

    private Vector3 meetingPlacePosition;

    private bool canRunToCastle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        MoveToMeetingPlace();
    }

    private void FixedUpdate()
    {
        if (canRunToCastle == false && Vector3.Distance(transform.position, meetingPlacePosition) <= 0.2f)
        {
            agent.isStopped = true;
            animator.SetBool("IsRunning", false);
            canRunToCastle = true;
        }
    }

    public void MoveToMeetingPlace()
    {
        var destinationPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        meetingPlacePosition = destinationPosition;
        agent.SetDestination(destinationPosition);
        animator.SetBool("IsRunning", true);
    }

    public void SendToEnemyCastle(Vector3 endPointPosition)
    {
        GetComponent<NavMeshAgent>().SetDestination(endPointPosition);
        animator.SetBool("IsRunning", true);
    }
}

public interface ICrowdUnit
{
    void SendToEnemyCastle(Vector3 endPointPosition);
}