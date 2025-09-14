using UnityEngine;
using System;

public enum SoundType
{
    // Player and Miscellaneous
    Jump,
    Hurt,
    Win,
    KeyCollect,
    PortalOpen,

    // Music
    FirstStageDeath, SecondStageDeath, ThirdStageDeath,
    
    // Tiles
    WalkGrass, GroundHitGrass,
    WalkWood, GroundHitWood,
    WalkStone, GroundHitStone,
    WalkGravel, GroundHitGravel,
    WalkMetal, GroundHitMetal,
    WalkRubber, GroundHitRubber,

    // UI
    ButtonClick, ButtonHover,
    PauseGame, ResumeGame,
    ExitMainMenu,

    Save, Heal,

    PortalWhooshing
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip levelOneBGM;
    [SerializeField] private AudioClip levelTwoBGM;
    [SerializeField] private AudioClip levelThreeBGM;
    [SerializeField] private AudioClip levelFourBGM;
    [SerializeField] private AudioClip levelFiveBGM;
    [SerializeField] private AudioClip levelSixBGM;

    [System.Serializable]
    public class Sound
    {
        public SoundType soundType;
        public AudioClip[] clips;
    }

    [Header("Audio Clips")]
    [SerializeField] private Sound[] sounds;

    private void Awake()
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

    private void OnEnable()
    {
        GameManager.Instance.OnSceneLoad += PlayBGMMusic;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSceneLoad -= PlayBGMMusic;
    }

    public void PlayBGMMusic(int currentScene)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        switch (currentScene)
        {
            case 0:
                musicSource.clip = mainMenuBGM;
                break;
            case 1:
                musicSource.clip = levelOneBGM;
                break;
            case 2:
                musicSource.clip = levelTwoBGM;
                break;
            case 3:
                musicSource.clip = levelThreeBGM;
                break;
            case 4:
                musicSource.clip = levelFourBGM;
                break;
            case 5:
                musicSource.clip = levelFiveBGM;
                break;
            case 6:
                musicSource.clip = levelSixBGM;
                break;
            default:
                break;
        }

        if (musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundType soundType)
    {
        Sound s = Array.Find(sounds, s => s.soundType == soundType);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundType + " not found!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, s.clips.Length);
        sfxSource.PlayOneShot(s.clips[randomIndex]);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void UnpauseMusic()
    {
        musicSource.UnPause();
    }
}