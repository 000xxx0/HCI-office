using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BookIterator : MonoBehaviour
{
    public TMP_Text interactPrompt;
    [SerializeField] private string targetSceneName = "BookScene";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    void Start()
    {
        SetupAllBooks();
        EnsureSceneManagerExists();
    }

    private void SetupAllBooks()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Book");
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            BookInteraction bookInteraction = obj.GetComponent<BookInteraction>();
            if (bookInteraction == null)
            {
                bookInteraction = obj.AddComponent<BookInteraction>();
            }
            
            if (obj.GetComponent<Collider>() == null)
            {
                BoxCollider collider = obj.AddComponent<BoxCollider>();
                collider.isTrigger = false;
                collider.size = new Vector3(0.3f, 0.4f, 0.1f);
            }

            bookInteraction.SetupInteraction(
                interactPrompt,
                targetSceneName,
                interactKey
            );
            
            count++;
        }

        if (count == 0)
        {
            Debug.LogWarning($"No GameObjects found with tag 'Book'.");
        }
        else
        {
            Debug.Log($"Successfully set up {count} interactive objects.");
        }
    }
    
    private void EnsureSceneManagerExists()
    {
        if (SceneManager.Instance == null)
        {
            GameObject sceneManagerObj = new GameObject("SceneManager");
            sceneManagerObj.AddComponent<SceneManager>();
            Debug.Log("Created new SceneManager instance");
        }
    }
}