using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Pixelplacement;

public class LibraryManager : Singleton<LibraryManager>
{
    public string APIEndpoint = "https://5db87d87177b350014ac7b45.mockapi.io";
    public BookDetails[] bookDetails;

    private void Start()
    {
        GetBookList();
    }

    // *************************** Upload new book and fetch existing book method start *************************//


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

    public void DeleteBook()
    {
        foreach(var book in bookDetails)
        {
            StartCoroutine(DeleteBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks/" + book.id));
        }
       
    }
    // *************************** Upload new book and fetch existing book method ends *************************//



    // ***************************Get, Post, Delete Couroutine Start *************************//

    IEnumerator _PostNewBook(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", LibraryUIController.Instance.AddNewBookNameText.text);
        form.AddField("Category", LibraryUIController.Instance.BookCategoryDropdown);
        form.AddField("Rating", LibraryUIController.Instance.RatingText.text);
        form.AddField("AuthorName", LibraryUIController.Instance.AddNewAuthorText.text);
        form.AddField("Language", LibraryUIController.Instance.AddNewLanguageText.text);
        form.AddField("Year", LibraryUIController.Instance.AddNewYearText.text);
        form.AddField("Pages", LibraryUIController.Instance.AddNewPagesText.text);
        form.AddField("Introduction", LibraryUIController.Instance.AddNewDescription.text);
        LibraryUIController.Instance.isMinimumWordsInIntroduction();
        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending _PostNewBook -> : " + uwr.error);
        }
        else
        {
            //Debug.Log("Uploaded NewBook: " + uwr.downloadHandler.text);
            GetBookList();
        }
    }

    IEnumerator DeleteBook(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Delete(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending DeleteBook -> : " + uwr.error);
        }
        else
        {
            Debug.Log("Deleted Book ->" +url);
        }
    }

    IEnumerator _GetBookList(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        //Creating an Object of class containing an array of Book Details
        BookCollection bookCollectionObject;
        //Getting data in JSON and storing inside the object (we are handling the JSON data which we are getting array and converting into JSON object to fix "JSON must represent an object type")
        bookCollectionObject = JsonUtility.FromJson<BookCollection>("{\"bookCollection\":" + uwr.downloadHandler.text + "}");
        //To read the data inside JSON object we need to convert the object to JSON
        var myjson = JsonUtility.ToJson(bookCollectionObject);

        // myjson now has data in JSON format
        var myObj = JsonUtility.FromJson<BookCollection>(myjson);
        bookDetails = myObj.bookCollection;
        
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending _GetBookList -> : " + uwr.error);
        }
        else
        {
            LibraryUIController.Instance.InstantiateBook(bookDetails);
            //DeleteBook();
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


