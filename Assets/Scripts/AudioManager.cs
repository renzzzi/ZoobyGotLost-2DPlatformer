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
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip firstStageBGM;
    [SerializeField] private AudioClip secondStageBGM;
    [SerializeField] private AudioClip thirdStageBGM;

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

    private void Start()
    {
        PlayMusic(mainMenuBGM);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
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

    public void RestartBackgroundMusic()
    {
        PlayMusic(mainMenuBGM);
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