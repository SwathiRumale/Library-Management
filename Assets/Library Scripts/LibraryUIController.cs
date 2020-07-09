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

    [Header("Public Variables for reference used in methods")]
    public string DropdownCategoryName;
    public int SelectID;
    public List<int> BooksList;
    public int BookID;
    

    //*************************** UI Panels and Button Control Starts****************//



    public void OnEnable()
    {
        foreach (var panels in OtherScreens)
        {
            panels.SetActive(false);
        }
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
        BookCategoryDropdown = dropdown.options[dropdown.value].text;
    }


    //*************************** UI Panels and Button Control ends****************//




    //*************************** Books Prefab Instantiate and assign text start **********************//


    public void InstantiateBook(BookDetails[] bookDetails)
    {
        foreach (var book in bookDetails)
        {
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
                        break;
                    case "Fiction":
                        BookController fiction = Instantiate(Fiction, ParentForBookPrefabs);
                        fiction.NameOfThisBook.text = book.Name;
                        fiction.CategoryOfThisBook.text = book.Category;
                        fiction.bookDetails = book;
                        break;

                    case "Comics":
                        BookController comics = Instantiate(Comics, ParentForBookPrefabs);
                        comics.NameOfThisBook.text = book.Name;
                        comics.CategoryOfThisBook.text = book.Category;
                        comics.bookDetails = book;
                        break;

                    case "Action":
                        BookController action = Instantiate(Action, ParentForBookPrefabs);
                        action.NameOfThisBook.text = book.Name;
                        action.CategoryOfThisBook.text = book.Category;
                        action.bookDetails = book;
                        break;
                }
            }
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

    //*************************** Books Prefab Instantiate and assign text ends **********************//
}

