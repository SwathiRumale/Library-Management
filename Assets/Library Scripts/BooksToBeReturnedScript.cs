using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksToBeReturnedScript : MonoBehaviour
{
   public void OnBooksToBeReturnedSelected()
    {
        LibraryUIController.Instance.OnReturnBookSelect();
    }
}
