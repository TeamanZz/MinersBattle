using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using DG.Tweening;

public class PathChecker : MonoBehaviour
{
    public static PathChecker Instance;

    public GameObject fightAnnouncementText;
    // public Tween fightAnnouncementTween;
    public float fightAnnouncementFlyDuration;
    // Sequence sequence;

    public bool isFight;
    private void Awake()
    {
        Instance = this;
    }

    public NavMeshAgent pathAgent;


    public bool canBuyUnits = true;

    public GameObject environmentMusic;
    public GameObject startBattleSound;
    public GameObject battleSound;
    public Transform pathCheckerTo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckPathExist();
        }
    }

    public void ShowFightAnnouncement()
    {
        isFight = true;
        fightAnnouncementText.SetActive(true);

        environmentMusic.SetActive(false);
        startBattleSound.SetActive(true);
        StartCoroutine(StartPlayBattleMusic());


        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            fightAnnouncementText.transform.DOScale(1f, fightAnnouncementFlyDuration).SetEase(Ease.OutBack);
        });
        sequence.AppendInterval(2);
        sequence.AppendCallback(() =>
        {
            fightAnnouncementText.transform.DOScale(0f, fightAnnouncementFlyDuration).SetEase(Ease.InBack);
        });
    }

    private IEnumerator StartPlayBattleMusic()
    {
        yield return new WaitForSeconds(2.5f);
        battleSound.SetActive(true);
    }

    private IEnumerator IECheckPathExist()
    {
        yield return new WaitForSeconds(0.1f);
        // var path = new NavMeshPath();
        // pathAgent.transform.position = pathCheckerFrom.position;
        // pathAgent.CalculatePath(pathCheckerTo.position, path);
        pathAgent.SetDestination(pathCheckerTo.position);
        // if (path.status == NavMeshPathStatus.PathComplete)
        // {
        //     Debug.Log("Path exist");
        // }
        // OnTruePathExist();
    }

    public void OnTruePathExist()
    {
        Destroy(pathAgent);
        PlayerOpponent.Instance.ChangeState(PlayerOpponentState.RunToEndPoint);
        for (int i = 0; i < RocksHandler.Instance.minersDetections.Count; i++)
        {
            Miner miner;
            if (RocksHandler.Instance.minersDetections[i] == null)
                return;

            if (RocksHandler.Instance.minersDetections[i].transform.parent.TryGetComponent<Miner>(out miner))
            {
                miner.MoveToCastle();
            }
        }

        canBuyUnits = false;

        if (!isFight)
            ShowFightAnnouncement();

        BattleCrowdController.Instance.canRunToCastle = true;
        BattleCrowdController.Instance.SendPlayerUnitsToEnemyCastle();
        BattleCrowdController.Instance.SendEnemyUnitsToPlayerCastle();

        Player.Instance.detectionCollider.rocksNearby.Clear();
        Player.Instance.animator.SetBool("IsAttacking", false);
        Player.Instance.detectionCollider.canMining = false;
    }

    public void CheckPathExist()
    {
        StartCoroutine(IECheckPathExist());
    }
}