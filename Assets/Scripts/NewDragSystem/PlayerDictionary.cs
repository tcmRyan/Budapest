using UnityEngine;
using System.Collections.Generic;

public class PlayerDictionary : MonoBehaviour {
    public GameObject word;
    private List<string> discoveredWords = new List<string>();
    HashSet<string> uniqueList = new HashSet<string>();
    string prefKey = "discoveredWords";
    public bool resetForTesting = true;
	// Use this for initialization
	void Start () {
        LoadDictionary();

        if (discoveredWords.Count == 0)
        {
            //string baseWord = "test";
            //for (int i = 0; i < 20; i++)
            //{
            //    string newWord = baseWord + i;
            //    AddWord(newWord);
            //}
            AddWord("test1");
            AddWord("test2");
            Debug.Log("Count after starting: " + discoveredWords.Count);
        }
        else
        {
            ManageDictionary();
        }
	}
	

    public void AddWord(string newWord)
    {
        Debug.Log(newWord);

        if (uniqueList.Add(newWord))
        {
            discoveredWords.Add(newWord);
            discoveredWords.Sort();
            ManageDictionary();
            SaveDictionary();
        }  
    }

    private void LoadDictionary()
    {
        //Load the dictionary into memory
        if (resetForTesting)
        {
            PlayerPrefs.SetString(prefKey, "");
        }     
        string[] ParsedWords = PlayerPrefs.GetString(prefKey).Split(";".ToCharArray());
        for (int i = 0; i < ParsedWords.Length; i++)
        {
            if (ParsedWords[i].Length > 1)
            {
                uniqueList.Add(ParsedWords[i]);
                discoveredWords.Add(ParsedWords[i]);
            }

        }
        Debug.Log(discoveredWords.Count);
    }

    private void ManageDictionary()
    {
        //TODO: Work on sorting rather then destorying
        DestoryAllChildren();
        foreach (string characters in discoveredWords)
        {
            GameObject newWord = Instantiate(word);
            newWord.transform.SetParent(transform);
            newWord.GetComponent<Word>().SetText(characters);
        }
    }

    void OnDestroy()
    {
        SaveDictionary();
    }

    void SaveDictionary()
    {
        //clear the string for now
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < discoveredWords.Count; i++)
        {
            if (i != discoveredWords.Count - 1)
            {
                builder.Append(discoveredWords[i]);
                builder.Append(";");
            }
            else
            {
                builder.Append(discoveredWords[i]);
            }
        }
        PlayerPrefs.SetString(prefKey, builder.ToString());
    }

    void DestoryAllChildren()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
