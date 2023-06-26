using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysics : MonoBehaviour
{
	[SerializeField]
	Rigidbody _handRb;
	[SerializeField]
	Transform _handTransform;
	bool start = false;
	
    // Start is called before the first frame update
    void Start()
    {
        if(_handRb != null && _handTransform != null)
		{
			start = true;
		}
    }

	// Update is called once per frame
    void FixedUpdate()
    {
		if(start)
		{
			_handRb.velocity = (_handTransform.position - transform.position) / Time.fixedDeltaTime;
		
			Quaternion rot = _handTransform.rotation * Quaternion.Inverse(transform.rotation);
			rot.ToAngleAxis(out float angle, out Vector3 axis);
			var rotDiff = angle * axis;
			
			_handRb.angularVelocity = rotDiff * Mathf.Deg2Rad / Time.fixedDeltaTime;
		}

    }
}
