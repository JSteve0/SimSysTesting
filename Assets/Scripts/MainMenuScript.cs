using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI keyboardText;
    [SerializeField] private TextMeshProUGUI controllerText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (keyboardText == null || controllerText == null)
        {
            Debug.LogError("TextMeshProUGUI link is null on " + gameObject.name);
        }
    }

    public void OnKeyboardButtonClick()
    {
        keyboardText.color = Color.white;
        keyboardText.fontStyle = FontStyles.Underline;
        
        controllerText.color = Color.gray;
        controllerText.fontStyle = FontStyles.Normal;
    }
    
    public void OnControllerButtonClick()
    {
        keyboardText.color = Color.gray;
        keyboardText.fontStyle = FontStyles.Normal;
        
        controllerText.color = Color.white;
        controllerText.fontStyle = FontStyles.Underline;
    }
}
