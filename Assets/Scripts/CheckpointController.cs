using UnityEngine;
using TMPro;

public class CheckpointController : MonoBehaviour
{
    private PlayerHealth playerState;
    [HideInInspector] public float respawnHP;
    [HideInInspector] public bool gotCheckPoint = false;
    public Transform respawnPoint;

    [Header("Checkpoint UI")]
    public TextMeshProUGUI checkpointMessageText;

    void Start()
    {
        playerState = GetComponent<PlayerHealth>();
    }

    void Update()
    {
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
        if (transform.position.y < -10f)
        {
            Debug.Log("Player fell. Respawning at checkpoint.");
            transform.position = respawnPoint.position;
            Physics.SyncTransforms();
        }
    }
}
