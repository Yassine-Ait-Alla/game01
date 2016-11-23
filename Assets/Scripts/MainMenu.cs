using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelData
{
	public LevelData(string levelName)
	{
		string data = PlayerPrefs.GetString(levelName);
		if (data == "")
			return ;
		string[] allData = data.Split('&');
		bestTime = float.Parse(allData[0]);
		silverTime = float.Parse(allData[1]);
		goldTime = float.Parse(allData[2]);
	}

	public float			bestTime {set; get;}
	public float			silverTime {set; get;}
	public float			goldTime {set; get;}
}

public class MainMenu : MonoBehaviour 
{
	public GameObject		levelButtonP;
	public GameObject		levelButtonContainer;
	private Transform		cameraTransform;
	private Transform		cameraDesiredLookAt;

	public GameObject		shopButtonP;
	public GameObject		shopButtonContainer;

	private bool			nextLevelLocked = false;

	public Material			playerMaterial;
	public Text				currencyText;
	public Text				priceText;
	private int				[]priceGrid = new int [] {25, 45, 95, 130, 190, 255, 300, 450};

	private void Start ()
	{
		int			textureIndex = 0;

		//PlayerPrefs.DeleteAll();

		ChangePlayerSkin(gameManager.Instance.currentSkin);
		//currencyText.text = "Currency : " + gameManager.Instance.currency.ToString();
		cameraTransform = Camera.main.transform;

		Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
		foreach(Sprite thumbnail in thumbnails)
		{
			GameObject container = Instantiate(levelButtonP) as GameObject;
			container.GetComponent<Image>().sprite = thumbnail;
			container.transform.SetParent(levelButtonContainer.transform,false);

			LevelData level = new LevelData(thumbnail.name);
			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
				= (level.bestTime != 0) ? level.bestTime.ToString("f") : "NONE";

			container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked;
			container.GetComponent<Button>().interactable = !nextLevelLocked;
			if (level.bestTime == 0.0f)
				nextLevelLocked = true;

			string sceneName = thumbnail.name;

			container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
		}

		Sprite[] textures = Resources.LoadAll<Sprite>("Player");
		foreach(Sprite texture in textures)
		{
			int			index = textureIndex;
			GameObject container = Instantiate(shopButtonP) as GameObject;
			container.GetComponent<Image>().sprite = texture;
			container.transform.SetParent(shopButtonContainer.transform,false);
			shopButtonContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, 0);

			container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
			container.GetComponent<Button>().GetComponentInChildren<Text>().text = priceGrid[index].ToString();
			if ((gameManager.Instance.skinAvailable & 1 << index) == 1 << index)
			{
				container.transform.GetChild(0).gameObject.SetActive(false);
			}
			else
			{
				//Debug.Log(priceGrid[index].ToString());
				//GameObject.Find("price").text = priceGrid[index].ToString();
				//priceText.text = priceGrid[index].ToString();
			}
			textureIndex++;
		}

	}

	private void Update ()
	{
		currencyText.text = "Currency : " + gameManager.Instance.currency.ToString();
		if (cameraDesiredLookAt != null)
		{
			cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, 3 * Time.deltaTime);
		}
	}

	public void		ResetShop(RectTransform menuTransform)
	{
		menuTransform.anchoredPosition = new Vector2(0, 0);
	}
	
	public void		ResetMenuPosition(RectTransform menuTransform)
	{
		if(menuTransform.name == "lvlButtonContainer")
			menuTransform.anchoredPosition = new Vector2(0, 0);
		else
			menuTransform.anchoredPosition = new Vector2(-350, 0);
	}

	private void	LoadLevel(string sceneName)
	{
		Application.LoadLevel(sceneName);
	}
	
	public void		LookAtMenu(Transform menuTransform)
	{
		cameraDesiredLookAt = menuTransform;
	}
	
	private void	ChangePlayerSkin(int index)
	{
		if ((gameManager.Instance.skinAvailable & 1 << index) == 1 << index)
		{

			float	x = (index % 4) * 0.25f;
			float	y = ((int)index) / 2 * 0.25f;

			if (y == 0.0f)
				y = 0.75f;
			else if (y == 0.25f)
				y = 0.5f;
			else if (y == 0.5f)
				y = 0.25f;
			else if (y == 0.75f)
				y = 0;

			playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));

			gameManager.Instance.currentSkin = index;
			gameManager.Instance.Save();
		}
		else
		{
			int			cost = priceGrid[index];

				Debug.Log("Currency = " + gameManager.Instance.currency);
				Debug.Log("Cost = " + cost);
			if (gameManager.Instance.currency >= cost)
			{
				gameManager.Instance.currency -= cost;
				Debug.Log("Currency = " + gameManager.Instance.currency);
				gameManager.Instance.skinAvailable += 1 << index;
				gameManager.Instance.Save();
				shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);

				//ChangePlayerSkin(index); // comment to un-auto-select-new_color
			}
		}
	}
}
