using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pixelplacement;
using UnityEngine.UI;

public class LibraryUIController : Singleton<LibraryUIController>
{
    [Header("MAIN PANELS")]
    public List<GameObject> HomeScreenPanel;
    public Transform FullScreenBorrowPanel;
    public Transform FullScreenAddNewBooksPanel;

    [Header("OTHER SCREENS (Apart From Home Screen)")]
    public List<GameObject> OtherScreens;

    [Header("NOTIFICATION PANELS")]
    public GameObject BorrowNotificationPanel;
    public GameObject ReturnNotificationPanel;

    [Header("CONTENT FOR BOOK COLLECTION")]
    public Transform BookCollectionContent;

    [Header("PREFABS FOR BOOKS COLLECTION")]
    public BookController Comics;
    public BookController Action;
    public BookController Fiction;
    public BookController NonFiction;

    [Header("TEXT REFERENCES - ADD NEW BOOK")]
    public Text AddNewBookNameText;
    public string BookCategoryDropdown { get; set; }
    public int DropDownValue;
    public Text AddNewAuthorText;
    public Text AddNewLanguageText;
    public Text AddNewYearText;
    public Text AddNewPagesText;
    public Text AddNewDescription;

    [Header("TEXT REFERENCES - WARNING MESSAGE")]
    public Text WarningText;

    [Header("TEXT REFERENCES - BOOK RATING & DETAILS")]
    public Text BookNameText;
    public Text AuthorNameText;
    public Text RatingText;
    public GameObject StarsPrefab;
    public Transform StarsParent;
    public Text PagesText;
    public Text LanguageText;
    public Text YearText;
    public Text DescriptionText;

    [Header("BOOKS BORROWED PANEL REFERENCES")]
    public BooksBorrowed BooksBorrowedPrefab;
    public Transform BooksBorrowedContent;
    public Button YesBorrow;
    public Button NoBorrow;
    public Text BookBorrowNameText;

    [Header("BOOKS RETURN PANEL REFERENCES")]
    public Button YesReturn;
    public Button Noreturn;
    public int CurrentReturnID { get; set; }
    public Text BookReturnNameText;

    [Header("Books Sprite for various Categories")]
    public Sprite ComicsSprite;
    public Sprite FictionSprite;
    public Sprite NonFictionSprite;
    public Sprite ActionSprite;

    [Header("Public Variables for reference used in methods")]
    public string DropdownCategoryName;
    public int SelectID;
    public List<int> BooksList;
    public int CurrentBookID { get; set; }
    public string CurrentAuthorName { get; set; }
    public string CurrentCategoryname { get; set; }
    public string CurrentBookName { get; set; }
    public Image CurrentBookIcon;
    public BookController CurrentbooksBorrowed { get; set; }

    public List<BooksBorrowed> BorrowedBooks = new List<BooksBorrowed>();

    //Events and Delegates Reference
    public delegate void OnBorrowButtonYesClicked();
    public event OnBorrowButtonYesClicked onBorrowButtonYesClicked;

    //*************************** UI Panels and Button Control Starts****************//

    public void OnEnable()
    {
        foreach (var panels in OtherScreens)
        {
            panels.SetActive(false);
        }
    }
    private void Start()
    {
        BookCategoryDropdown = "Action";
    }
    /// <summary>
    /// To show the Borrow Panel which includes Rating, Lang, Year etc.
    /// </summary>
    public void ShowBorrowPanel()
    {
        FullScreenBorrowPanel.gameObject.SetActive(true);
    }
    /// <summary>
    /// To back to Home Screen
    /// </summary>
    public void GotoHomeScreen()
    {
        Reset();
        foreach (var homescreen in HomeScreenPanel)
        {
            homescreen.SetActive(true);
        }
        foreach (var panels in OtherScreens)
        {
            panels.SetActive(false);
        }
    }

    public void OnBorrowBookSelect()
    {
        BorrowNotificationPanel.SetActive(true);
        BookBorrowNameText.text = CurrentBookName;
    }

    public void OnReturnBookSelect()
    {
        ReturnNotificationPanel.SetActive(true);
    }

    public void OnAddButtonSelected()
    {
        FullScreenAddNewBooksPanel.gameObject.SetActive(true);
    }

    public void OnEnterSelect()
    {
        FullScreenAddNewBooksPanel.gameObject.SetActive(false);
        GotoHomeScreen();
    }

    public bool isMinimumWordsInIntroduction()
    {
        if (AddNewDescription.text.Length < 100)
        {
            return true;
        }
        else
        {
            StartCoroutine(ShowWarningMessageForTwoSecond());
            return false;
        }
    }

    IEnumerator ShowWarningMessageForTwoSecond()
    {
        WarningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        WarningText.gameObject.SetActive(false);
    }

    public void SetDropDownValue(Dropdown dropdown)
    {
        //Debug.Log("Drop Down" + dropdown.options[dropdown.value].text);
        BookCategoryDropdown = dropdown.options[dropdown.value].text;
    }

    public void InstantiateStar(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(StarsPrefab, StarsParent);
        }
    }

    private void ResetStars()
    {
        foreach (Transform star in StarsParent)
        {
            DestroyImmediate(star.gameObject);
        }
    }

    public void OnYesBorrow()
    {
        Destroy(CurrentbooksBorrowed.gameObject);
        GotoHomeScreen();
        InstantiateBooksBorrowed();
    }

    public void InstantiateBooksBorrowed()
    {
        BooksBorrowed booksBorrowed = Instantiate(BooksBorrowedPrefab, BooksBorrowedContent);
        booksBorrowed.Author.text = CurrentAuthorName;
        booksBorrowed.Name.text = CurrentBookName;
        booksBorrowed.Category.text = CurrentCategoryname;
        booksBorrowed.Id = CurrentReturnID;
        BorrowedBooks.Add(booksBorrowed);
        SetSprite(booksBorrowed.CategoryIcon, booksBorrowed.Category.text);
    }

    public void ShowBorrowedList()
    {
        foreach (var booksborrowed in BorrowedBooks)
        {
            Instantiate(booksborrowed, BooksBorrowedContent);
        }
    }


    public void OnYesReturn()
    {
        foreach (var book in BorrowedBooks)
        {
            if (book.Id == CurrentReturnID)
            {
                BooksList.Remove(book.Id);
                BorrowedBooks.Remove(book);
                Destroy(book.gameObject);
                break;
            }
        }
        LibraryManager.Instance.GetBookList();
    }
    //*************************** UI Panels and Button Control ends****************//




    //*************************** Books Prefab Instantiate and assign text start **********************//


    public void InstantiateBook(BookDetails[] bookDetails)
    {
        //Debug.Log("InstantiateBook");
        foreach (var book in bookDetails)
        {
            if (!BooksList.Contains(book.id))
            {
                if (book.Name != "")
                {
                    switch (book.Category)
                    {
                        case "NonFiction":
                            BookController nonfiction = Instantiate(NonFiction, BookCollectionContent);
                            nonfiction.NameOfThisBook.text = book.Name;
                            nonfiction.CategoryOfThisBook.text = book.Category;
                            nonfiction.bookDetails = book;
                            BooksList.Add(book.id);
                            break;
                        case "Fiction":
                            BookController fiction = Instantiate(Fiction, BookCollectionContent);
                            fiction.NameOfThisBook.text = book.Name;
                            fiction.CategoryOfThisBook.text = book.Category;
                            fiction.bookDetails = book;
                            BooksList.Add(book.id);
                            break;

                        case "Comics":
                            BookController comics = Instantiate(Comics, BookCollectionContent);
                            comics.NameOfThisBook.text = book.Name;
                            comics.CategoryOfThisBook.text = book.Category;
                            comics.bookDetails = book;
                            BooksList.Add(book.id);
                            break;

                        case "Action":
                            BookController action = Instantiate(Action, BookCollectionContent);
                            action.NameOfThisBook.text = book.Name;
                            action.CategoryOfThisBook.text = book.Category;
                            action.bookDetails = book;
                            BooksList.Add(book.id);
                            break;
                    }
                }
            }
        }
    }

    public void SetSprite(Image bookImage, string categoryName)
    {
        switch (categoryName)
        {
            case "Fiction":
                bookImage.sprite = FictionSprite;
                break;
            case "NonFiction":
                bookImage.sprite = NonFictionSprite;
                break;
            case "Comics":
                bookImage.sprite = ComicsSprite;
                break;
            case "Action":
                CurrentBookIcon.sprite = ActionSprite;
                break;
            default:
                break;
        }
    }

    public void ResetFullScreenBorrowPanel()
    {
        BookNameText.text = "";
        AuthorNameText.text = "";
        PagesText.text = "";
        LanguageText.text = "";
        DescriptionText.text = "";
    }

    public void ResetFullScreenAddNewBooksPanel()
    {
        //aDebug.Log("ResetFullScreenAddNewBooksPanel");
        AddNewAuthorText.transform.parent.GetComponent<InputField>().text = "";
        AddNewBookNameText.transform.parent.GetComponent<InputField>().text = "";
        AddNewDescription.transform.parent.GetComponent<InputField>().text = "";
        AddNewLanguageText.transform.parent.GetComponent<InputField>().text = "";
        AddNewYearText.transform.parent.GetComponent<InputField>().text = "";
        AddNewPagesText.transform.parent.GetComponent<InputField>().text = "";
        RatingText.transform.parent.GetComponent<InputField>().text = "";

    }

    public void Reset()
    {
        ResetStars();
        ResetFullScreenBorrowPanel();
        ResetFullScreenAddNewBooksPanel();
    }

    //*************************** Books Prefab Instantiate and assign text ends **********************//
}

