using UnityEngine;
using TMPro;

public class FloorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text interactPrompt;
    private string targetSceneName;
    private KeyCode interactKey = KeyCode.E;
    private bool isInteractable = true;

    void Start()
    {
        // This will be called after SetupInteraction
        UpdateTargetBasedOnCurrentScene();
    }

    public void SetupInteraction(TMP_Text prompt, string defaultSceneName, KeyCode interactKey)
    {
        this.interactPrompt = prompt;
        this.targetSceneName = defaultSceneName;
        this.interactKey = interactKey;
        
        // Update the target scene if we're already in the scene specified
        UpdateTargetBasedOnCurrentScene();
    }

    private void UpdateTargetBasedOnCurrentScene()
    {
        // Get current scene name
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        // If we're in DTE2, set target to DTE
        if (currentScene == "DTE2")
        {
            targetSceneName = "DTE";
        }
        // If we're in DTE, set target to DTE2
        else if (currentScene == "DTE")
        {
            targetSceneName = "DTE2";
        }
        // For any other scene, keep the default
    }

    public void Interact()
    {
        if (isInteractable)
        {
            Debug.Log($"Lift activated. Loading scene: {targetSceneName}");
            
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
            // Get the floor name from the target scene
            string floorName = targetSceneName == "DTE" ? "First Floor" : "Second Floor";
            interactPrompt.text = $"Press {interactKey} to use lift to {floorName}";
        }
    }

    public void HidePrompt()
    {
        if (interactPrompt != null)
            interactPrompt.text = "";
    }
}

public class FloorIterator : MonoBehaviour
{
    public TMP_Text interactPrompt;
    [SerializeField] private string defaultSceneName = "DTE2";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    void Start()
    {
        SetupAllLifts();
        EnsureSceneManagerExists();
    }

    private void SetupAllLifts()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Lift");
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            FloorInteraction floorInteraction = obj.GetComponent<FloorInteraction>();
            if (floorInteraction == null)
            {
                floorInteraction = obj.AddComponent<FloorInteraction>();
            }
            
            if (obj.GetComponent<Collider>() == null)
            {
                BoxCollider collider = obj.AddComponent<BoxCollider>();
                collider.isTrigger = false;
                collider.size = new Vector3(1f, 0.1f, 1f);
            }

            floorInteraction.SetupInteraction(
                interactPrompt,
                defaultSceneName,
                interactKey
            );
            
            count++;
        }

        if (count == 0)
        {
            Debug.LogWarning($"No GameObjects found with tag 'Lift'.");
        }
        else
        {
            Debug.Log($"Successfully set up {count} lift objects.");
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