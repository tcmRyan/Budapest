using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWord : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler {
    public string characters;
    public GameObject wordPrefab;

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        GameObject spawnWord = Instantiate(wordPrefab);
        ExecuteEvents.Execute<IEndDragHandler>(gameObject, eventData, ExecuteEvents.endDragHandler);
        eventData.pointerDrag = spawnWord;
        spawnWord.GetComponent<Word>().SetText(gameObject.GetComponent<Word>().characters);
        ExecuteEvents.Execute<IBeginDragHandler>(spawnWord, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Started on: " + gameObject.name);
    }
}
