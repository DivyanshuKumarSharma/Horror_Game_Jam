using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject currentObj;
    public GameObject nextObj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint trigger.");
            CheckpointController checkpointCont = other.GetComponent<CheckpointController>();
            if (checkpointCont != null)
            {
                checkpointCont.SetCheckpoint(transform);
            }
            // Disable current artwork
            if (currentObj != null)
            {
                currentObj.SetActive(false);
            }

            // Enable next artwork
            if (nextObj != null)
            {
                nextObj.SetActive(true);
            }
        }
    }
}
