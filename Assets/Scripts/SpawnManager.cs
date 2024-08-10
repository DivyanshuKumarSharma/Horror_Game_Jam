using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private static SpawnManager instance;

    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpawnManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SpawnManager");
                    instance = go.AddComponent<SpawnManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Destroying duplicate SpawnManager.");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // Initialize or reset necessary references
        InitializeReferences();

        // Delay positioning to ensure everything is loaded
        StartCoroutine(PositionPlayerAfterDelay(0.1f)); // Adjust the delay as needed
    }

    private IEnumerator PositionPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PositionPlayer();
    }

    private void InitializeReferences()
    {
        // Ensure all critical references are set
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint")?.transform;
        // Other initialization code...
    }

    public void PositionPlayer()
    {
        GameObject player = PersistentPlayerManager.Instance.GetPlayerInstance();

        if (player != null)
        {
            Debug.Log("Player found.");

            // Check if respawnPoint exists; if not, fallback to playerSpawnPoint
            if (playerSpawnPoint != null)
            {
                player.transform.position = playerSpawnPoint.position;
                player.transform.rotation = playerSpawnPoint.rotation;
                Debug.Log("Player positioned at: " + player.transform.position);
            }
            else
            {
                Debug.LogWarning("PlayerSpawnPoint not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }
    }
}