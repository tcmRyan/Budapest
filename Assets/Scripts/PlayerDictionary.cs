using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerDictionary : MonoBehaviour {

    public GameObject BaseWordPrefab;
    private Vector3 startingPosition;
    public List<string> discoveredWords = new List<string>();
    HashSet<string> uniqueList = new HashSet<string>();
    string prefKey = "discoveredWords";
    string prefsAsString;
	// Use this for initialization
	void Start () {
        //Load the dictionary into memory
        //PlayerPrefs.SetString(prefKey, "");
        string[] temp = PlayerPrefs.GetString(prefKey).Split(";".ToCharArray());
        for (int i=0; i < temp.Length; i++)
        {
            if (temp[i].Length > 1)
            {
                uniqueList.Add(temp[i]);
                discoveredWords.Add(temp[i]);
            }
            
        }
        Debug.Log(discoveredWords.Count);
        if (discoveredWords.Count == 0)
        {    
            AddWord("test1");
            AddWord("test2");
        }
        else
        {
            ManageDictionary();
        }
	}
	

    public void AddWord(string newWord)
    {
        if (uniqueList.Add(newWord))
        {
            discoveredWords.Add(newWord);
            discoveredWords.Sort();
            ManageDictionary();
            SaveDictionary();
        }  
    }

    private void ManageDictionary()
    {
        float x = 8.5f;
        float y = 4.5f;
        
        GameObject[] existingDictionary = GameObject.FindGameObjectsWithTag("baseWord");
        foreach (GameObject oldWord in existingDictionary)
        {
            Destroy(oldWord.gameObject);
        }
        foreach (string word in discoveredWords)
        {
            startingPosition = new Vector3(x, y, 0f);
            GameObject newBase = Instantiate(BaseWordPrefab, startingPosition, Quaternion.identity) as GameObject;
            newBase.GetComponent<BaseWord>().BaseWordSetup(word);
            y = y - 1;
        }
    }

    void OnDestroy()
    {
        SaveDictionary();
    }

    void SaveDictionary()
    {
        //clear the string for now
        prefsAsString = "";
        for (int i = 0; i < discoveredWords.Count; i++)
        {
            if (i != discoveredWords.Count - 1)
            {
                prefsAsString += discoveredWords[i] + ";";
            }
            else
            {
                prefsAsString += discoveredWords[i];
            }
        }
        PlayerPrefs.SetString(prefKey, prefsAsString);
    }
}
