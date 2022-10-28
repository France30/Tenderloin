using UnityEngine.Audio;
using UnityEngine;

public class GunAudioController : MonoBehaviour
{

    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip noAmmoClip;
    [SerializeField] private AudioClip reloadClip;

    private AudioSource gunSoundEffect;
    private AudioClip currentClip;
    public bool isPlaying { get { return gunSoundEffect.isPlaying;  } }

    public void Play(string action = "shoot")
    {
        switch (action)
        {
            case "shoot":
                gunSoundEffect.clip = shootClip;
                break;
            case "no ammo":
                gunSoundEffect.clip = noAmmoClip;

                if (currentClip == gunSoundEffect.clip && isPlaying)
                    return;
                break;
            case "reload":
                gunSoundEffect.clip = reloadClip;
                break;
        }

        currentClip = gunSoundEffect.clip;
        gunSoundEffect.Play();
    }

    private void Awake()
    {
        gunSoundEffect = GetComponent<AudioSource>();
    }
}
