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

    public Transform endPoint;

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

    public bool CheckPathExist()
    {


        var path = new NavMeshPath();
        pathAgent.CalculatePath(endPoint.position, path);

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Path don't exist");
            return false;
        }
        else
        {
            Debug.Log("Path exist");

            if (!isFight)
                ShowFightAnnouncement();

            // pathAgent.SetDestination(endPoint.position);
            return true;
        }
    }
}