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
    //public string[] ListOfBooksInCollection;
    public List<string> ListOfBooksInCollection;
    public List<GameObject> AllBooks;



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

    private void OnEnable()
    {
        CheckForExistingBooks();
    }

    private void Start()
    {
        //Debug.Log("API IS" + APIendpoint + "AddBooks");
        //StartCoroutine(GetBookList($"{APIendpoint}/AddBooks?"));
        //= $"{ApiEndpoint}/project/getContactDataForProject/{projectID}/{projectsectionID}?";
        //StartCoroutine(GetBookList("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
        //StartCoroutine(UploadNewBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
        //StartCoroutine(DeleteBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks/5"));
    }

    
    public void AddNewBookToTheCollection()
    {
        StartCoroutine(UploadNewBook("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
    }

    public void GetBookInTheCollection()
    {
        //todo: ask why i got error here
        //StartCoroutine(GetBookList($"{APIendpoint}/AddBooks"));
        StartCoroutine(GetBookList("https://5db87d87177b350014ac7b45.mockapi.io/AddBooks"));
    }

    IEnumerator UploadNewBook(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("Name",LibraryUIController.Instance.AddName.text);
        form.AddField("Category", LibraryUIController.Instance.InitBookCategory());
        Debug.Log("Drop Down 1 is" + LibraryUIController.Instance.AddCategory.options[1].text);
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
            Debug.Log("Received: " + uwr.downloadHandler.text);
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

    IEnumerator GetBookList(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        //Creating an Object of class containing an array of Book Details
        RootBookCollection bookCollectionObject;

        //Getting data in JSON and storing inside the object (we are handling the JSON data which we are getting array and converting into JSON object to fix "JSON must represent an object type")
        bookCollectionObject = JsonUtility.FromJson<RootBookCollection>("{\"rootbookCollection\":" + uwr.downloadHandler.text + "}");

        //To read the data inside JSON object we need to convert the object to JSON
        var myjson = JsonUtility.ToJson(bookCollectionObject);

        // myjson now has data in JSON format
        var myObj = JsonUtility.FromJson<RootBookCollection>(myjson);
        BookDetails[] b  = myObj.rootbookCollection;
        foreach(var bn in b)
        {
            //ListOfBooksInCollection.Add(bn.Name);
            NameAPI = bn.Name;
            CategoryAPI = bn.Category;
            idAPI = bn.id;

            Debug.Log("idapi is " + idAPI);
            LibraryUIController.Instance.InitBookCategory();
            if (LibraryUIController.Instance.BookID == LibraryUIController.Instance.SelectID)
            {
                Debug.Log("newbook and select book are equal");
                Debug.Log("b is" + b);
                yield return b;
            }
        }
        
        //for(StartPrefabCount = CurrentPrefabCount; StartPrefabCount <= b.Length; StartPrefabCount++)
        //{
        //    Debug.Log("b.length is ->" + b.Length);
        //    Debug.Log("CurrentPrefabCount before" + CurrentPrefabCount);
        //    CurrentPrefabCount = StartPrefabCount + 1;
        //    Debug.Log("CurrentPrefabCount after" + CurrentPrefabCount);
        //}
        
        //foreach (var v in myObj.rootbookCollection)
        //{
        //    
        //    if (LibraryUIController.Instance.Rating != null)
        //    {
        //        for(int i = 1; i <= v.Rating; i++)
        //        {
        //            Instantiate(LibraryUIController.Instance.Stars,LibraryUIController.Instance.StarsParent);
        //        }
        //    }
 
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    private void CheckForExistingBooks()
    {
        GetBookInTheCollection();
    }
}

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
public class RootBookCollection
{
    public BookDetails[] rootbookCollection;
}

        