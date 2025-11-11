using UnityEngine;
using UnityEngine.Events;

public class KeyEventTrigger : MonoBehaviour
{
    // Public variables to specify the key and the event in the Unity Editor
    public KeyCode triggerKey = KeyCode.Space;
    public UnityEvent onKeyPressed;
    public SettingMenuController setController;
    // Update is called once per frame
    void Update()
    {
        // Check if the specified key is pressed
        if (Input.GetKeyDown(triggerKey))
        {
            // Invoke the UnityEvent
            onKeyPressed.Invoke();
        }
        
    }
    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space) && UIWorldElementManager.Instance.setPanel.activeInHierarchy)
        {
            UIWorldElementManager.Instance.setPanel.gameObject.SetActive(false);
            setController.HideSettings();
        }
    }
}
