using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pixelplacement;

public class BookID : Singleton<BookID>
{
  
   
    void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnButonClick);
        Debug.Log("Book id is active");
    }
    public void OnButonClick()
    {
        LibraryUIController.Instance.SelectID = LibraryManagement.Instance.idAPI;
    }
}
