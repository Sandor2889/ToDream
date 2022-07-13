using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controlable : MonoBehaviour
{
	private Controller _controller;
	private Controlable _controlable;
	protected Controller _Controller { get{return _controller;} }
	protected Controlable _Controlable { get{return _controlable;} set{_controlable = value;} }
	
	private void Awake()
	{
		_controller = FindObjectOfType<Controller>();
	}
	
	public abstract void Move(Vector2 input);
	
	public abstract void Rotate(Vector2 input);
	
	public abstract void Interact();
	
	public abstract void JumpOrBreak(bool keydown);
	
	public abstract void Boost(bool keydown);
}
