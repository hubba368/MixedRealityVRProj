using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorButton : MonoBehaviour
{
	[SerializeField]
	private float threshold;
	[SerializeField]
	private float deadZone;
	
	private ConfigurableJoint _joint;
	private Vector3 _startPos;
	private bool isPressed;
	
	[SerializeField]
	private bool isWithinBounds = false;
	[SerializeField]
	private Rigidbody _rb;
	
	[SerializeField]
	UnityEvent onPressed;
	[SerializeField]
	UnityEvent onReleased;
	
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
		isPressed = false;
		_joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
		if(isWithinBounds)
		{
			_rb.isKinematic = false;
	        if(!isPressed && GetValue() + threshold >= 1)
			{
				OnPress();
			}
			if(isPressed && GetValue() - threshold <= 0)
			{
				OnRelease();
			}		
		}
		else
		{
			//_rb.isKinematic = true;
		}
    }
	
	private float GetValue()
	{
		// check the threshold pos with transform and joint
		// based on a percentage
		var v = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;
		
		// check for edge cases
		if(Mathf.Abs(v) < deadZone)
			v = 0;
		return Mathf.Clamp(v, -1f, 1f);
	}
	
	private void OnPress()
	{
		isPressed = true;
		onPressed.Invoke();
		Debug.Log("pressed");
	}
	
	private void OnRelease()
	{
		isPressed = false;
		onReleased.Invoke();
		Debug.Log("not pressed");
	}
	
	
	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
			isWithinBounds = true;
	}
	
	void OnTriggerExit(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
			isWithinBounds = false;
	}
}

