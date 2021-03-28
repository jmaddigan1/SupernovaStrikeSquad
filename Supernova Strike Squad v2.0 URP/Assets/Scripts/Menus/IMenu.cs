using System;

public interface IMenu
{
	void Close();
	void Open(Action<string[]> callback);
}