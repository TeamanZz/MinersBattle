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

    [Space]
    public int teamIndex;
    public int TeamIndex { get => teamIndex; set => teamIndex = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        AddUnitToUnitsArray();
        // MoveToMeetingPlace();

        SendToOpponentCastle();
    }

    private void FixedUpdate()
    {
        MoveToEnemyCastle();
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
        if (opponentTarget != null)
        {
            // Shoot();
            transform.LookAt(new Vector3(opponentTarget.position.x, this.transform.position.y, opponentTarget.position.z));
        }
    }

    public void MoveToMeetingPlace()
    {
        if (isRunningToMeetingPlace)
            return;

        var destinationPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        meetingPlacePosition = destinationPosition;
        agent.SetDestination(destinationPosition);
        animator.SetBool("IsRunning", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit enemyUnit;
        if (other.TryGetComponent<ICrowdUnit>(out enemyUnit))
        {
            if (opponentTarget != null)
                return;

            opponentTarget = other.transform;
            animator.SetBool("HaveTarget", true);
            animator.SetBool("IsRunning", false);

            agent.isStopped = true;
            isRuninngToCastle = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICrowdUnit enemyUnit;
        if (other.TryGetComponent<ICrowdUnit>(out enemyUnit))
        {
            if (enemyUnit == opponentTarget.GetComponent<ICrowdUnit>())
            {
                opponentTarget = null;
                animator.SetBool("HaveTarget", false);
            }
        }
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
        arrowRB.AddForce(new Vector3(spawnPosition.x, spawnPosition.y + 0.5f, spawnPosition.z) * 11, ForceMode.Impulse);
        arrowRB.AddTorque(transform.right * torque);
        transform.SetParent(null);
        newArrow.GetComponent<MeleeWeaponTrail>()._base = newArrow.transform;
    }

    public void SendToOpponentCastle()
    {
        var opponentCastle = BattleCrowdController.Instance.GetOpponentCastleTransform(teamIndex);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().SetDestination(opponentCastle.position);
        animator.SetBool("IsRunning", true);
        Debug.Log("sended");
        animator.SetBool("HaveTarget", false);

        // animator.SetBool("EnemyIsNearby", false);

        isRuninngToCastle = true;
    }

    public void DecreaseHP(int value)
    {
        throw new System.NotImplementedException();
    }
}