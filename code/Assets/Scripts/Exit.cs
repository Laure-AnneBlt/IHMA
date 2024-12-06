using UnityEngine;

public class KeyboardEvents : MonoBehaviour
{
    void Update()
    {
        // Bouton ECHAP ou ce que vous voulez
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

            // Log a message for debugging purposes (works in the editor)
            Debug.Log("Application Quit");
        }
    }
}