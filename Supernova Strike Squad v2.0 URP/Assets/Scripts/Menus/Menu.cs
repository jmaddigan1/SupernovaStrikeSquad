using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]

// A basic menu as a close and open function
// Note, there do  not have 
public class Menu : MonoBehaviour
{
	// Is a menu currently in transition?
	public static bool MenuTransitionInProgress = false;

	// The Menu this  was opened from
	[HideInInspector]
	public Menu ParentMenu;

	// When this menu is closed we call this
	protected System.Action callback;

	// This menus Open and Close Transitions
	public MenuTransition OpenTransition = null;
	public MenuTransition CloseTransition = null;

	/// <summary>
	/// Open this Menu.
	/// </summary>
	/// <param name="parent">The Menu that has opened us.</param>
	/// <param name="closeParentMenu">Do we want to destroy our parent?</param>
	/// <param name="closeCallback">The callback for when we are closed.</param>
	public virtual void OpenMenu(Menu parent, bool closeParentMenu = true, System.Action closeCallback = null)
	{
		ParentMenu = parent;
		callback = closeCallback;

		StartCoroutine(coOpenMenu(closeParentMenu));
	}

	public virtual void CloseMenu()
	{
		StartCoroutine(coCloseMenu());
	}

	IEnumerator coOpenMenu(bool closeParentMenu)
	{
		MenuTransitionInProgress = true;

		// If we want to close out parent menu
		if (closeParentMenu)
		{
			// Run the parents close coroutine
			yield return ParentMenu.coCloseMenu();
		}

		// Play our Open transition
		GetComponent<CanvasGroup>().alpha = 1f;
		yield return OpenTransition.Play(this);

		MenuTransitionInProgress = false;
	}

	IEnumerator coCloseMenu()
	{
		yield return CloseTransition.Play(this);

		callback?.Invoke();
	}

	public void Open(Menu target)
	{
		target.gameObject.SetActive(true);
		target.GetComponent<CanvasGroup>().alpha = 0f;
		target.OpenMenu(this, true);
	}

	public void UpdateCursor(CursorLockMode mode, bool visible)
	{
		Cursor.lockState = mode;
		Cursor.visible = visible;
	}
}
