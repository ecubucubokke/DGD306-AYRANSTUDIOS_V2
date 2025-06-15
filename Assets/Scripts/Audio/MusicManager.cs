using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [System.Serializable]
    public class SceneMusic
    {
        [Tooltip("The exact name of the scene (case-sensitive) that this track belongs to.")]
        public string sceneName;
        [Tooltip("Audio clip that should play in the specified scene.")]
        public AudioClip musicClip;
    }

    [Header("Scene → Music Mapping")]
    [SerializeField] private SceneMusic[] sceneMusic;  // List of scene/clip pairs set up in the Inspector

    [Header("Audio Source Settings")]
    [SerializeField] private AudioSource audioSource;   // Optional; will be created automatically if null
    [SerializeField, Range(0f, 1f)] private float defaultVolume = 0.4f;

    private void Awake()
    {
        // Singleton pattern so that only one instance persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure we have an AudioSource component
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = defaultVolume;

            // Subscribe to scene change callback
            SceneManager.sceneLoaded += HandleSceneLoaded;

            // Immediately play for the startup scene
            HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    /// <summary>
    /// Plays the assigned clip for the specified scene name, if one exists.
    /// </summary>
    /// <param name="sceneName">Name of the loaded scene.</param>
    private void PlayMusicForScene(string sceneName)
    {
        if (sceneMusic == null || sceneMusic.Length == 0) return;

        foreach (var pair in sceneMusic)
        {
            if (pair.sceneName == sceneName && pair.musicClip != null)
            {
                if (audioSource.clip == pair.musicClip) return; // Already playing correct clip

                audioSource.Stop();
                audioSource.clip = pair.musicClip;
                audioSource.Play();
                return;
            }
        }

        // If no clip is found for the scene, optionally stop music
        // audioSource.Stop();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }
} 