using UnityEngine.Audio;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip idleClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    [SerializeField] private AudioSource enemySoundEffect;

    public void Play(string state = "Idle")
    {
        switch (state)
        {
            case "Idle":
                enemySoundEffect.clip = idleClip;
                enemySoundEffect.loop = true;
                break;
            case "Hit":
                enemySoundEffect.clip = hitClip;
                enemySoundEffect.loop = false;
                break;
            case "Death":
                enemySoundEffect.clip = deathClip;
                enemySoundEffect.loop = false;
                break;
        }

        enemySoundEffect.Play();
    }

    private void OnEnable()
    {
        Play("Idle");
    }
}
