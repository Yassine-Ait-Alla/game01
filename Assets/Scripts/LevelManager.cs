using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{

	private static LevelManager instance;
	public static LevelManager Instance{get{return instance;}}

	public GameObject		pauseMenu;

	public Transform		respawn;
	private GameObject		player;

	private float			startTime;
	public float			silverTime;
	public float			goldTime;

	public Text				speed;
	public Text				time;

	private void Start ()
	{
		//PlayerPrefs.DeleteAll();
		startTime = Time.time;
		silverTime = 30.0f;
		goldTime = 10.0f;
		instance = this;
		pauseMenu.SetActive(false);
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = respawn.position;
	}

	private void Update ()
	{
		time.text = "TIME : " + Time.time.ToString();
		speed.text = "SPEED : " + player.GetComponent<Rigidbody>().velocity.magnitude.ToString();
		if (player.transform.position.y < -10.0f)
			Death();
	}

	public void		Victory()
	{
		float duration = Time.time - startTime;
		/*
		Debug.Log("VICTORY");
		Debug.Log(duration);
		Debug.Log(goldTime);
		*/
		if (duration < goldTime)
			gameManager.Instance.currency += 50;
		else if (duration < silverTime)
			gameManager.Instance.currency += 25;
		else
			gameManager.Instance.currency += 10;
		//Debug.Log("currency = " + gameManager.Instance.currency);
		gameManager.Instance.Save();

		string	saveString = "";

		LevelData level = new LevelData(Application.loadedLevelName);

		saveString += (level.bestTime > duration || level.bestTime == 0.0f)
			? duration.ToString() : level.bestTime.ToString();
		saveString += '&';
		saveString += silverTime.ToString();
		saveString += '&';
		saveString += goldTime.ToString();
		PlayerPrefs.SetString(Application.loadedLevelName, saveString);

		//Behaviour of Victory
		Application.LoadLevel("MainMenu");
	}
	
	public void		TogglePauseMenu()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		Time.timeScale = Time.timeScale == 1 ? 0 : 1;
	}

	public void		ToMenu()
	{
		Application.LoadLevel("MainMenu");
		Time.timeScale = 1;
	}

	public void	Death()
	{
		player.transform.position = respawn.position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}

	public void	Restart()
	{
		Application.LoadLevel(Application.loadedLevelName);
		Time.timeScale = 1;
	}

	public void	NextLevel()
	{
		string		next = (Application.loadedLevelName[0] - '0' + 1).ToString() + "_level";
		Application.LoadLevel(next);
		Time.timeScale = 1;
	}

}
