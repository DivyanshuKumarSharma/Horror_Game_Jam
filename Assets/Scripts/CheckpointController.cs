using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    private PlayerHealth playerState;
    [HideInInspector] public float respawnHP;
    [HideInInspector] public bool gotCheckPoint = false;
    public Transform respawnPoint;
    public GameObject playerSpawnPoint; // Reference to the PlayerSpawnPoint GameObject

    [Header("Checkpoint UI")]
    public TextMeshProUGUI checkpointMessageText;

    void Start()
    {
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        playerState = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            Debug.Log("FOUND PLAYER SPAWN");
        }
        else
        {
            Debug.Log("FOUND PLAYER SPAWN");
        }

        RespawnIfFall();
    }

    public void SetCheckpoint(Transform newPosition, string checkpointMessage)
    {
        gotCheckPoint = true;
        respawnPoint = newPosition;
        respawnHP = 100f;
        Debug.Log("Checkpoint set at: " + respawnPoint.position + " with HP: " + respawnHP);

        // Set checkpoint message
        if (checkpointMessageText != null)
        {
            checkpointMessageText.text = checkpointMessage;
            checkpointMessageText.gameObject.SetActive(true);
        }
    }

    private void RespawnIfFall()
    {
        if (transform.position.y < -2f)
        {
            Debug.Log("Player fell. Respawning at checkpoint.");

            // Use the SpawnManager to handle player positioning
            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.PositionPlayer();
            }
            else
            {
                Debug.LogError("SpawnManager instance is not found.");
            }
        }
    }
}
