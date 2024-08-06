using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactUIContainer;
    [SerializeField] private PlayerInteraction playerInteract;
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private Transform npcStand;
    [SerializeField] private TextMeshProUGUI interactText;
    // Start is called before the first frame update
    void Start()
    {
        playerInteract = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInteract.GetInteractable() != null)
        {
            npcStand.gameObject.SetActive(false);
            if (!dialogueSystem.alreadyStarted) {
                show(playerInteract.GetInteractable());
            }
            else
            {
                hide();
            }

        }
        else
        {
            npcStand.gameObject.SetActive(true);
            dialogueSystem.EndDialogue();
            hide();
        }
    }

    private void show(IInteractable interactable)
    {
        interactUIContainer.SetActive(true);
        interactText.text = interactable.getInteractText();
    }

    public void hide() {
        interactUIContainer.SetActive(false);
    }

}
