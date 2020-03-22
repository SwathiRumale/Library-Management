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
    public GameObject Comics;
    public GameObject Action;
    public GameObject Fiction;
    public GameObject NonFiction;

    [Header("Add New Book Text Box")]
    public Text AddName;
    public Dropdown AddCategory;
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

    private void Update()
    {
        switch (AddCategory.value)
        {
            case 1:
                DropdownCategoryName = AddCategory.options[0].text;
                break;
            case 2:
                DropdownCategoryName = AddCategory.options[1].text;
                break;
            case 3:
                DropdownCategoryName = AddCategory.options[2].text;
                break;
            case 4:
                DropdownCategoryName = AddCategory.options[3].text;
                break;
        }
    }

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

    public string InitBookCategory()
    {
        if (LibraryUIController.Instance.AddCategory.options[0].text.Contains("Fiction"))
        {
            //For showing bookname and category on homescreen
            DropdownCategoryName = LibraryUIController.Instance.AddCategory.options[0].text;
            GameObject fiction = Instantiate(Fiction, ParentForBookPrefabs);
            Debug.Log(" Instantiated ....");
            BooksToBeBorrowedInfo.Instance.NameOfThisBook.text = LibraryManagement.Instance.NameAPI;
            BookName.text = "";
            BookName.text = BooksToBeBorrowedInfo.Instance.NameOfThisBook.text;
            BooksToBeBorrowedInfo.Instance.CategoryOfThisBook.text = LibraryManagement.Instance.CategoryAPI;
           
            //For book id
            BookID bookID = fiction.AddComponent<BookID>();
            BooksList.Add(BookID);
            BookID = LibraryManagement.Instance.idAPI;
            Debug.Log("BookID is" + BookID);

        }

        else if (LibraryUIController.Instance.AddCategory.options[1].text.Contains("Action"))
        {
            DropdownCategoryName = LibraryUIController.Instance.AddCategory.options[1].text;
            GameObject action = Instantiate(Action, ParentForBookPrefabs);
        }
        else if (LibraryUIController.Instance.AddCategory.options[2].text.Contains("Non-Fiction"))
        {
            DropdownCategoryName = LibraryUIController.Instance.AddCategory.options[2].text;
            GameObject nonfiction = Instantiate(NonFiction, ParentForBookPrefabs);
        }
        else if (LibraryUIController.Instance.AddCategory.options[3].text.Contains("Comics"))
        {
            DropdownCategoryName = LibraryUIController.Instance.AddCategory.options[3].text;
            GameObject comics = Instantiate(Comics, ParentForBookPrefabs);
        }
        return DropdownCategoryName;
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
}
