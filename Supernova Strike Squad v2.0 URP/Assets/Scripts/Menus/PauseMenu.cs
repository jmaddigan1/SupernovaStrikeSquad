using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject = null;
    public KeyCode OpenButton = KeyCode.Escape;

   private bool open = false;

    void Update()
    {
		if (Input.GetKeyDown(OpenButton))
        {
            // If were NOT interacting with a menu
            if (PlayerController.Interacting == false)
            {
                open = true;
                UpdatePlayer(open);
            }
			else
			{
				if (open)
                {
                    open = false;
                    UpdatePlayer(open);
                }
			}
        }
    }

    void UpdatePlayer(bool opening)
	{
        string current = SceneManager.GetActiveScene().name;

		if (current == "Gameplay")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (current == "Main")
        {
            Cursor.lockState = opening ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = opening;
        }
 
        PlayerController.Interacting = opening;

        menuObject.SetActive(opening);

    }
}
