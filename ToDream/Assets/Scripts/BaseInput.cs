using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInput : MonoBehaviour
{
    #region Fields
    
	[SerializeField] protected float _vertical;
	[SerializeField] protected float _horizontal;
    
    #endregion
    
    #region Properties
    
	public float _Vertical {get{return _vertical;}}
	public float _Horizontal {get{return _horizontal;}}
    
    #endregion
    
    #region Unity Events
    
	public virtual void Update()
	{
		HandleInput();
	}
    
    #endregion
    
    #region Custom Methods
    
	protected virtual void HandleInput()
	{
		_vertical = Input.GetAxisRaw("Vertical");
		_horizontal = Input.GetAxisRaw("Horizontal");
	}
    
    #endregion
}
