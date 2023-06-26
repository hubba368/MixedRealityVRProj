using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIKControl : MonoBehaviour
{
	public bool ikActive = false;
	
    private Animator _animator;	
	private Transform _heldObjPos;

	
	void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	public void SetHeldObject(Transform obj)
	{
		_heldObjPos = obj;
	}
	
	private void OnAnimatorIK()
	{
		if(_animator)
		{
			if(ikActive)
			{
				if(_heldObjPos != null)
				{
					_animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
					_animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);
					_animator.SetIKPosition(AvatarIKGoal.RightHand, _heldObjPos.position);
					_animator.SetIKRotation(AvatarIKGoal.RightHand, _heldObjPos.rotation);
				}
			}
			else
			{
				_animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				_animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0);
			}
		}
	}
}
