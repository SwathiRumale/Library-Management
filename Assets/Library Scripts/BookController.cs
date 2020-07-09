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

    /// <summary>
    /// On click of Book prefab
    /// </summary>
    public void InitBookDetails()
    {
        LibraryUIController.Instance.BorrowPanel.SetActive(true);
        SetBorrowPanelDetails();
    }

    public void SetBorrowPanelDetails()
    {
        LibraryUIController.Instance.BookName.text = bookDetails.Name;
        LibraryUIController.Instance.AuthorName.text = bookDetails.AuthorName;
        //Rating.text = book.Rating;
        LibraryUIController.Instance.Pages.text = bookDetails.Pages.ToString();
        LibraryUIController.Instance.Language.text = bookDetails.Language;
        LibraryUIController.Instance.Introduction.text = bookDetails.Introduction;
    }
}
