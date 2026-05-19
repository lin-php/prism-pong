using UnityEngine;

public class ContactButton : MonoBehaviour
{
    
    public void OpenFeedbackMail()
    {
        Application.OpenURL("mailto:linphp.dev@gmail.com?subject=Prism-Pong-Feedback");
    }


}
