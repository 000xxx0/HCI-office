// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Invector;

// public class BookRaycast : MonoBehaviour
// {
//     public vThirdPersonCamera _camera;
//     [SerializeField] private string bookTitle = "Book Title"; // Optional: Book title
//     [SerializeField] private Image crosshair;
//     [SerializeField] private KeyCode interactKey;
//     [SerializeField] private float RaycastLength = 2f;
//     private BookInteraction bookInteraction;

//     void Start()
//     {
//         _camera = GetComponent<vThirdPersonCamera>();
//     }
//     private void Update()
//     {
//         // Move crosshair to mouse position only if cursor is visible and unlocked
//         if (crosshair != null)
//         {
//             if (Cursor.visible && Cursor.lockState == CursorLockMode.None)
//             {
//                 Vector2 mousePos = Input.mousePosition;
//                 crosshair.rectTransform.position = mousePos;
//             }
//             else
//             {
//                 // Center crosshair if cursor is locked (typical for third-person gameplay)
//                 crosshair.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
//             }
//         }
//         // Raycast from crosshair position
//         Vector3 crosshairScreenPos = crosshair != null ? (Vector3)crosshair.rectTransform.position : new Vector3(Screen.width / 2, Screen.height / 2, 0);
//         Ray ray = _camera.GetComponent<Camera>().ScreenPointToRay(crosshairScreenPos);
//         int bookLayer = LayerMask.NameToLayer("Ground");
//         if (Physics.Raycast(ray, out RaycastHit hit, RaycastLength))
//         {
//             Debug.Log($"Raycast hit: {hit.collider.gameObject.name} | Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)} | Tag: {hit.collider.tag}");
//             if (hit.collider.gameObject.layer == bookLayer)
//             {
//                 bookInteraction = hit.collider.GetComponent<BookInteraction>();
//                 if (bookInteraction != null)
//                 {
//                     Debug.Log("Hovering over book: " + bookInteraction.gameObject.name);
//                     crosshair.color = Color.green; // Highlight green when hovering over a book
//                     bookInteraction.SetBookTitle(bookTitle);
//                     bookInteraction.ShowPrompt();
//                     if (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown(0))
//                     {
//                         Debug.Log("Interacting with book: " + bookInteraction.gameObject.name);
//                         bookInteraction.Interact();
//                     }
//                 }
//             }
//             else
//             {
//                 crosshair.color = Color.white; // Revert to white if not hovering over a book
//                 if (bookInteraction != null)
//                 {
//                     bookInteraction.HidePrompt();
//                 }
//             }
//         }
//         else
//         {
//             crosshair.color = Color.white; // Also revert to white if raycast hits nothing
//             if (bookInteraction != null)
//             {
//                 bookInteraction.HidePrompt();
//             }
//         }
//     }

// }