using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class PlayerOpponent : MonoBehaviour, IAIMiner
{
    private NavMeshAgent agent;
    public Detection detectionCollider;
    public Transform minerPlatePosition;
    public Transform warriorsPlatePosition;
    public Transform archersPlatePosition;

    public RocksHandler rocksHandler;
    public Transform defaultPosition;

    public PlayerOpponentState currentState = PlayerOpponentState.Idle;
    [HideInInspector] public MiningRock targetRock;

    private Animator animator;
    [HideInInspector] public BackPack backPack;

    public MiningRock TargetRock { get => targetRock; set => targetRock = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        backPack = GetComponent<BackPack>();
    }

    private void Start()
    {
        rocksHandler = RocksHandler.Instance;
        rocksHandler.minersDetections.Add(detectionCollider);
        ChangeState(PlayerOpponentState.Mining);
        InvokeRepeating("SetNewMiningRockDestination", 0.5f, 1);
    }

    private void FixedUpdate()
    {
        if ((detectionCollider.rocksNearby.Count == 0 && targetRock != null) || currentState == PlayerOpponentState.RunningToMinersPlate || currentState == PlayerOpponentState.RunningToWarriorsPlate || currentState == PlayerOpponentState.RunningToStoragePlate || currentState == PlayerOpponentState.RunningToArchersPlate)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        MiningRock miningRock;
        if (other.gameObject.TryGetComponent<MiningRock>(out miningRock))
        {
            if (miningRock == targetRock)
                agent.isStopped = true;
        }
    }

    public void OnRocksFull()
    {
        ChangeState(PlayerOpponentState.RunningToMinersPlate);
    }

    //Animation event
    public void HitRocksNearby()
    {
        List<MiningRock> rocksNearbyCopy = new List<MiningRock>(detectionCollider.rocksNearby);
        for (int i = 0; i < rocksNearbyCopy.Count; i++)
        {
            rocksNearbyCopy[i].HitRock(detectionCollider.pickaxe);
        }
    }

    //После того как помайнил, рандомится место, в которое он побежит. После того как он прибежал в Plate и сбросил ресы, рандомится, побежит он майнить или к хранилищу
    public void SetNewRandomActivityAfterUnloading()
    {
        int newRandomActivityIndex = Random.Range(0, 1);
        if (newRandomActivityIndex == 0 && StorageOpponent.Instance.currentRocksCount != 0)
        {
            ChangeState(PlayerOpponentState.RunningToStoragePlate);
        }
        else
        {
            ChangeState(PlayerOpponentState.Mining);
        }
    }

    public void SetNewRandomActivityAfterLoading()
    {
        int newRandomActivityIndex = Random.Range(0, 3);
        if (newRandomActivityIndex == 0)
        {
            ChangeState(PlayerOpponentState.RunningToMinersPlate);
            return;
        }
        if (newRandomActivityIndex == 1)
        {
            ChangeState(PlayerOpponentState.RunningToWarriorsPlate);
            return;
        }
        if (newRandomActivityIndex == 2)
        {
            ChangeState(PlayerOpponentState.RunningToArchersPlate);
            return;
        }
    }

    public void ChangeState(PlayerOpponentState newState)
    {
        if (newState == PlayerOpponentState.Idle)
        {
            currentState = PlayerOpponentState.Idle;
            animator.SetBool("IsRunning", false);
        }

        if (newState == PlayerOpponentState.Mining)
        {
            currentState = PlayerOpponentState.Mining;
            SetNewMiningRockDestination();
        }

        if (newState == PlayerOpponentState.RunningToStoragePlate)
        {
            animator.SetBool("IsRunning", true);
            targetRock = null;
            agent.SetDestination(StorageOpponent.Instance.transform.position);
            agent.isStopped = false;
            currentState = PlayerOpponentState.RunningToStoragePlate;
        }

        if (newState == PlayerOpponentState.Unloading)
        {
            animator.SetBool("IsRunning", false);
            agent.isStopped = true;
            currentState = PlayerOpponentState.Unloading;
        }

        if (newState == PlayerOpponentState.RunningToMinersPlate)
        {
            animator.SetBool("IsRunning", true);
            agent.isStopped = false;
            targetRock = null;
            agent.SetDestination(minerPlatePosition.position);
            currentState = PlayerOpponentState.RunningToMinersPlate;
        }

        if (newState == PlayerOpponentState.RunningToWarriorsPlate)
        {
            animator.SetBool("IsRunning", true);
            targetRock = null;
            agent.SetDestination(warriorsPlatePosition.position);
            agent.isStopped = false;
            currentState = PlayerOpponentState.RunningToWarriorsPlate;
        }

        if (newState == PlayerOpponentState.RunningToArchersPlate)
        {
            animator.SetBool("IsRunning", true);
            targetRock = null;
            agent.SetDestination(archersPlatePosition.position);
            agent.isStopped = false;
            currentState = PlayerOpponentState.RunningToArchersPlate;
        }
    }

    private IEnumerator IEStartRunningAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("IsRunning", true);
    }

    private void SetNewMiningRockDestination()
    {
        if (currentState != PlayerOpponentState.Mining)
            return;

        // if (detectionCollider.rocksNearby.Count != 0)
        //     return;

        // if (currentState == PlayerOpponentState.RunningToStoragePlate || backPack.isUnloading)
        //     return;

        if (rocksHandler.miningRocks.Count <= 0)
            return;

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        var newDestination = newTarget.transform.position;
        targetRock = newTarget.GetComponent<MiningRock>();
        targetRock.currentMiner = this;
        agent.SetDestination(newDestination);
        agent.isStopped = false;
    }

    public void OnTargetRockDestroyed()
    {
        SetNewMiningRockDestination();
    }
}

public interface IAIMiner
{
    void OnTargetRockDestroyed();
    MiningRock TargetRock { get; set; }
}

public enum PlayerOpponentState
{
    Idle,
    Mining,
    Unloading,
    RunningToMinersPlate,
    RunningToStoragePlate,
    RunningToWarriorsPlate,
    RunningToArchersPlate,
}