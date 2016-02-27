using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BaseWord : MonoBehaviour {

    public string characters;
    public GameObject wordPrefab;

    public void OnClick()
    {
        GameObject newWord = Instantiate(wordPrefab, new Vector3( transform.position.x, transform.position.y, transform.position.z), transform.rotation) as GameObject;
        Word word = newWord.GetComponent<Word>();
        word.characters = characters;
        TextMesh text = newWord.GetComponent<TextMesh>();
        text.text = characters;
        SimpleDragAndDrop dragndrop = newWord.GetComponent<SimpleDragAndDrop>();
        dragndrop.EnableObject();
        
    }

    public void BaseWordSetup(string characters)
    {
        this.characters = characters;
        this.GetComponent<TextMesh>().text = characters;
    }
}
