using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class BookController : Singleton<BookController>
{
    public Text NameOfThisBook;
    public Text CategoryOfThisBook;

    public BookDetails bookDetails;


    //public void OnEnable()
    //{
    //    LibraryUIController.Instance.onBorrowButtonYesClicked += InstantiateBooksBorrowed;
    //}

    //public void OnDisable()
    //{
    //    LibraryUIController.Instance.onBorrowButtonYesClicked -= InstantiateBooksBorrowed;
    //}
    /// <summary>
    /// On click of Book prefab
    /// </summary>
    public void InitBookDetails()
    {
        LibraryUIController.Instance.BorrowPanel.SetActive(true);
        SetBorrowPanelDetails();
        LibraryUIController.Instance.CurrentBookID = bookDetails.id;
        LibraryUIController.Instance.CurrentAuthorName = bookDetails.AuthorName;
        LibraryUIController.Instance.CurrentBookName = bookDetails.Name;
        LibraryUIController.Instance.CurrentCategoryname = bookDetails.Category;
        LibraryUIController.Instance.CurrentReturnID = bookDetails.id;
        LibraryUIController.Instance.CurrentbooksBorrowed = this;
    }

    public void SetBorrowPanelDetails()
    {
        LibraryUIController.Instance.BookName.text = bookDetails.Name;
        LibraryUIController.Instance.AuthorName.text = bookDetails.AuthorName;
        LibraryUIController.Instance.InstantiateStar(bookDetails.Rating);
        LibraryUIController.Instance.Pages.text = bookDetails.Pages.ToString();
        LibraryUIController.Instance.Language.text = bookDetails.Language;
        LibraryUIController.Instance.Introduction.text = bookDetails.Introduction;
    }
}
