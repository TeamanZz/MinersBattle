using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpponentEndPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BackPack playerOpponent;
        if (other.TryGetComponent<BackPack>(out playerOpponent))
        {
            playerOpponent.GetComponent<Animator>().SetBool("IsRunning", false);
        }
    }
}