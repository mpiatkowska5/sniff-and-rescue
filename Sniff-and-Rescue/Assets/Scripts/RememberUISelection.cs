using UnityEngine;
using UnityEngine.EventSystems;

public class RememberUISelection : MonoBehaviour
{
    EventSystem eventSystem;
    GameObject lastSelected;

    private void Awake()
    {
        eventSystem = EventSystem.current;
    }
    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject && lastSelected != eventSystem.currentSelectedGameObject)
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
        if (!eventSystem.currentSelectedGameObject && lastSelected) 
        { 
            eventSystem.SetSelectedGameObject(lastSelected);
        }

    }
}
