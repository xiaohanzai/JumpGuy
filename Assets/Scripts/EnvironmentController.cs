using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public Transform[] backgroundImageTransforms;
    public Transform collectablesParent;
    private float spriteWidth;
    private Vector3 originalPos;

    public Transform leftBoundaryMarker;

    void Start()
    {
        spriteWidth = backgroundImageTransforms[0].GetComponent<SpriteRenderer>().bounds.size.x;
        originalPos = transform.position;
        InitBGImagePos();
    }

    private void InitBGImagePos()
    {
        for (int i = 0; i < backgroundImageTransforms.Length; i++)
        {
            backgroundImageTransforms[i].position = new Vector3(spriteWidth * i, 0, 0);
        }
    }

    private void Update()
    {
        foreach (Transform child in transform.Find("NewPlatforms"))
        {
            if (child.position.x < leftBoundaryMarker.position.x)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void Scroll(float scrollSpeed)
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        for (int i = 0; i < backgroundImageTransforms.Length; i++)
        {
            //// Move the background leftward continuously
            //backgroundImageTransforms[i].Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // If the background has moved entirely offscreen to the left
            if (backgroundImageTransforms[i].position.x < -spriteWidth)
            {
                // Move it to the right so that it appears to wrap around
                backgroundImageTransforms[i].Translate(Vector3.right * spriteWidth * 2);
            }
        }
    }

    public void Reset()
    {
        transform.position = originalPos;
        InitBGImagePos();
        SetCollectablesActive(collectablesParent);
        foreach (Transform child in transform.Find("NewPlatforms"))
        {
            Destroy(child.gameObject);
        }
    }

    private void SetCollectablesActive(Transform parent)
    {
        // Iterate through each child of the parent GameObject
        foreach (Transform child in parent)
        {
            // Check if the child has the desired tag
            if (child.CompareTag("Collectable"))
            {
                // If it does, set active
                child.gameObject.SetActive(true);
            }

            //// Recursively call this function for grandchildren
            //SetCollectablesActive(child);
        }
    }
}
