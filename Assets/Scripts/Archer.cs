using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : MonoBehaviour, ICrowdUnit
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public Transform opponentTarget;
    public Coroutine shootCoroutine;
    [HideInInspector] public NavMeshAgent agent;
    public float torque;
    [HideInInspector] public Animator animator;
    public bool meetingPlaceReached;
    public bool isRunningToMeetingPlace;
    public bool isRuninngToCastle;
    public bool isFighting;

    [Header("Meeting place range")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public GameObject deathParticles;
    public Vector3 meetingPlacePosition;

    public int hp;
    public List<Transform> whoAttackThisUnit = new List<Transform>();

    [Space]
    public int teamIndex;
    public int TeamIndex { get => teamIndex; set => teamIndex = value; }
    public Transform OpponentTarget { get => opponentTarget; set => opponentTarget = value; }

    public Coroutine deathCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        AddUnitToUnitsArray();
        MoveToMeetingPlace();

    }

    //Бег на плац
    private void MeetingPlaceReachedCheck()
    {
        // if (BattleCrowdController.Instance.canRunToCastle)
        //     return;
        if (meetingPlaceReached == false && Vector3.Distance(transform.position, meetingPlacePosition) <= 0.3f)
        {
            agent.isStopped = true;
            animator.SetBool("IsRunning", false);
            meetingPlaceReached = true;
            isRunningToMeetingPlace = false;
        }
    }

    private void FixedUpdate()
    {
        MoveToEnemyCastle();
        MeetingPlaceReachedCheck();
    }

    //НУЖНО УДАЛИТЬ ПОСЛЕ ТОГО КАК ОТДЕБАЖИШЬ и использовать метод BattleCrowdController'a
    private void MoveToEnemyCastle()
    {
        if (opponentTarget == null && !isRuninngToCastle)
        {
            SendToOpponentCastle();
        }
    }

    private void Update()
    {
        if (opponentTarget == null)
            return;

        Vector3 targetDir = opponentTarget.position - transform.position;
        targetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.time * 1);
        // if (opponentTarget != null)
        // {
        //     // Shoot();
        //     transform.LookAt(new Vector3(opponentTarget.position.x, this.transform.position.y, opponentTarget.position.z));
    }

    public void MoveToMeetingPlace()
    {
        if (isRunningToMeetingPlace || meetingPlaceReached)
            return;
        Debug.Log("AASAS");
        isRunningToMeetingPlace = true;
        var destinationPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        meetingPlacePosition = destinationPosition;
        agent.SetDestination(destinationPosition);
        animator.SetBool("IsRunning", true);
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

    private void Shoot()
    {
        if (opponentTarget == null)
            return;

        var newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z + 120));
        var arrowRB = newArrow.GetComponent<Rigidbody>();
        newArrow.GetComponent<Arrow>().teamIndex = teamIndex;
        arrowRB.isKinematic = false;
        var spawnPosition = (opponentTarget.position - transform.position).normalized;
        var randomShootPower = Random.Range(7, 12);
        var randomYDirection = Random.Range(0.1f, 1f);
        arrowRB.AddForce(new Vector3(spawnPosition.x, spawnPosition.y + 0.2f, spawnPosition.z) * 14, ForceMode.Impulse);
        arrowRB.AddTorque(transform.right * torque);
        transform.SetParent(null);
        newArrow.GetComponent<MeleeWeaponTrail>()._base = newArrow.transform;
    }

    public void SendToOpponentCastle()
    {
        if (!BattleCrowdController.Instance.canRunToCastle)
            return;

        var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().SetDestination(opponentCastle.position);
        animator.SetBool("IsRunning", true);
        animator.SetBool("HaveTarget", false);

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
        BattleCrowdController.Instance.playerCrowdTransforms.Remove(this.transform);
        BattleCrowdController.Instance.enemyCrowdTransforms.Remove(this.transform);
        for (int i = 0; i < whoAttackThisUnit.Count; i++)
        {
            whoAttackThisUnit[i].GetComponent<ICrowdUnit>().OpponentTarget = null;
        }
        whoAttackThisUnit.Clear();
        Destroy(gameObject);
    }

    public void AddAttackerUnit(Transform unit)
    {
        // throw new System.NotImplementedException();
    }
}