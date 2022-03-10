using UnityEngine;
using UnityEngine.UI;

public class BooksBorrowed : MonoBehaviour
{
    public Text Name;
    public Text Category;
    public Text Author;
    public int Id;
    public Image CategoryIcon;

    public void OnBooksToBeReturnedSelected()
    {
        LibraryUIController.Instance.CurrentReturnID = Id;
        LibraryUIController.Instance.OnReturnBookSelect();
        LibraryUIController.Instance.BookReturnNameText.text = Name.text;
    }
}
