using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
	private static gameManager 	instance;
	public static gameManager 	Instance{get{ return instance;}}
	
	public int					currentSkin = 0;
	public int					currency = 0;
	public int					skinAvailable = 1;




	private void Awake ()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		//PlayerPrefs.DeleteAll();

		if (PlayerPrefs.HasKey("CurrentSkin"))
		{
			currentSkin = PlayerPrefs.GetInt("CurrentSkin");
			currency = PlayerPrefs.GetInt("Currency");
			skinAvailable = PlayerPrefs.GetInt("SkinAvailable");
		}
		else
		{
			Save();
		}
	}
	
	public void Reset()
	{
		PlayerPrefs.DeleteAll();
	}

	public void	Save()
	{
		PlayerPrefs.SetInt("CurrentSkin", currentSkin);
		PlayerPrefs.SetInt("Currency", currency);
		PlayerPrefs.SetInt("SkinAvailable", skinAvailable);
	}

	void Update () {}


}
