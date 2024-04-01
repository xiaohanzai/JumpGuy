using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private EnvironmentController environmentController;
    private PlayerController playerController;

    public float initScrollSpeed = 2f;
    private float scrollSpeed;

    public bool isGameStarted;
    public bool isGameOver;

    public float minY = -3.3f;
    public float maxY = -2.2f;
    public Transform instantiateMarker;
    private Vector3 instantiateMarkerOriginalPos;
    public Transform rightBoundaryMarker;
    public Transform leftBoundaryMarker;

    public GameObject[] platformPrefabs;

    private float timer;

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (instance == null)
        {
            instance = this; // Set the instance to this object if it's null
            DontDestroyOnLoad(gameObject); // Optional: Keep GameManager alive between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        environmentController = FindObjectOfType<EnvironmentController>();
        timer = 0;
        scrollSpeed = initScrollSpeed;
        instantiateMarkerOriginalPos = instantiateMarker.position;
    }

    void Update()
    {
        if (!isGameStarted && Input.GetKeyDown(KeyCode.Space) && !isGameOver)
        {
            isGameStarted = true;
        }

        if (isGameStarted)
        {
            environmentController.Scroll(scrollSpeed);
            HandlePlatformInstantiation();
            if (playerController.transform.position.x < leftBoundaryMarker.position.x)
            {
                SetGameOver();
            }
        }

        timer += Time.deltaTime;
        if (timer > 15)
        {
            timer = 0;
            scrollSpeed += 0.3f * initScrollSpeed;
        }
    }

    public void SetGameOver()
    {
        isGameStarted = false;
        isGameOver = true;
        playerController.Freeze();
        UIManager.instance.SetGameOver();
    }

    public void StartGame()
    {
        isGameStarted = false;
        isGameOver = false;
        timer = 0;
        scrollSpeed = initScrollSpeed;
        playerController.Reset();
        environmentController.Reset();
        UIManager.instance.Reset();
        instantiateMarker.position = instantiateMarkerOriginalPos;
    }

    private void HandlePlatformInstantiation()
    {
        if (rightBoundaryMarker.position.x < instantiateMarker.position.x)
        {
            return;
        }

        // instantiate 5 platforms at a time
        int i = 0;
        while (i < 5)
        {
            // randomly choose a platform
            int ind = Random.Range(0, platformPrefabs.Length);
            GameObject newPlatform = Instantiate(platformPrefabs[ind], environmentController.transform.Find("NewPlatforms"));
            // get its length and move it forward
            float length = newPlatform.transform.Find("PlatformBlock").GetComponent<SpriteRenderer>().bounds.size.x;
            newPlatform.transform.position = new Vector3(instantiateMarker.position.x + length / 2f + 1f, Random.Range(minY, maxY), 0);
            // move the marker forward
            instantiateMarker.position = new Vector3(instantiateMarker.position.x + length + 1f, instantiateMarker.position.y, 0);
            i++;
        }
        
    }
}
