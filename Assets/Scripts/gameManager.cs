using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public GameObject gameTime;
	public timeScript timer;

	private bool _init = false;
    private int _matches = 8;        //Number of Matched Cards 
    // Start is called before the first frame update
    void Start()
    {
		ScoreManager.Instance.ResetScore();
	}


	// Update is called once per frame
	void Update()
	{
		if (!_init)
			initializeCards();

		if (Input.GetMouseButtonUp(0))
			checkCards();

	}

	void initializeCards()
	{
		List<int> shuffledIndices = Enumerable.Range(0, cards.Length).OrderBy(x => Random.value).ToList();
		int index = 0;

		for (int id = 0; id < 4; id++) // Rows
		{
			for (int i = 1; i < 9; i++) // Columns and Number of Matched paired Cards
			{
				if (index >= cards.Length) break;

				cards[shuffledIndices[index]].GetComponent<cardScript>().cardValue = i;
				cards[shuffledIndices[index]].GetComponent<cardScript>().initialized = true;
				index++;
			}
		}

		foreach (GameObject c in cards)
			c.GetComponent<cardScript>().setupGraphics();

		if (!_init)
			_init = true;
	}

	public Sprite getCardBack()
	{
		return cardBack;
	}

	public Sprite getCardFace(int i)
	{
		return cardFace[i - 1];
	}

	void checkCards()
	{
		List<int> c = new List<int>();

		for (int i = 0; i < cards.Length; i++)
		{
			if (cards[i].GetComponent<cardScript>().state == 1)
				c.Add(i);
		}

		if (c.Count == 2)
			cardComparison(c);
	}

	void cardComparison(List<int> c)
	{
		cardScript.DO_NOT = true;

		int x = 0;

		if (cards[c[0]].GetComponent<cardScript>().cardValue == cards[c[1]].GetComponent<cardScript>().cardValue)
		{
			x = 2;
			_matches--;                         //Matched
			AudioManager.Instance.PlayMatchSound();
			float matchTime = timer.GetElapsedTime();
			ScoreManager.Instance.AddMatchScore(matchTime);
			Debug.Log("Matched");
			if (_matches == 0)
				gameTime.GetComponent<timeScript>().endGame();
        }
        else
        {
			AudioManager.Instance.PlayUnmatchSound();
			ScoreManager.Instance.AddMismatchPenalty();
			Debug.Log("Not Matched");                    //Not Matched
        }


		for (int i = 0; i < c.Count; i++)
		{
			cards[c[i]].GetComponent<cardScript>().state = x;
			cards[c[i]].GetComponent<cardScript>().falseCheck();
		}

	}

	public void reGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void reMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
