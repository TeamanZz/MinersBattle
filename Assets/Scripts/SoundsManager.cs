using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;

    public List<AudioClip> pickaxeHitSounds = new List<AudioClip>();
    public List<AudioClip> swordHitSounds = new List<AudioClip>();
    public List<AudioClip> arrowHitSounds = new List<AudioClip>();
    public List<AudioClip> endGameSounds = new List<AudioClip>();
    public List<AudioClip> stackingSound = new List<AudioClip>();

    public List<AudioClip> unitDeathSounds = new List<AudioClip>();
    public List<AudioClip> rockDestroySounds = new List<AudioClip>();
    public List<AudioClip> rockInBackpackSounds = new List<AudioClip>();

    public GameObject battleMusic;
    public float someY;

    private void Awake()
    {
        someY = transform.position.y;
        Instance = this;
    }

    public void DisableBattleMusic()
    {
        battleMusic.SetActive(false);
    }

    public void PlayUnitDeathSound()
    {
        var newHitSound = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        Destroy(newHitSound, 3);
        var newSource = newHitSound.AddComponent<AudioSource>();
        newSource.PlayOneShot(unitDeathSounds[Random.Range(0, unitDeathSounds.Count)]);
    }

    public void PlaySwordHitSound()
    {
        var newHitSound = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        Destroy(newHitSound, 3);
        var newSource = newHitSound.AddComponent<AudioSource>();
        newSource.PlayOneShot(swordHitSounds[Random.Range(0, swordHitSounds.Count)]);
    }

    public void PlayArrowHitSound()
    {
        var newHitSound = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        Destroy(newHitSound, 3);
        var newSource = newHitSound.AddComponent<AudioSource>();
        newSource.PlayOneShot(arrowHitSounds[Random.Range(0, arrowHitSounds.Count)]);
    }
}