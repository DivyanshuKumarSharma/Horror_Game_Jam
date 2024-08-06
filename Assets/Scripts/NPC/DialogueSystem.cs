using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel; 
    public TextMeshProUGUI dialogueText; 
    private Queue<string> sentences;
    [HideInInspector] public bool isDone = false;
    [HideInInspector] public bool alreadyStarted = false;

    void Start() 
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); 
    }

    public void StartDialogue(string[] dialogue)
    {
        alreadyStarted = true;
        isDone = false;
        sentences.Clear();

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        dialoguePanel.SetActive(true); 
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        isDone = true;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        alreadyStarted = false;
    }

}