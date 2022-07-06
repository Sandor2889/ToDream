using UnityEngine;

public class PlayerMoveInput
{
	private KeyCode _keyJump = KeyCode.Space;
	private KeyCode _keyUI = KeyCode.C;
	
	public float _Vertical { get{return Input.GetAxisRaw("Vertical");} }
	public float _Horizontal { get{return Input.GetAxisRaw("Horizontal");} }
	public KeyCode _KeyJump { get{return _keyJump;} }
	public KeyCode _KeyUI { get{return _keyUI;} }
}
