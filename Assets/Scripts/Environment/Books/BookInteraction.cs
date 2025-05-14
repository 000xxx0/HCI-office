using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
public class BookInteraction : MonoBehaviour
{
    private TMP_Text interactPrompt;
    private TMP_Text bookContentText;
    private string bookFileName;
    private GameObject bookUIPanel;
    private TMP_Text bookTitleText;
    private string bookTitle;
    private string trialBookContent;
    private bool isOpen = false;
    private string[] pages;
    private int currentPage = 0;
    [SerializeField] private int charsPerPage = 800; // Adjust for your UI size

    public void SetReferences(
        TMP_Text interactPrompt,
        TMP_Text bookContentText,
        string bookFileName,
        GameObject bookUIPanel,
        TMP_Text bookTitleText,
        string bookTitle,
        string trialBookContent)
    {
        this.interactPrompt = interactPrompt;
        this.bookContentText = bookContentText;
        this.bookFileName = bookFileName;
        this.bookUIPanel = bookUIPanel;
        this.bookTitleText = bookTitleText;
        this.bookTitle = bookTitle;
        this.trialBookContent = trialBookContent;

        // Initialize UI state
        HidePrompt();
        if (bookContentText != null)
            bookContentText.text = "";
        if (bookUIPanel != null)
            bookUIPanel.SetActive(false);
    }

    public void Interact()
    {
        if (!isOpen)
        {
            OpenBook();
        }
        else
        {
            CloseBook();
        }
    }

    public void ShowPrompt()
    {
        if (interactPrompt != null && !isOpen)
            interactPrompt.text = "Press E to read book";
    }

    public void HidePrompt()
    {
        if (interactPrompt != null)
            interactPrompt.text = "";
    }

    public void NextPage()
    {
        if (pages == null || pages.Length == 0) return;
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PrevPage()
    {
        if (pages == null || pages.Length == 0) return;
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        if (bookContentText != null && pages != null && pages.Length > 0)
        {
            bookContentText.text = pages[currentPage];
            
            // Display page number if there are multiple pages
            if (pages.Length > 1)
            {
                bookContentText.text += $"\n\nPage {currentPage + 1} of {pages.Length}";
            }
        }
    }

    private void OpenBook()
    {
        isOpen = true;
        HidePrompt();
        if (bookUIPanel != null)
            bookUIPanel.SetActive(true);
        if (bookTitleText != null)
            bookTitleText.text = bookTitle;
        if (bookContentText != null)
        {
            string content = trialBookContent;
            // Split content into pages
            pages = SplitToPages(content, charsPerPage);
            currentPage = 0;
            UpdatePage();
        }
        // Optionally hide gameplay UI here
    }

    public void CloseBook()
    {
        isOpen = false;
        if (bookUIPanel != null)
            bookUIPanel.SetActive(false);
        if (bookContentText != null)
            bookContentText.text = "";
        pages = null;
        currentPage = 0;
        // Optionally show gameplay UI again here
    }

    private string[] SplitToPages(string content, int charsPerPage)
    {
        if (string.IsNullOrEmpty(content)) return new string[] { "" };
        
        // Try to split at paragraphs first
        string[] paragraphs = content.Split(new[] { "\n\n", "\r\n\r\n" }, System.StringSplitOptions.None);
        List<string> pagesList = new List<string>();
        string currentPage = "";
        
        foreach (string paragraph in paragraphs)
        {
            // If adding this paragraph would exceed the page length
            if ((currentPage + paragraph).Length > charsPerPage)
            {
                // If the current page isn't empty, add it to the pages list
                if (!string.IsNullOrEmpty(currentPage))
                {
                    pagesList.Add(currentPage);
                    currentPage = "";
                }
                
                // If the paragraph itself is longer than a page
                if (paragraph.Length > charsPerPage)
                {
                    // Split the paragraph into chunks
                    for (int i = 0; i < paragraph.Length; i += charsPerPage)
                    {
                        int length = Mathf.Min(charsPerPage, paragraph.Length - i);
                        pagesList.Add(paragraph.Substring(i, length));
                    }
                }
                else
                {
                    // Start a new page with this paragraph
                    currentPage = paragraph;
                }
            }
            else
            {
                // Add a line break if the page isn't empty
                if (!string.IsNullOrEmpty(currentPage))
                {
                    currentPage += "\n\n";
                }
                currentPage += paragraph;
            }
        }
        
        // Add the last page if it's not empty
        if (!string.IsNullOrEmpty(currentPage))
        {
            pagesList.Add(currentPage);
        }
        
        return pagesList.ToArray();
    }
}
