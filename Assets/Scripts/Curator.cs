using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum CuratorState
{
	start = 0,
	liftTop = 1,
	roomEnter = 2,
	gameEnter = 3,
}

public class Curator : MonoBehaviour
{
	[SerializeField]
	private AudioClip _dialogueNoise;
	[SerializeField]
	public AudioSource _dialogueSource;
	[SerializeField]
	private List<string> dialogueLines;
	
	[SerializeField]
	private TMP_Text textLine;

	[SerializeField]
	private CuratorState state;
	
	[SerializeField]
	UnityEvent onNextGameRound;
	private Transform playerPos;
	
	bool _stillSpeaking = false;
	
    // Start is called before the first frame update
    void Start()
    {
		playerPos = GameObject.Find("Main Camera").transform;
        if(textLine.text == "")
			textLine.text = "Hey!";

		if(state == CuratorState.gameEnter)
		{
			//StartingGame();			
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPos != null)
		{
			transform.LookAt(playerPos);
		}
		if(Vector3.Distance(transform.position, playerPos.position) > 15f)
		{
			Destroy(gameObject);
		}
    }
	
	private void SetText(string t)
	{
		textLine.text = t;
	}
	
	private void SetTextConcatS(string t)
	{
		textLine.text += t;
	}
	
	private void SetTextConcatC(char c)
	{
		textLine.text += c;
	}
	
	public void SetAlive(CuratorState s)
	{
		this.GetComponent<ParticleSystem>().Play();
		
		switch(s)
		{
			case CuratorState.start:
			SetText(dialogueLines[0]);
			break;
			case CuratorState.liftTop:
			SetText(dialogueLines[1]);
			break;
			case CuratorState.roomEnter:
			SetText(dialogueLines[2]);
			break;
			//case CuratorState.gameEnter:
			//StartCoroutine(Dialogue());
			//break;
		}
		
	}
	
	private IEnumerator Dialogue()
	{
		if(dialogueLines.Count > 0)
		{
			//StartCoroutine(PlayAudio());
		}
		yield return new WaitForSeconds(0.1f);
		string text = "";
		
		for(int i = 3; i < dialogueLines.Count; i++)
		{
			text = "";
			var current = dialogueLines[i];
			for(int j = 0; j < current.Length; j++)
			{
				yield return new WaitForSeconds(0.1f);
				SetTextConcatC(current[j]);
			}
			
			yield return new WaitForSeconds(0.1f);
			_stillSpeaking = false;
		}
	}
	
	
	private IEnumerator PlayAudio()
	{
		_stillSpeaking = true;
		
		while(_stillSpeaking)
		{
			_dialogueSource.Stop();
			float randomPitch = Random.Range(0.0f, 1.0f);
			_dialogueSource.clip = _dialogueNoise;
			_dialogueSource.Play();
			while(_dialogueSource.isPlaying)
			{
				yield return null;
			}
		}
	}
	
	private void DisplayEndofRoundText(bool loss)
	{
		if(loss)
		{
			var t = dialogueLines[Random.Range(0, dialogueLines.Count-1)];
			SetText(t);
		}
		else
		{
			Debug.Log("win");
			GameObject.Find("GameThing").GetComponent<GameHandler>().nextRound = true;
		}
	}
	
	public void StartingGame()
	{
		GameObject.Find("GameThing").GetComponent<GameHandler>().endRound.AddListener(DisplayEndofRoundText);
	}
}
