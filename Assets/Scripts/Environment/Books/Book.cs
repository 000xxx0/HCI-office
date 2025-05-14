using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    public TMP_Text interactPrompt;
    public TMP_Text bookContentText;
    public string bookFileName = "book.txt";
    public string bookTitle = "Book Title";
    [TextArea(5, 20)] public string trialBookContent = "This is a trial book.\nYou can fill this string with any content you want to display in the book.\nAdd more lines to simulate multiple pages!";
    public GameObject bookUIPanel;
    public TMP_Text bookTitleText;
    private BookInteraction bookInteraction;

    void Awake()
    {
        // Add the BookInteraction component if it doesn't exist
        bookInteraction = GetComponent<BookInteraction>();
        if (bookInteraction == null)
        {
            bookInteraction = gameObject.AddComponent<BookInteraction>();
        }

        // Set up the BookInteraction with references - do this in Awake to ensure it's ready
        SetupInteraction();
    }

    void Start()
    {
        // Ensure all book objects in the scene have the proper setup
        SetupAllBooks();
    }

    private void SetupInteraction()
    {
        if (bookInteraction != null)
        {
            bookInteraction.SetReferences(
                interactPrompt,
                bookContentText,
                bookFileName,
                bookUIPanel,
                bookTitleText,
                bookTitle,
                trialBookContent
            );
        }
    }

    private void SetupAllBooks()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Book");
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            Book book = obj.GetComponent<Book>();
            if (book != null)
            {
                book.interactPrompt = interactPrompt;
                book.bookContentText = bookContentText;
                book.bookUIPanel = bookUIPanel;
                book.bookTitleText = bookTitleText;
                
                // Ensure the BookInteraction is properly set up
                BookInteraction interaction = obj.GetComponent<BookInteraction>();
                if (interaction != null)
                {
                    book.SetupInteraction();
                }
                count++;
            }
        }

        if (count == 0)
        {
            Debug.LogWarning($"No GameObjects found with tag 'Book'.");
        }
    }
}