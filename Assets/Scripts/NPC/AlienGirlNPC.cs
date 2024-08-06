using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienGirlNPC : MonoBehaviour
{
    public Animator npcAnimator; 
    public string revealAnimationName; 
    public string endCreditsSceneName;
    private PlayerInteractUI playerUI;
    private AudioSource audioSrc;

    private void Start()
    {
        npcAnimator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerUI = player.GetComponent<PlayerInteractUI>();
        }
    }

    public void OnDialoguesExhausted()
    {
        playerUI.enabled = false;
        StartCoroutine(PlayRevealAnimationAndLoadScene());
    }

    private IEnumerator PlayRevealAnimationAndLoadScene()
    {
        if (npcAnimator != null)
        {
            npcAnimator.SetTrigger(revealAnimationName);
        }

        if(audioSrc != null)
        {
            audioSrc.Play();
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(endCreditsSceneName);
    }
}
