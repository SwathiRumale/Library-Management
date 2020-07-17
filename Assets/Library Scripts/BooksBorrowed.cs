using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BooksBorrowed : MonoBehaviour
{
    public Text Name;
    public Text Category;
    public Text Author;
    public int Id;


    public void Init()
    {
        
    }
    public void OnBooksToBeReturnedSelected()
    {
        LibraryUIController.Instance.CurrentReturnID = Id;
        LibraryUIController.Instance.OnReturnBookSelect();
    }
}
