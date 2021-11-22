using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, ICrowdUnit
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    public Transform opponentTarget;
    public WarriorEnemyDetection enemyDetection;
    public WarriorAttackDetection enemyAttackDetection;

    [Space]
    public int teamIndex;
    public int TeamIndex { get => teamIndex; set => teamIndex = value; }
    public int hp;
    public Transform OpponentTarget { get => opponentTarget; set => opponentTarget = value; }
    public bool isRuninngToCastle;
    public bool isRunningToMeetingPlace;
    public bool meetingPlaceReached;

    [Header("Meeting place range")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public float delayToNextHit;
    public float currentDelayValue;

    public GameObject deathParticles;
    public Vector3 meetingPlacePosition;
    public List<Transform> whoAttackThisUnit = new List<Transform>();
    private Coroutine deathCoroutine;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        AddUnitToUnitsArray();

        MoveToMeetingPlace();
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
        if (BattleCrowdController.Instance.canRunToCastle)
            return;
        if (meetingPlaceReached == false && Vector3.Distance(transform.position, meetingPlacePosition) <= 0.3f)
        {
            agent.isStopped = true;
            animator.SetBool("IsRunning", false);
            meetingPlaceReached = true;
            isRunningToMeetingPlace = false;
        }
    }

    public void GiveDamageToEnemy()
    {
        if (currentDelayValue > 0 || opponentTarget == null)
            return;
        opponentTarget.GetComponent<ICrowdUnit>().DecreaseHP(1);

        currentDelayValue = delayToNextHit;
    }

    public void MoveToMeetingPlace()
    {
        if (isRunningToMeetingPlace || meetingPlaceReached)
            return;
        Debug.Log("start Run");
        isRunningToMeetingPlace = true;
        var destinationPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        meetingPlacePosition = destinationPosition;
        agent.SetDestination(destinationPosition);
        animator.SetBool("IsRunning", true);
    }

    public void SendToOpponentCastle()
    {

        if (!BattleCrowdController.Instance.canRunToCastle)
            return;

        var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().SetDestination(opponentCastle.position);
        animator.SetBool("IsRunning", true);
        animator.SetBool("EnemyIsNearby", false);

        isRuninngToCastle = true;
    }

    public void DecreaseHP(int value)
    {
        if (deathCoroutine != null)
            return;
        hp -= value;
        if (hp <= 0)
        {
            deathCoroutine = StartCoroutine(IEDeath());
        }
    }

    private IEnumerator IEDeath()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        BattleCrowdController.Instance.enemyCrowdTransforms.Remove(this.transform);
        BattleCrowdController.Instance.playerCrowdTransforms.Remove(this.transform);
        for (int i = 0; i < whoAttackThisUnit.Count; i++)
        {
            if (whoAttackThisUnit[i] != null)
                whoAttackThisUnit[i].GetComponent<ICrowdUnit>().OpponentTarget = null;
            // var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
            // whoAttackThisWarrior[i].SendToOpponentCastle(opponentCastle.position);
        }
        whoAttackThisUnit.Clear();
        Destroy(gameObject);
    }

    public void AddAttackerUnit(Transform unit)
    {
        whoAttackThisUnit.Add(unit);
    }
}

public interface ICrowdUnit
{
    void SendToOpponentCastle();
    void DecreaseHP(int value);
    Transform OpponentTarget { get; set; }
    void AddAttackerUnit(Transform unit);
    int TeamIndex { get; set; }
}