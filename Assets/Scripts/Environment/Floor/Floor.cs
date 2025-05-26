using UnityEngine;
using TMPro;

public class FloorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text interactPrompt;
    private string targetSceneName = "DTE2";
    private KeyCode interactKey = KeyCode.E;
    private bool isInteractable = true;

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
            interactPrompt.text = $"Press {interactKey} to use lift";
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
    [SerializeField] private string targetSceneName = "DTE2";
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
                targetSceneName,
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