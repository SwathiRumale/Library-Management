using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using Pixelplacement;

public class LibraryManagement : Singleton<LibraryManagement>
{
    public string APIendpoint = "https://5db87d87177b350014ac7b45.mockapi.io";

    [Header("Book Detailas from API")]
    public int idAPI;
    public string NameAPI;
    public string CategoryAPI;
    public string AuthorNameAPI;
    public int RatingAPI;
    public string LanguageAPI;
    public int YearAPI;
    public int PagesAPI;
    public string IntroductionAPI;

    private void Start()
    {
        GetBookList();
    }


    // ***************************Upload new book and fetch existing book method start *************************//


    public void AddNewBookToTheCollection()
    {
        PostNewBook();
    }

    public void GetBookList()
    {
        StartCoroutine(_GetBookList("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
    }

    public void PostNewBook()
    {
        StartCoroutine(_PostNewBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
    }

    public void DeleteBook(int id)
    {
        StartCoroutine(DeleteBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks/" + id));
    }
    // ***************************Upload new book and fetch existing book method ends *************************//





    // ***************************Get, Post, Delete Couroutine Start *************************//

    IEnumerator _PostNewBook(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", LibraryUIController.Instance.AddName.text);
        form.AddField("Category", LibraryUIController.Instance.BookCategoryDropdown);
        //Debug.Log("Drop Down 1 is" + LibraryUIController.Instance.BookCategoryDropdown.options[1].text);
        form.AddField("Rating", LibraryUIController.Instance.Rating.text);
        form.AddField("AuthorName", LibraryUIController.Instance.AddAuthorName.text);
        form.AddField("Language", LibraryUIController.Instance.AddLanguage.text);
        form.AddField("Year", LibraryUIController.Instance.AddYear.text);
        form.AddField("Pages", LibraryUIController.Instance.AddPages.text);
        form.AddField("Introduction", LibraryUIController.Instance.AddIntroductin.text);
        LibraryUIController.Instance.isMinimumWordsInIntroduction();
        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Uploaded NewBook: " + uwr.downloadHandler.text);
            LibraryManagement.Instance.GetBookList();
        }
    }

    IEnumerator DeleteBook(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Delete(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Deleted");
        }
    }

    IEnumerator _GetBookList(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        Debug.Log("GetBookList url ->" + uri);
        yield return uwr.SendWebRequest();

        //Creating an Object of class containing an array of Book Details
        BookCollection bookCollectionObject;
        //Getting data in JSON and storing inside the object (we are handling the JSON data which we are getting array and converting into JSON object to fix "JSON must represent an object type")
        bookCollectionObject = JsonUtility.FromJson<BookCollection>("{\"bookCollection\":" + uwr.downloadHandler.text + "}");
        //To read the data inside JSON object we need to convert the object to JSON
        var myjson = JsonUtility.ToJson(bookCollectionObject);

        // myjson now has data in JSON format
        var myObj = JsonUtility.FromJson<BookCollection>(myjson);
        BookDetails[] bookDetails = myObj.bookCollection;


        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            LibraryUIController.Instance.InstantiateBook(bookDetails);
        }
       
    }
}

// *************************** Get, Post, Delete Couroutine Ends *************************//






// *************************** Book Details Serializable Class Start *************************//
[Serializable]
public class BookDetails
{
    public int id;
    public string Name;
    public string Category;
    public string AuthorName;
    public int Rating;
    public string Language;
    public int Year;
    public int Pages;
    public string Introduction;
}

[Serializable]
public class BookCollection
{
    public BookDetails[] bookCollection;
}

// *************************** Book Details Serializable Class End *************************//


