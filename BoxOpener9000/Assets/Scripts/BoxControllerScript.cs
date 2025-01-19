using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxControllerScript : MonoBehaviour
{
    public GameObject box;                    // The box GameObject
    private Animator boxAnimator;             // Animator for the box
    private bool isAnimating = false;         // Flag to check if an animation is in progress

    // Prefab spawning
    public GameObject[] itemPrefabs;         // Array of item prefabs to spawn
    private int currentPrefabIndex = 0;      // To track which prefab to spawn

    // Size of the prefab
    public Vector3 prefabScale = new Vector3(1, 1, 1); // Default size for the spawned prefab

    // Upward motion and rotation parameters
    public float moveSpeed = 2f;             // Speed at which the prefab moves upward
    public float rotationSpeed = 100f;       // Rotation speed of the prefab

    // Box material textures
    public Material boxMaterial1;
    public Material boxMaterial2;
    public Texture[] textures;               // Array of textures to change

    // UI Button reference
    public Button openBoxButton;

    void Start()
    {
        boxAnimator = box.GetComponent<Animator>();
        openBoxButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Only trigger the animation if no animation is in progress
        if (isAnimating) return;

        StartCoroutine(HandleBoxAnimation());
    }

    // Coroutine to handle the box animation and item spawning
    IEnumerator HandleBoxAnimation()
    {
        isAnimating = true;

        // Trigger closing animation first
        boxAnimator.SetTrigger("CloseBox");
        yield return new WaitForSeconds(1f); // Wait for the close animation to finish (adjust time if necessary)

        // Change box materials (textures)
        ChangeBoxTextures();

        // Trigger opening animation
        boxAnimator.SetTrigger("OpenBox");
        yield return new WaitForSeconds(1f); // Wait for the open animation to finish (adjust time if necessary)

        // Spawn the item from the box
        SpawnItem();

        isAnimating = false;
    }

    // Change box textures based on the array
    void ChangeBoxTextures()
    {
        if (textures.Length > 0)
        {
            boxMaterial1.mainTexture = textures[Random.Range(0, textures.Length)];
            boxMaterial2.mainTexture = textures[Random.Range(0, textures.Length)];
        }
    }

    // Spawn the item prefab from the box
    void SpawnItem()
    {
        // Check if we have items in the array
        if (itemPrefabs.Length == 0) return;

        // Instantiate the item prefab and apply size
        GameObject spawnedItem = Instantiate(itemPrefabs[currentPrefabIndex], box.transform.position, Quaternion.identity);
        spawnedItem.transform.localScale = prefabScale;

        // Apply upward motion and rotation
        StartCoroutine(MoveAndRotateItem(spawnedItem));

        // Update the prefab index for the next item
        currentPrefabIndex = (currentPrefabIndex + 1) % itemPrefabs.Length;
    }

    // Coroutine to handle upward motion and rotation of the spawned item
    IEnumerator MoveAndRotateItem(GameObject item)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = item.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 3f; // Move 3 units upward (adjust if necessary)

        while (elapsedTime < 1f)
        {
            item.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            item.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate item
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        item.transform.position = endPosition; // Ensure final position
    }
}

