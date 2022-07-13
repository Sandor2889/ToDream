using UnityEngine;

public class PlayerMoveInput
{
	private KeyCode _keyJump = KeyCode.Space;
	
	public float _Vertical { get{return Input.GetAxisRaw("Vertical");} }
	public float _Horizontal { get{return Input.GetAxisRaw("Horizontal");} }
	public KeyCode _KeyJump { get{return _keyJump;} }
}
