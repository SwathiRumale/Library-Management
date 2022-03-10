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
        LibraryUIController.Instance.FullScreenBorrowPanel.gameObject.SetActive(true);
        SetBorrowPanelDetails();
        LibraryUIController.Instance.CurrentBookID = bookDetails.id;
        LibraryUIController.Instance.CurrentAuthorName = bookDetails.AuthorName;
        LibraryUIController.Instance.CurrentBookName = bookDetails.Name;
        LibraryUIController.Instance.CurrentCategoryname = bookDetails.Category;
        LibraryUIController.Instance.CurrentReturnID = bookDetails.id;
        LibraryUIController.Instance.SetSprite(LibraryUIController.Instance.CurrentBookIcon, LibraryUIController.Instance.CurrentCategoryname);
        LibraryUIController.Instance.CurrentbooksBorrowed = this;
    }

    public void SetBorrowPanelDetails()
    {
        LibraryUIController.Instance.BookNameText.text = bookDetails.Name;
        LibraryUIController.Instance.AuthorNameText.text = bookDetails.AuthorName;
        LibraryUIController.Instance.InstantiateStar(bookDetails.Rating);
        LibraryUIController.Instance.PagesText.text = bookDetails.Pages.ToString();
        LibraryUIController.Instance.LanguageText.text = bookDetails.Language;
        LibraryUIController.Instance.DescriptionText.text = bookDetails.Introduction;
    }
}
