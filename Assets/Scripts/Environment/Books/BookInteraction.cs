using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class BookInteraction : MonoBehaviour, IInteractable
{
    private TMP_Text interactPrompt;

    private string targetSceneName;
    private KeyCode interactKey;
    private bool isInteractable = true;

    public void SetReferences(
        TMP_Text interactPrompt,
        GameObject bookUIPanel)
    {
        this.interactPrompt = interactPrompt;
        HidePrompt();
    }

    public void SetupInteraction(TMP_Text prompt, string sceneName, KeyCode interactKey)
    {
        this.interactPrompt = prompt;
        this.targetSceneName = sceneName;
        this.interactKey = interactKey;
    }

    public void Interact()
    {
        if (isInteractable)
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.LoadScene(targetSceneName);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
                Debug.LogWarning("Custom SceneManager not found, using Unity's SceneManager instead.");
            }
        }
    }

    public void ShowPrompt()
    {
        if (interactPrompt != null)
        {
            if (isInteractable)
            {
                interactPrompt.text = $"Press {interactKey} to interact";
            }
            else
            {
                interactPrompt.text = "Press E to read book";
            }
        }
    }

    public void HidePrompt()
    {
        if (interactPrompt != null)
            interactPrompt.text = "";
    }


}