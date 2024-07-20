using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private PlayerHealth playerState;
    [HideInInspector] public float respawnHP;
    [HideInInspector] public bool gotCheckPoint = false;
    public Transform respawnPoint;

    void Start()
    {
        playerState = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        RespawnIfFall();
    }

    public void SetCheckpoint(Transform newPosition)
    {
        gotCheckPoint = true;
        respawnPoint = newPosition;
        respawnHP = playerState.playerHP;
        Debug.Log("Checkpoint set at: " + respawnPoint.position + " with HP: " + respawnHP);
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
