using UnityEngine;

public enum SoundType
{
    Click,
    Upgrade,
    Money
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioClip moneySound;

    [Header("Ambience")]
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioClip ambienceClip;
    [SerializeField] private float ambienceVolume = 0.2f;

    private void Awake()
    {
        Instance = this;

        SetupAmbience();
    }

    public void PlaySound(SoundType type)
    {
        AudioClip clip = type switch
        {
            SoundType.Click => clickSound,
            SoundType.Upgrade => upgradeSound,
            SoundType.Money => moneySound,
            _ => null
        };

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void SetupAmbience()
    {
        if (ambienceSource == null || ambienceClip == null) return;

        ambienceSource.clip = ambienceClip;
        ambienceSource.loop = true;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.Play();
    }
}
