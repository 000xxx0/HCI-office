using UnityEngine;
using System.Collections.Generic;

public class PlayerAreaInteraction : MonoBehaviour
{
    public float interactionRadius = 2f;
    public DoorInteraction currentInteractable;
    private DoorInteraction lastInteractable;
    public BookInteraction currentBookInteractable;
    private BookInteraction lastBookInteractable;
    
    // Keep track of all interactable objects we've shown prompts for
    private List<DoorInteraction> activeDoorInteractables = new List<DoorInteraction>();
    private List<BookInteraction> activeBookInteractables = new List<BookInteraction>();

    void Update()
    {
        // Clear current interactables
        currentInteractable = null;
        currentBookInteractable = null;
        
        // Save lists of previous active interactables so we can hide prompts for objects no longer in range
        List<DoorInteraction> previousDoorInteractables = new List<DoorInteraction>(activeDoorInteractables);
        List<BookInteraction> previousBookInteractables = new List<BookInteraction>(activeBookInteractables);
        
        // Clear the active lists to rebuild them
        activeDoorInteractables.Clear();
        activeBookInteractables.Clear();

        // Find all interactables in range
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
        
        foreach (var hit in hits)
        {
            DoorInteraction door = hit.GetComponent<DoorInteraction>();
            if (door != null)
            {
                activeDoorInteractables.Add(door);
                if (currentInteractable == null) // Use the first door found as current
                {
                    currentInteractable = door;
                    door.ShowPrompt();
                }
            }
            
            BookInteraction book = hit.GetComponent<BookInteraction>();
            if (book != null)
            {
                activeBookInteractables.Add(book);
                if (currentBookInteractable == null) // Use the first book found as current
                {
                    currentBookInteractable = book;
                    book.ShowPrompt();
                }
            }
        }
        
        // Hide prompts for any interactables that are no longer in range
        foreach (var door in previousDoorInteractables)
        {
            if (!activeDoorInteractables.Contains(door))
            {
                door.HidePrompt();
            }
        }
        
        foreach (var book in previousBookInteractables)
        {
            if (!activeBookInteractables.Contains(book))
            {
                book.HidePrompt();
            }
        }

        // Handle input - prioritize books over doors
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBookInteractable != null)
            {
                currentBookInteractable.Interact();
            }
            else if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}