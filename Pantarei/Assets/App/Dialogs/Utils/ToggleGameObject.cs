using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField, Tooltip("List of GameObjects to toggle")]
    private List<GameObject> gameObjects;

    [SerializeField, Tooltip("If true all GameObjects will deactivate on startup")]
    private bool DeactivateOnStartup = false;
    void Start()
    {
        foreach (GameObject go in gameObjects) go.SetActive(!DeactivateOnStartup);
    }

    public void Toggle()
    {
        foreach (GameObject go in gameObjects) Toggle(go);
    }

    public void Toggle(GameObject o)
    {
        o.SetActive(!o.activeSelf);
    }

    
}
