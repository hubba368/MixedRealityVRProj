using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	
	[SerializeField]
	private Transform _bottomPos, _topPos;
	[SerializeField]
	private float _moveSpeed;
	[SerializeField]
	private GameObject _player;
	
	private bool _isMovingUp = false;
	private bool _reachedDest = false;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_player == null)
		{
			_player = GameObject.Find("XR Origin");
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void MoveElevator()
	{
		_isMovingUp = !_isMovingUp;
		UpdatePlayerParent(true);
	}
	
	private void UpdatePlayerParent(bool flag)
	{
		if(flag)
		{
			_player.gameObject.transform.SetParent(this.gameObject.transform);
		}
		else
		{
			_player.transform.parent = null;
		}
	}
	
	private void FixedUpdate()
	{
		if(!_isMovingUp)
		{
			transform.position = Vector3.MoveTowards(transform.position, _bottomPos.position, _moveSpeed * Time.deltaTime);
			if(transform.position == _bottomPos.position)
			{
				UpdatePlayerParent(false);		
			}
		}
		else if(_isMovingUp)
		{
			transform.position = Vector3.MoveTowards(transform.position, _topPos.position, _moveSpeed * Time.deltaTime);
			if(transform.position == _topPos.position)
			{
				UpdatePlayerParent(false);	
			}
		}
	}
}
