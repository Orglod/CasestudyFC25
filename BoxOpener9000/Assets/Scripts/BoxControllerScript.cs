using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshProUGUI

public class BoxControllerScript : MonoBehaviour
{
    public GameObject box;
    private Animator boxAnimator;
    private bool isAnimating = false;
    private bool isBoxOpen = false;

    public GameObject[] itemPrefabs;
    private int currentPrefabIndex = 0;

    public Vector3 prefabScale = new Vector3(1, 1, 1);
    public Vector3 endPositionOffset = new Vector3(0, 3f, 0);

    public float moveSpeed = 2f;
    public float rotationSpeed = 100f;
    public float scaleDownSpeed = 2f;

    public Material boxMaterial1;
    public Material boxMaterial2;
    public Texture[] textures;

    public Button openBoxButton;
    public TextMeshProUGUI buttonText; // Use TextMeshProUGUI instead of Text

    private List<GameObject> spawnedItems = new List<GameObject>();

    // Light Animation Parameters
    public Light boxLight;
    public float lightRangeOpen = 5f;
    public float lightRangeClose = 0f;
    public float lightAnimationSpeed = 2f;

    // Material Color Gradient Transition (Random colors)
    public Material planeMaterial;   // Material for the plane
    public Color[] possibleColors; // List of colors to randomly choose from
    public float colorTransitionSpeed = 2f; // Speed of the color transition

    void Start()
    {
        boxAnimator = box.GetComponent<Animator>();
        openBoxButton.onClick.AddListener(OnButtonClick);

        buttonText.text = "Open the Box";

        // Ensure the light starts at the "closed" range
        if (boxLight)
        {
            boxLight.range = lightRangeClose;
        }

        // Set initial material color to a random one
        if (planeMaterial) planeMaterial.color = GetRandomColor();
    }

    void OnButtonClick()
    {
        if (isAnimating) return;

        // Toggle button text
        buttonText.text = isBoxOpen ? "Open the Box" : "Close the Box";

        StartCoroutine(HandleBoxAnimation());
    }

    IEnumerator HandleBoxAnimation()
    {
        isAnimating = true;

        if (isBoxOpen)
        {
            // Close the box
            yield return StartCoroutine(ScaleDownAndDestroyItems());
            boxAnimator.SetTrigger("CloseBox");
            yield return new WaitForSeconds(1f);

            // Change box textures
            ChangeBoxTextures();

            // Start color gradient transition
            StartCoroutine(TransitionMaterialColors());

            // Animate light to the "closed" range
            if (boxLight)
            {
                yield return StartCoroutine(AnimateLightRange(lightRangeClose));
            }
        }
        else
        {
            // Open the box
            boxAnimator.SetTrigger("OpenBox");
            yield return new WaitForSeconds(1f);

            // Start light animation and item movement simultaneously
            if (boxLight)
            {
                StartCoroutine(AnimateLightRange(lightRangeOpen));
            }
            SpawnItem();
        }

        isBoxOpen = !isBoxOpen; // Toggle the state
        isAnimating = false;
    }

    void ChangeBoxTextures()
    {
        if (textures.Length > 0)
        {
            boxMaterial1.mainTexture = textures[Random.Range(0, textures.Length)];
            boxMaterial2.mainTexture = textures[Random.Range(0, textures.Length)];
        }
    }

    void SpawnItem()
    {
        // Destroy previously spawned items (if any remain)
        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        if (itemPrefabs.Length == 0) return;

        GameObject spawnedItem = Instantiate(itemPrefabs[currentPrefabIndex], box.transform.position, Quaternion.identity);
        spawnedItem.transform.localScale = prefabScale;

        spawnedItems.Add(spawnedItem);

        StartCoroutine(MoveAndRotateItem(spawnedItem));

        currentPrefabIndex = (currentPrefabIndex + 1) % itemPrefabs.Length;
    }

    IEnumerator MoveAndRotateItem(GameObject item)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = item.transform.position;
        Vector3 endPosition = startPosition + endPositionOffset;

        Quaternion initialRotation = item.transform.rotation;

        while (elapsedTime < 1f)
        {
            item.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);

            // Rotate around its own axis, considering its initial rotation
            item.transform.rotation = initialRotation * Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        item.transform.position = endPosition; // Ensure final position
    }

    IEnumerator ScaleDownAndDestroyItems()
    {
        foreach (var item in spawnedItems)
        {
            Vector3 initialScale = item.transform.localScale;
            float elapsedTime = 0f;

            while (item.transform.localScale.magnitude > 0.01f)
            {
                item.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime * scaleDownSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(item); // Destroy the item after it has scaled down
        }

        spawnedItems.Clear(); // Clear the list of items
    }

    IEnumerator AnimateLightRange(float targetRange)
    {
        float initialRange = boxLight.range;
        float elapsedTime = 0f;

        while (Mathf.Abs(boxLight.range - targetRange) > 0.01f)
        {
            boxLight.range = Mathf.Lerp(initialRange, targetRange, elapsedTime * lightAnimationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        boxLight.range = targetRange;
    }

    IEnumerator TransitionMaterialColors()
    {
        Color startColor = planeMaterial.color;
        Color targetColor = GetRandomColor(); // Get a random color each time

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            Color currentColor = Color.Lerp(startColor, targetColor, elapsedTime * colorTransitionSpeed);
            if (planeMaterial) planeMaterial.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set
        if (planeMaterial) planeMaterial.color = targetColor;
    }

    // Random color selection from the list of possible colors
    Color GetRandomColor()
    {
        if (possibleColors.Length == 0)
        {
            return Color.white; // Default to white if no colors are available
        }

        return possibleColors[Random.Range(0, possibleColors.Length)];
    }
}
