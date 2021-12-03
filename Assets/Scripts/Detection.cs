using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Detection : MonoBehaviour
{
    public Transform axeTransform;
    public Pickaxe pickaxe;
    Tween doOneScale;
    Tween doZeroScale;

    public bool canMining = true;

    public List<MiningRock> rocksNearby = new List<MiningRock>();
    public List<ICrowdUnit> enemiesNearby = new List<ICrowdUnit>();

    private void OnTriggerEnter(Collider other)
    {
        MiningRock miningRock;
        if (other.TryGetComponent<MiningRock>(out miningRock) && canMining)
        {
            rocksNearby.Add(miningRock);
            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", true);
            doZeroScale.Kill();
            doOneScale = axeTransform.DOScale(Vector3.one * 1.3f, 0.9f);
        }

        ICrowdUnit enemy;
        if (other.TryGetComponent<ICrowdUnit>(out enemy))
        {
            if (enemy.TeamIndex == 0)
                return;
            enemiesNearby.Add(enemy);
            // rocksNearby.Add(miningRock);
            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", true);
            doZeroScale.Kill();
            doOneScale = axeTransform.DOScale(Vector3.one * 1.3f, 0.9f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MiningRock miningRock;
        if (other.TryGetComponent<MiningRock>(out miningRock) && canMining)
        {
            rocksNearby.Remove(miningRock);
            if (rocksNearby.Count == 0)
            {
                transform.parent.GetComponent<Animator>().SetBool("IsAttacking", false);
                doOneScale.Kill();
                doZeroScale.Kill();

                doZeroScale = axeTransform.DOScale(Vector3.zero, 0.6f);
            }
        }

        ICrowdUnit enemy;
        if (other.TryGetComponent<ICrowdUnit>(out enemy))
        {
            if (enemy.TeamIndex == 0)
                return;
            enemiesNearby.Remove(enemy);
            if (rocksNearby.Count == 0)
            {
                Debug.Log("Exus");
                // rocksNearby.Add(miningRock);
                transform.parent.GetComponent<Animator>().SetBool("IsAttacking", false);
                doOneScale.Kill();
                doZeroScale.Kill();
                doZeroScale = axeTransform.DOScale(Vector3.zero, 0.6f);
            }
        }
    }

    public void RemoveFromNearbyArray(MiningRock rock)
    {
        if (!canMining)
            return;
        if (!rocksNearby.Contains(rock))
            return;
        rocksNearby.Remove(rock);
        if (rocksNearby.Count == 0)
        {
            if (this == null)
                return;

            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", false);
            doOneScale.Kill();
            doZeroScale.Kill();

            doZeroScale = axeTransform.DOScale(Vector3.zero, 0.6f);
        }
    }

    public void RemoveEnemyFromNearbyArray(ICrowdUnit enemy)
    {
        enemiesNearby.Remove(enemy);
        if (enemiesNearby.Count == 0)
        {
            if (this == null)
                return;
            Debug.Log("Юнит был убит, атака сброшена");

            transform.parent.GetComponent<Animator>().SetBool("IsAttacking", false);
            doOneScale.Kill();
            doZeroScale.Kill();

            doZeroScale = axeTransform.DOScale(Vector3.zero, 0.6f);
        }
    }
}