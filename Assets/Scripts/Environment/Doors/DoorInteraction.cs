using UnityEngine;
using TMPro;
using Invector.vCharacterController;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private Transform teleportDestination; // Drag the destination here in the Inspector

    public void SetPrompt(TMP_Text prompt)
    {
        interactPrompt = prompt;
    }

    public void SetTeleportDestination(Transform destination)
    {
        teleportDestination = destination;
    }

    void Start()
    {
        HidePrompt();
    }

    public void Interact()
    {
        Debug.Log("Door interacted with!");
        HidePrompt();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Disable Invector vThirdPersonController if present
            var invectorController = player.GetComponent<Invector.vCharacterController.vThirdPersonController>();
            if (invectorController != null)
                invectorController.enabled = false;

            // Optional: Disable Rigidbody if present
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            float distanceInFront = 1f; // Adjust as needed

            // Calculate direction from door to player, flatten on Y axis
            Vector3 doorToPlayer = (player.transform.position - transform.position);
            doorToPlayer.y = 0;
            Vector3 direction = doorToPlayer.normalized;
            if (direction == Vector3.zero)
                direction = transform.forward;
            else
                direction = -direction; // Invert direction to always cross to the other side

            // Teleport to the other side of the door
            Vector3 newPosition = transform.position + direction * distanceInFront;
            newPosition.y = player.transform.position.y;

            player.transform.position = newPosition;
            player.transform.rotation = transform.rotation;

            // Optional: Re-enable Rigidbody
            if (rb != null)
                rb.isKinematic = false;

            // Re-enable Invector vThirdPersonController
            if (invectorController != null)
                invectorController.enabled = true;
        }
    }

    public void ShowPrompt()
    {
        if (interactPrompt != null)
            interactPrompt.text = "Press E to interact with door";
    }

    public void HidePrompt()
    {
        if (interactPrompt != null)
            interactPrompt.text = "";
    }
}