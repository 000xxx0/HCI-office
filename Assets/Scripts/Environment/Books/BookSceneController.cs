using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
// Add this using statement for built-in Unity SceneManager
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class BookSceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text bookTitleText;
    [SerializeField] private TMP_Text bookContentText;
    [SerializeField] private Button closeButton;
    [SerializeField] private string mainSceneName = "DTE2";
    
    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseBook);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBook();
        }
    }
    
    
    public void CloseBook()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadScene(mainSceneName);
        }
        else
        {
            UnitySceneManager.LoadScene(mainSceneName);
        }
    }
    
}