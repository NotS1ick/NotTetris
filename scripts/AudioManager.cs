using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource moveSoundAudioSource;
    [SerializeField] public AudioSource backGroundAudioSource;
    public AudioClip[] audioClips;

    void Start()
    {  
        backGroundAudioSource.clip = audioClips[1];
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMoveSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[0]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly.");
        }
    }

    public void PlayRotateSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[2]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on Rotate sound.");
        }
    }

    public void AddToGridSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[3]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on AddToGrid sound.");
        }
    }

    public void GameOverSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[4]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on AddToGrid sound.");
        }
    }

    public void LevelUpSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[5]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on AddToGrid sound.");
        }
    }

    public void LineClearSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[6]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on AddToGrid sound.");
        }
    }

    public void SeceretLineClearSound()
    {
        if (moveSoundAudioSource != null && audioClips.Length > 0)
        {
            moveSoundAudioSource.PlayOneShot(audioClips[7]);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips not set properly on AddToGrid sound.");
        }
    }
}