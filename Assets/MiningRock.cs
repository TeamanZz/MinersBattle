using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiningRock : MonoBehaviour
{
    public float delayToNextHit;
    public float currentDelayValue;

    private Vector3 punchVector = new Vector3(1.5f, 0.3f, -2f);
    private float punchDuration = 0.3f;
    private int vibrato = 10;
    private float elastic = 1;

    // public int maxHp;
    public int currentHp;

    // public List<RockState> rockStates = new List<RockState>();
    public int currentStateID = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickaxe pickaxe;
        if (other.TryGetComponent<Pickaxe>(out pickaxe))
            if (currentDelayValue <= 0)
            {
                Debug.Log("Ай");
                transform.DOPunchRotation(punchVector, punchDuration, vibrato, elastic);
                currentDelayValue = delayToNextHit;
                currentHp--;
                if (currentHp <= 0)
                {
                    RocksHandler.Instance.SpawnNewRock(currentStateID, transform);
                    RocksHandler.Instance.RemoveRockFromUnitArrays(this);
                    Destroy(gameObject);
                    // currentStateID++;
                    // ChangeRockState();
                }
            }
    }

    // private void ChangeRockState()
    // {
    //     transform.localScale = rockStates[currentStateID].scale;
    //     GetComponent<MeshFilter>().mesh = rockStates[currentStateID].mesh;
    //     currentHp = rockStates[currentStateID].hp;
    // }

    private void FixedUpdate()
    {
        if (currentDelayValue > 0)
        {
            currentDelayValue -= Time.deltaTime;
        }
    }

    [System.Serializable]
    public class RockState
    {
        public Vector3 scale;
        public Mesh mesh;
        public int hp;
    }
}