using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameHandler : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private GameObject scoreIcon;
	[SerializeField]
	private List<GameObject> slots;
	[SerializeField]
	private List<GameColliderPart> targs;
	[SerializeField]
	private TMP_Text ScoreText;
	[SerializeField]
	private List<int> currentTargs;
	
	bool gameStart = false;
	public bool nextRound = false;
	bool gameOver = false;
	public float waitTime = 0.5f;
	
	int score = 0;
	int level = 0;
	
	[SerializeField]
	public UnityEvent<bool> endRound; // send to curator
	
	private Color32 red = new Color32(255, 39, 0, 255);
	private Color32 green = Color.green;
	private Color32 invis = new Color32(0, 0, 0, 0);
	
    // Start is called before the first frame update
    void Start()
    {
		currentTargs = new List<int>();
		ScoreText = scoreIcon.GetComponent<TMP_Text>();
		foreach(var t in slots)
		{
			targs.Add(t.transform.GetChild(0).GetComponent<GameColliderPart>());
		}
		
		for(int i = 0; i < targs.Count; i++)
		{
			targs[i].OnGameHit.AddListener(CheckHit);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(gameOver)
		{
			ResetStuff();
			endRound.Invoke(true);
			gameStart = false;
			gameOver = false;
			level = 0;
			ScoreText.text = score.ToString();
		}
		Debug.Log(currentTargs.Count);
		if(gameStart && currentTargs.Count == 0)
		{
			endRound.Invoke(false);
			currentTargs.Add(0);
		}
        if(gameStart && nextRound)
		{
			StartNextRound();
		}
    }
	
	public void StartNextRound()
	{
		ResetStuff();
		score++;
		ScoreText.text = score.ToString();
		nextRound = true;
		currentTargs.Clear();
		level+=1;
		StartCoroutine(beginRound(level));
		
	}
	
	public void ResetStuff()
	{
		foreach(var t in targs)
		{
			t.ShowColor(Color.white);
			t.wasHit = false;
		}
	}
	
	private IEnumerator beginRound(int level)
	{
		Debug.Log("starting round");
		nextRound = false;
		
		for(int i = 0; i < level; i++)
		{
			var r = Random.Range(0, targs.Count-1);
			targs[r].ShowColor(green);
			currentTargs.Add(targs[r].roundNum);
			yield return new WaitForSeconds(waitTime);
			targs[r].ShowColor(Color.white);
		}
	}
	
	
	private void CheckHit(int num)
	{
		Debug.Log("checking from hit");
		if(currentTargs.Count == 0)
			return;
		var toRemove = new List<int>();
		for(int i = 0; i < currentTargs.Count; i++)
		{
			Debug.Log("num is : " + num);
			if(num != currentTargs[i])
			{
				Debug.Log("game over lol");
				targs[i].ShowColor(red);
				score = 0;
				ScoreText.text = score.ToString();
				gameOver = true;
			}
			else
			{
				Debug.Log("correct");
				targs[i].ShowColor(green);
				toRemove.Add(currentTargs[i]);
			}
		}
		
		foreach(var t in toRemove)
		{
			Debug.Log("removing");
			currentTargs.Remove(t);
		}
		
	}
	
	public void EndRound()
	{
		
	}
	
	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
		{
			gameStart = true;
		}
	}
}
