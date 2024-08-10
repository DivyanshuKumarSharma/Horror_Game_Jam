using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextTransition : MonoBehaviour
{
    public TextMeshProUGUI storyText; 
    public string[] storyLines;
    public float textDisplayTime = 5f;
    public bool isEnding = false;

    private int currentLineIndex = 0;
    private bool isTextDisplayed = false;
    private float timer = 0f;

    public string sceneToLoad;

    void Start()
    {
        DisplayNextLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }

        if (isTextDisplayed)
        {
            timer += Time.deltaTime;
            if (timer >= textDisplayTime)
            {
                DisplayNextLine();
            }
        }
    }

    void DisplayNextLine()
    {
        if (currentLineIndex < storyLines.Length)
        {
            storyText.text = storyLines[currentLineIndex];
            currentLineIndex++;
            timer = 0f;
            isTextDisplayed = true;
        }
        else if(isEnding)
        {
            Application.Quit();
        }
        else
        {
            StartCoroutine(LoadGameScene());
        }
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(2f); 
        SceneManager.LoadScene(sceneToLoad); 
    }
}
