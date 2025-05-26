using UnityEngine;
using System.Collections.Generic;

public class PlayerAreaInteraction : MonoBehaviour
{
    public float interactionRadius = 2f;
    
    // Interaction objects
    public DoorInteraction currentDoorInteractable;
    public BookInteraction currentBookInteractable;
    public FloorInteraction currentFloorInteractable;
    
    // Keep track of all interactable objects
    private List<DoorInteraction> activeDoorInteractables = new List<DoorInteraction>();
    private List<BookInteraction> activeBookInteractables = new List<BookInteraction>();
    private List<FloorInteraction> activeFloorInteractables = new List<FloorInteraction>();

    // For distance-based selection
    private Transform nearestInteractable = null;
    private float nearestDistance = float.MaxValue;

    void Update()
    {
        // Clear current interactables
        currentDoorInteractable = null;
        currentBookInteractable = null;
        currentFloorInteractable = null;
        
        // Save lists of previous active interactables
        List<DoorInteraction> previousDoorInteractables = new List<DoorInteraction>(activeDoorInteractables);
        List<BookInteraction> previousBookInteractables = new List<BookInteraction>(activeBookInteractables);
        List<FloorInteraction> previousFloorInteractables = new List<FloorInteraction>(activeFloorInteractables);
        
        // Clear the active lists to rebuild them
        activeDoorInteractables.Clear();
        activeBookInteractables.Clear();
        activeFloorInteractables.Clear();

        // Reset nearest tracking
        nearestInteractable = null;
        nearestDistance = float.MaxValue;

        // Find all interactables in range
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
        
        foreach (var hit in hits)
        {
            // Calculate distance to this object
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            
            // Check for doors
            DoorInteraction door = hit.GetComponent<DoorInteraction>();
            if (door != null)
            {
                activeDoorInteractables.Add(door);
                
                // Track if this is the closest interactable
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestInteractable = hit.transform;
                    currentDoorInteractable = door;
                }
            }
            
            // Check for books
            BookInteraction book = hit.GetComponent<BookInteraction>();
            if (book != null)
            {
                activeBookInteractables.Add(book);
                
                // Track if this is the closest interactable
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestInteractable = hit.transform;
                    currentDoorInteractable = null; // Clear door if book is closer
                    currentBookInteractable = book;
                }
            }
            
            // Check for floor/lift interactions
            FloorInteraction floor = hit.GetComponent<FloorInteraction>();
            if (floor != null)
            {
                activeFloorInteractables.Add(floor);
                
                // Track if this is the closest interactable
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestInteractable = hit.transform;
                    currentDoorInteractable = null; // Clear door if floor is closer
                    currentBookInteractable = null; // Clear book if floor is closer
                    currentFloorInteractable = floor;
                }
            }
        }
        
        // Hide prompts for any interactables that are no longer the nearest
        foreach (var door in previousDoorInteractables)
        {
            if (door != currentDoorInteractable)
            {
                door.HidePrompt();
            }
        }
        
        foreach (var book in previousBookInteractables)
        {
            if (book != currentBookInteractable)
            {
                book.HidePrompt();
            }
        }
        
        foreach (var floor in previousFloorInteractables)
        {
            if (floor != currentFloorInteractable)
            {
                floor.HidePrompt();
            }
        }
        
        // Show prompt only for the nearest interactable
        if (currentBookInteractable != null)
        {
            currentBookInteractable.ShowPrompt();
        }
        else if (currentFloorInteractable != null)
        {
            currentFloorInteractable.ShowPrompt();
        }
        else if (currentDoorInteractable != null)
        {
            currentDoorInteractable.ShowPrompt();
        }

        // Handle input with priority: Book > Floor > Door
        if (Input.GetKeyDown(KeyCode.E))
        {
            float minimumInteractionDistance = 3.0f; // Adjust as needed
            
            if (currentBookInteractable != null && nearestDistance <= minimumInteractionDistance)
            {
                currentBookInteractable.Interact();
            }
            else if (currentFloorInteractable != null && nearestDistance <= minimumInteractionDistance)
            {
                currentFloorInteractable.Interact();
            }
            else if (currentDoorInteractable != null && nearestDistance <= minimumInteractionDistance)
            {
                currentDoorInteractable.Interact();
            }
        }
        
        // Debug section - ADD THIS
        foreach (var hit in hits)
        {
            if (hit.GetComponent<DoorInteraction>() != null)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                Debug.Log($"Door detected: {hit.name} at distance {Vector3.Distance(transform.position, hit.transform.position)}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}