using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class BooksToBeBorrowedInfo : Singleton<BooksToBeBorrowedInfo>
{
    public Text NameOfThisBook;
    public Text CategoryOfThisBook;
    /// <summary>
    /// On click of Book prefab
    /// </summary>
    public void InitBookDetails()
    {
        Debug.Log("InitBookDetails clicked");
        LibraryUIController.Instance.BorrowPanel.SetActive(true);       
    }
}
