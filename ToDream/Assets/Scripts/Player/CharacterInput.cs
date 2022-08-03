using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : BaseInput
{
    #region Fields
    
	[SerializeField] protected float _mouseX;
	[SerializeField] protected float _mouseY;
    
    #endregion
    
    #region Properties
    
	public float _MouseX {get{return _mouseX;}}
	public float _MouseY {get{return _mouseY;}}
    
    #endregion
    
    #region Unity Events
    
	public override void Update()
	{
		base.Update();
		HandleInput();
	}
    
    #endregion
    
    #region Custom Methods
    
	protected override void HandleInput()
	{
		base.HandleInput();
		_mouseX = Input.GetAxis("Mouse X");
		_mouseY = Input.GetAxis("Mouse Y");
	}
    
    #endregion
}
