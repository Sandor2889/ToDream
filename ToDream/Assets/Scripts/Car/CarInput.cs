using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInput : BaseInput
{
    #region Fields
    
	private bool _boostKey = false;
    
    #endregion
    
    #region Properties
    
	public bool _BoostKey {get{return _boostKey;}}
    
    #endregion
    
    #region Unity Events
    
	public override void Update()
	{
		base.Update();
	}
    
    #endregion
    
    #region Custom Methods
    
	protected override void HandleInput()
	{
		base.HandleInput();
	}
    
    #endregion
}
