using UnityEngine;

class MessageBox
{
    static Object cacheObject = null;

    public static UIMessageBox Show(string achieveName, string message, string title = "", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (cacheObject == null)
        {
            if (type == MessageBoxType.Information)
            {
                cacheObject = Resources.Load<Object>("UI/UIMapName");

            }
            if(type == MessageBoxType.notice)
            {
                cacheObject = Resources.Load<Object>("UI/UINotice");
            }
            if (type == MessageBoxType.Confirm || type == MessageBoxType.Error)
            {
                cacheObject = Resources.Load<Object>("UI/UIMessageBox");
            }
        }
        else
        {
            if (type == MessageBoxType.Information)
            {
                cacheObject = Resources.Load<Object>("UI/UIMapName");

            }
            if(type == MessageBoxType.notice)
            {
                cacheObject = Resources.Load<Object>("UI/UINotice");
            }

            if (type == MessageBoxType.Confirm || type == MessageBoxType.Error)
            {
                cacheObject = Resources.Load<Object>("UI/UIMessageBox");
            }
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();
        msgbox.Init(achieveName, title, message, type, btnOK, btnCancel);
        return msgbox;
    }
}

public enum MessageBoxType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    /// 
    notice = 3,
    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    /// 
    Error = 4
}