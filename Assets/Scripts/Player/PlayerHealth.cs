using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float playerHP = 100f;
    private CheckpointController CPController;

    void Start()
    {
        CPController = GetComponent<CheckpointController>();
    }

    void Update()
    {

    }

    public void TakeDamage(float damageTaken)
    {
        playerHP -= damageTaken;
        if (playerHP <= 0)
        {
            Die();
        }
        Debug.Log("Player took damage. Current HP: " + playerHP);
        // Play damage sound
        // Play damage animation
    }

    public void Die()
    {
        Debug.Log("Player died.");
        if (!CPController.gotCheckPoint)
        {
            // Play death sound and animation
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            respawnFromLastCheckpoint();
        }
    }

    public void respawnFromLastCheckpoint()
    {
        Debug.Log("Respawning at checkpoint.");
        transform.position = CPController.respawnPoint.position;
        Physics.SyncTransforms();
        playerHP = CPController.respawnHP;
        Debug.Log("Respawned with HP: " + playerHP);
    }
}
