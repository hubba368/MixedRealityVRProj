using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

public class HandAnim : MonoBehaviour
{
	public Animator _animator = null;

	private Collider[] _colliders = null;
	
	[SerializeField] InputActionReference controllerGrip;
	[SerializeField] InputActionReference controllerTrigger;

    void Start()
    {
		// activate hand model colliders
		_colliders = this.GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
		EnableHandColliders();
		
		// create controller action callbacks using Unity Input System
		controllerGrip.action.performed += OnGripPress;
		controllerTrigger.action.performed += OnTriggerPress;
    }
	
	// each "part" of the hand has a collider
	public void EnableHandColliders()
	{	
		for (int i = 0; i < _colliders.Length; ++i)
		{
			Collider collider = _colliders[i];
			collider.enabled = true;
		}
	}
	public void DisableHandColliders()
	{	
		for (int i = 0; i < _colliders.Length; ++i)
		{
			Collider collider = _colliders[i];
			collider.enabled = false;
		}
	}
	
	private void OnGripPress(InputAction.CallbackContext ctx) => _animator.SetFloat("Flex", ctx.ReadValue<float>());
	private void OnTriggerPress(InputAction.CallbackContext ctx) => _animator.SetFloat("Pinch", ctx.ReadValue<float>());

}
