using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClick : MonoBehaviour
{
    public List<GameObject> gameObjects; 

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => OnCLickButton(gameObjects));
    }

    public void OnCLickButton(List<GameObject> gameObjects)
    {
        gameObjects[0].SetActive(true);
        gameObjects[1].SetActive(true);
        print("Button CLicked");
    }
}
