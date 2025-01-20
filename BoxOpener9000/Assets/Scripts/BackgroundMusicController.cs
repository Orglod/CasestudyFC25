using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip backgroundMusic;  // The background music clip
    public float volume = 1f;          // Volume control for the music
    public float startTime = 0f;       // The time in seconds to start the music from (default is 0)
    private AudioSource audioSource;   // AudioSource to play the background music

    void Start()
    {
        // Add an AudioSource component if it doesn't already exist
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip and volume
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.volume = volume;
            audioSource.loop = true;  // Loop the background music
            audioSource.time = startTime; // Set the start time of the music
            audioSource.Play();      // Start playing the music from the specified time
        }
    }

    // Function to adjust the volume at runtime
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume); // Clamp volume between 0 and 1
        audioSource.volume = volume;
    }

    // Function to change the start time dynamically
    public void SetStartTime(float newStartTime)
    {
        startTime = newStartTime;
        audioSource.time = startTime;  // Set the start time of the music
        if (!audioSource.isPlaying)
        {
            audioSource.Play();  // Play the music if it's not already playing
        }
    }
}
