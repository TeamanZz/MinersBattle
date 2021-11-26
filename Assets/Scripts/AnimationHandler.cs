using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Detection detection;
    public Miner miner;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if ((detection.rocksNearby.Count == 0 && miner.targetRock != null) || miner.isMovingToStorage == true || miner.isMovingToCastle == true)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
}