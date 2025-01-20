using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the Video Player component
    public float waitTime = 10f;    // Time (in seconds) to wait before transitioning
    public string nextSceneName;   // Name of the next scene to load

    private void Start()
    {
        // Start playing the video
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        // Start the coroutine to handle the delay and scene transition
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // Optional: Stop the video if you want to end it early
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
