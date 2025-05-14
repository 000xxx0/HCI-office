using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Door : MonoBehaviour
{
    public string doorLayerName = "Door";
    public TMP_Text interactPrompt; // Drag your UI Text here in the Inspector

    void Start()
    {
        int doorLayer = LayerMask.NameToLayer(doorLayerName);
        if (doorLayer == -1)
        {
            Debug.LogWarning($"Layer '{doorLayerName}' does not exist.");
            return;
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == doorLayer)
            {
                if (obj.GetComponent<Collider>() == null)
                {
                    obj.AddComponent<BoxCollider>();
                }

                DoorInteraction interaction = obj.GetComponent<DoorInteraction>();
                if (interaction == null)
                {
                    interaction = obj.AddComponent<DoorInteraction>();
                }
                interaction.SetPrompt(interactPrompt);
                count++;
            }
        }

        if (count == 0)
        {
            Debug.LogWarning($"No GameObjects found on layer '{doorLayerName}'.");
        }
    }
}