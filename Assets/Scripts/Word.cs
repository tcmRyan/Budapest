using UnityEngine;
using System.Collections;
using SQLConnect;
using System;

public class Word : MonoBehaviour {

    public string characters;
    //public Sprite background;
    //public Word prefab;
    private SQLiteConnector connection;
    private GameObject manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        connection = manager.GetComponent<SQLiteConnector>();
    }

    public string matchWords(string targetWord)
    {
        // Need to sort the array and lower the case
        string[] sortList = { characters, targetWord };
        Array.Sort(sortList);
        string result = connection.MatchQuery(sortList[0], sortList[1]);
        return result;
    }

    public void wordUpdate(Word targetWord)
    {
        string matchResult = matchWords(targetWord.characters);
        if (!(matchResult == ""))
        {
            Destroy(targetWord.gameObject);
            createNewWord(matchResult);
            Destroy(this.gameObject);
        }
    }

    public void createNewWord(string result)
    {
        Word newWord;
        newWord = Instantiate(this);
        newWord.characters = result;
        TextMesh text = newWord.GetComponent<TextMesh>();
        text.text = result;
        manager.GetComponent<PlayerDictionary>().AddWord(result);
    }
}
