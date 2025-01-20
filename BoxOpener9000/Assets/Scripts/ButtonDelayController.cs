using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.SceneManagement;

public class ButtonDelayController : MonoBehaviour
{
    public GameObject targetButtonObject; // Reference to the entire button GameObject
    public float delayTime = 5f;          // Time (in seconds) to enable the button
    public string nextSceneName;          // Name of the next scene to load

    private void Start()
    {
        // Disable the entire button GameObject at the start
        if (targetButtonObject != null)
        {
            targetButtonObject.SetActive(false); // Makes the button invisible and inactive
        }

        // Start the coroutine to enable the button after a delay
        StartCoroutine(EnableButtonAfterDelay());
    }

    private IEnumerator EnableButtonAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Enable the entire button GameObject
        if (targetButtonObject != null)
        {
            targetButtonObject.SetActive(true); // Makes the button visible and interactable
        }
    }

    public void LoadNextScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(nextSceneName);
    }
}
