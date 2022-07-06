using UnityEngine;

public class PlayerMouseInput
{
	[SerializeField] [Range(0, 10)] private int rotCamXAxisSpeed = 5;
	[SerializeField] [Range(0, 5)] private int rotCamYAxisSpeed = 3;
	private float limitMinX = -60.0f;
	private float limitMaxX = 50.0f;
	private float eulerAngleX;
	private float eulerAngleY;
	
	public float _MouseX { get{return Input.GetAxis("Mouse X");} }
	public float _MouseY { get{return Input.GetAxis("Mouse Y");} }
	
	public float UpdateRotate(float mouseX, float mouseY)
	{
		eulerAngleY += mouseX * rotCamYAxisSpeed;
		eulerAngleX -= mouseY * rotCamXAxisSpeed;
		eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
		return eulerAngleY;
	}
	
	private float ClampAngle(float angle, float min, float max)
	{
		if(angle < -360) angle += 360;
		if(angle > 360) angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}
