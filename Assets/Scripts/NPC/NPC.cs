using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [HideInInspector] public float health = 100f;
    public string interactText;
    public string[] dialogues;
    private DialogueSystem dialogueSystem;
    [HideInInspector] public bool isStarted = false;
    public float rotationSpeed = 2f;

    private AlienGirlNPC alienGirl;

    void Start()
    {
        // Ensure the DialogueSystem is set up properly
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GameObject dialogueManager = player.transform.Find("DialogueManager")?.gameObject;
            if (dialogueManager != null)
            {
                dialogueSystem = dialogueManager.GetComponent<DialogueSystem>();
            }
        }

        // Get the GirlfriendNPC component if it exists in the scene
        alienGirl = GetComponent<AlienGirlNPC>();
    }


    void Update()
    {
        // Optionally handle any updates needed for NPC here
    }

    public void Interact(Transform interacterTransform)
    {
        if (dialogueSystem == null)
        {
            Debug.LogWarning("DialogueSystem is not assigned.");
            return;
        }

        Vector3 direction = (interacterTransform.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction * Time.deltaTime * rotationSpeed);

        if (!isStarted)
        {
            dialogueSystem.StartDialogue(dialogues);
            isStarted = true;
        }
        else if (!dialogueSystem.isDone)
        {
            dialogueSystem.DisplayNextSentence();

            if (dialogueSystem.isDone)
            {
                isStarted = false;
                // Call the method to handle the reveal and scene transition
                if (alienGirl != null)
                {
                    alienGirl.OnDialoguesExhausted();
                    enabled = false;
                }
            }
        }
    }

    public string getInteractText()
    {
        return interactText;
    }

    public void EndInteraction()
    {
        isStarted = false;
    }

    public void UpdateDialogues(string[] newDialogues)
    {
        dialogues = newDialogues;
    }
}