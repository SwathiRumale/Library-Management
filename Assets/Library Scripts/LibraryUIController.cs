using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pixelplacement;
using UnityEngine.UI;

public class LibraryUIController : Singleton<LibraryUIController>
{
    [Header("UI Main Panels")]
    public List<GameObject> HomeScreenPanel;
    public GameObject BorrowPanel;
    public GameObject AddPanel;

    [Header("Other Screens Apart From Home Screen")]
    public List<GameObject> OtherScreens;

    [Header("Notification Panels")]
    public GameObject BorrowNotificationPanel;
    public GameObject ReturnNotificationPanel;

    [Header("Parent for Books")]
    public Transform ParentForBookPrefabs;

    [Header("Prefabs for Books")]
    public BookController Comics;
    public BookController Action;
    public BookController Fiction;
    public BookController NonFiction;

    [Header("Add New Book Text Box")]
    public Text AddName;
    public string BookCategoryDropdown { get; set; }
    public int DropDownValue;
    public Text AddAuthorName;
    public Text AddLanguage;
    public Text AddYear;
    public Text AddPages;
    public Text AddIntroductin;

    [Header("Warning Message for Introduction")]
    public Text WarningMessage;

    [Header("Book Ratings and Details")]
    public Text BookName;
    public Text AuthorName;
    public Text Rating;
    public GameObject Stars;
    public Transform StarsParent;
    public Text Pages;
    public Text Language;
    public Text Year;
    public Text Introduction;
    public Color[] BookBackgroundColour;

    [Header("Books Borrowed")]
    public BooksBorrowed BooksBorrowedPrefab;
    public Transform BooksBorrowedTransform;
    public Button YesBorrow;
    public Button NoBorrow;
    public Text BookBorrowNameText;

    [Header("Books Return")]
    public Button YesReturn;
    public Button Noreturn;
    public int CurrentReturnID;
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
    public int CurrentBookID;
    public string CurrentAuthorName;
    public string CurrentCategoryname;
    public string CurrentBookName;
    public Image CurrentBookIcon;
    public BookController CurrentbooksBorrowed;

    public List<BooksBorrowed> BorrowedBooks = new List<BooksBorrowed>();

    //Events and Delegates Reference
    public delegate void OnBorrowButtonYesClicked();
    public event OnBorrowButtonYesClicked onBorrowButtonYesClicked;

    //*************************** UI Panels and Button Control Starts****************//
    public List<BooksBorrowed> bo = new List<BooksBorrowed>();


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
        BorrowPanel.SetActive(true);
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
        AddPanel.SetActive(true);
    }

    public void OnEnterSelect()
    {
        AddPanel.SetActive(false);
        GotoHomeScreen();
        //LibraryManagement.Instance.GetBookList();
    }

    public bool isMinimumWordsInIntroduction()
    {
        if (AddIntroductin.text.Length < 100)
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
        WarningMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        WarningMessage.gameObject.SetActive(false);
    }

    public void SetDropDownValue(Dropdown dropdown)
    {
        Debug.Log("Drop Down" + dropdown.options[dropdown.value].text);
        BookCategoryDropdown = dropdown.options[dropdown.value].text;
    }

    public void InstantiateStar(int count)
    {
        //ResetStars();
        for(int i = 0; i < count; i++)
        {
            Instantiate(Stars, StarsParent);
        }
    }

    private void ResetStars()
    {
        foreach(Transform star in StarsParent)
        {
            DestroyImmediate(star.gameObject);
        }
    }

    public void OnYesBorrow()
    {
        //if (onBorrowButtonYesClicked != null)
        //{
        //    onBorrowButtonYesClicked();
        //    Debug.Log("onBorrowButtonYesClicked invoke");
        //}
        //BooksBorrowed  booksBorrowed = Instantiate(BooksBorrowedPrefab, BooksBorrowedTransform);

        // BorrowedBooks.Add(booksBorrowed);
        Destroy(CurrentbooksBorrowed.gameObject);
        GotoHomeScreen();
        Debug.Log("Destroyed" + CurrentbooksBorrowed);
        InstantiateBooksBorrowed();
        //BorrowPanel.SetActive(false);
        //LibraryManagement.Instance.GetBookList();
    }

    public void InstantiateBooksBorrowed()
    {
            BooksBorrowed booksBorrowed = Instantiate(BooksBorrowedPrefab, BooksBorrowedTransform);
            booksBorrowed.Author.text = CurrentAuthorName;
            booksBorrowed.Name.text = CurrentBookName;
            booksBorrowed.Category.text = CurrentCategoryname;
            booksBorrowed.Id = CurrentReturnID;
            BorrowedBooks.Add(booksBorrowed);
            SetSprite(booksBorrowed.CategoryIcon, booksBorrowed.Category.text);
    }

    public void ShowBorrowedList()
    {
        foreach(var booksborrowed in BorrowedBooks)
        {
            Instantiate(booksborrowed, BooksBorrowedTransform);
        }
    }


    public void OnYesReturn()
    {
        //LibraryManagement.Instance.DeleteBook(CurrentReturnID);
        foreach (var book in BorrowedBooks)
        {
            if(book.Id == CurrentReturnID)
            {
                BooksList.Remove(book.Id);
                Debug.Log("removed-------");
                BorrowedBooks.Remove(book);
                Destroy(book.gameObject);
                break;
            }
        }
        //BooksList.Remove()
        LibraryManagement.Instance.GetBookList();
    }
    //*************************** UI Panels and Button Control ends****************//




    //*************************** Books Prefab Instantiate and assign text start **********************//


    public void InstantiateBook(BookDetails[] bookDetails)
    {
        Debug.Log("InstantiateBook");
        foreach (var book in bookDetails)
        {
            if (!BooksList.Contains(book.id))
            {
                Debug.Log("returned without instatiation");
                //  return;
                //}
                Debug.Log("Book.name" + book.Name);
                if (book.Name != "")
                {
                    //Debug.Log("Book.Category ->" + book.Category);
                    switch (book.Category)
                    {
                        case "NonFiction":
                            BookController nonfiction = Instantiate(NonFiction, ParentForBookPrefabs);
                            nonfiction.NameOfThisBook.text = book.Name;
                            nonfiction.CategoryOfThisBook.text = book.Category;
                            //non.bookDetails.Name = book.Name;
                            nonfiction.bookDetails = book;
                            BooksList.Add(book.id);
                            break;
                        case "Fiction":
                            BookController fiction = Instantiate(Fiction, ParentForBookPrefabs);
                            fiction.NameOfThisBook.text = book.Name;
                            fiction.CategoryOfThisBook.text = book.Category;
                            fiction.bookDetails = book;
                            BooksList.Add(book.id);
                            break;

                        case "Comics":
                            BookController comics = Instantiate(Comics, ParentForBookPrefabs);
                            comics.NameOfThisBook.text = book.Name;
                            comics.CategoryOfThisBook.text = book.Category;
                            comics.bookDetails = book;
                            BooksList.Add(book.id);
                            break;

                        case "Action":
                            BookController action = Instantiate(Action, ParentForBookPrefabs);
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
        BookName.text = "";
        AuthorName.text = "";
        //Rating.text = book.Rating;
        Pages.text = "";
        Language.text = "";
        Introduction.text = "";
        //BookBackgroundColour =  
    }

    public void Reset()
    {
        ResetStars();
        ResetFullScreenBorrowPanel();
    }
    //*************************** Books Prefab Instantiate and assign text ends **********************//
}

