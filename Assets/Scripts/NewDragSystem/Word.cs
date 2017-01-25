using UnityEngine;
using SQLConnect;
using System;
using UnityEngine.UI;

public class Word : MonoBehaviour {

    public string characters;
    private SQLiteConnector connection;
    private GameObject manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        connection = manager.GetComponent<SQLiteConnector>();
    }

    public string matchWords(string targetWord)
    {
        // Need to sort the array and lower the case
        string[] sortList = { characters, targetWord };
        Array.Sort(sortList);
        return connection.MatchQuery(sortList[0], sortList[1]);
    }

    public void WordUpdate(Word targetWord)
    {
        string matchResult = matchWords(targetWord.characters);
        if (!(matchResult == ""))
        {
            Destroy(targetWord.gameObject);
            CreateNewWord(matchResult);
            Destroy(gameObject);
            Debug.Log(matchResult);
            manager = GameObject.FindGameObjectWithTag("WordGrid");
            manager.GetComponent<PlayerDictionary>().AddWord(matchResult);
        }
    }

    public void CreateNewWord(string result)
    {
        GameObject newWord = Instantiate(gameObject);
        newWord.GetComponent<Word>().SetText(result);
    }

    public void SetText(string wordText)
    {
        characters = wordText;
        try
        {
            gameObject.GetComponentInChildren<TextMesh>().text = wordText;
        }
        catch(NullReferenceException e)
        {
            gameObject.GetComponentInChildren<Text>().text = wordText;
        }
    }
}
