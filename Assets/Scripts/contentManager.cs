using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class contentManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GetGames());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator GetGames()
    {
        WWW games = new WWW("http://word-matcher-admin.herokuapp.com/gametables");
        yield return games;
    }

    IEnumerator GetGameData(int gameId)
    {
        string url = string.Concat("http://word-matcher-admin.herokuapp.com/gamedata/", gameId);
        WWW gameData = new WWW(url);
        yield return gameData;
    }
}
