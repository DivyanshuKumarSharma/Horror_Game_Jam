using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienGirlNPC : MonoBehaviour
{
    public Animator npcAnimator; 
    public string revealAnimationName; 
    public string endCreditsSceneName;
    private PlayerInteractUI playerUI;

    private void Start()
    {
        npcAnimator = GetComponent<Animator>();
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

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(endCreditsSceneName);
    }
}
