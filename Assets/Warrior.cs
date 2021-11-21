using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, ICrowdUnit
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    public Transform opponentTarget;
    public Transform enemyCastle;
    public WarriorEnemyDetection enemyDetection;
    public WarriorAttackDetection enemyAttackDetection;

    [Space]
    public int teamIndex;
    public int TeamIndex { get => teamIndex; set => teamIndex = value; }
    [Space]

    public bool isRunningToMeetingPlace;
    public bool isRuninngToCastle;
    public bool isFighting;

    [Header("Meeting place range")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public float delayToNextHit;
    public float currentDelayValue;
    public int hp;

    public GameObject deathParticles;
    public Vector3 meetingPlacePosition;

    public List<Warrior> whoAttackThisWarrior = new List<Warrior>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        AddUnitToUnitsArray();

        // MoveToMeetingPlace();
        SendToOpponentCastle();
    }

    private void AddUnitToUnitsArray()
    {
        if (teamIndex == 0)
        {
            BattleCrowdController.Instance.playerCrowdTransforms.Add(transform);
        }
        else
        {
            BattleCrowdController.Instance.enemyCrowdTransforms.Add(transform);
        }
    }

    private void FixedUpdate()
    {
        MeetingPlaceReachedCheck();

        MoveToEnemyTarget();

        MoveToEnemyCastle();

        DecreaseInvulnerabilityTime();
    }

    //НУЖНО УДАЛИТЬ ПОСЛЕ ТОГО КАК ОТДЕБАЖИШЬ и использовать метод BattleCrowdController'a
    private void MoveToEnemyCastle()
    {
        if (opponentTarget == null && !isRuninngToCastle)
        {
            SendToOpponentCastle();
        }
    }

    private void MoveToEnemyTarget()
    {
        if (opponentTarget != null)
        {
            agent.SetDestination(opponentTarget.position);
            Vector3 targetDir = opponentTarget.position - transform.position;
            targetDir.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.time * 1);
        }
    }

    private void DecreaseInvulnerabilityTime()
    {
        if (currentDelayValue > 0)
        {
            currentDelayValue -= Time.deltaTime;
        }
    }

    //Бег на плац
    private void MeetingPlaceReachedCheck()
    {
        if (isRunningToMeetingPlace == false && Vector3.Distance(transform.position, meetingPlacePosition) <= 0.3f)
        {
            agent.isStopped = true;
            animator.SetBool("IsRunning", false);
        }
    }

    public void GiveDamageToEnemy()
    {
        if (currentDelayValue > 0)
            return;

        opponentTarget.GetComponent<ICrowdUnit>().DecreaseHP(1);

        currentDelayValue = delayToNextHit;
    }

    public void MoveToMeetingPlace()
    {
        if (isRunningToMeetingPlace)
            return;

        var destinationPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        meetingPlacePosition = destinationPosition;
        agent.SetDestination(destinationPosition);
        animator.SetBool("IsRunning", true);
        isRunningToMeetingPlace = true;
    }

    public void SendToOpponentCastle()
    {
        var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().SetDestination(opponentCastle.position);
        animator.SetBool("IsRunning", true);
        animator.SetBool("EnemyIsNearby", false);

        isRuninngToCastle = true;
    }

    public void DecreaseHP(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            StartCoroutine(IEDeath());
        }
    }

    private IEnumerator IEDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        BattleCrowdController.Instance.enemyCrowdTransforms.Remove(this.transform);
        for (int i = 0; i < whoAttackThisWarrior.Count; i++)
        {
            whoAttackThisWarrior[i].opponentTarget = null;
            // var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
            // whoAttackThisWarrior[i].SendToOpponentCastle(opponentCastle.position);
        }
        whoAttackThisWarrior.Clear();
        Destroy(gameObject);
    }
}

public interface ICrowdUnit
{
    void SendToOpponentCastle();
    void DecreaseHP(int value);

    int TeamIndex { get; set; }
}