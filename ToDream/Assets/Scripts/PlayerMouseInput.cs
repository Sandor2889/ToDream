using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseInput : MonoBehaviour
{
	[SerializeField] [Range(0, 10)] private int rotCamXAxisSpeed = 5;
	[SerializeField] [Range(0, 5)] private int rotCamYAxisSpeed = 3;
	private float limitMinX = -60.0f;
	private float limitMaxX = 50.0f;
	private float eulerAngleX;
	private float eulerAngleY;
	
	public void UpdateRotate(float mouseX, float mouseY)
	{
		eulerAngleY += mouseX * rotCamYAxisSpeed;
		eulerAngleX -= mouseY * rotCamXAxisSpeed;
		eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
		transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
	}
	
	private float ClampAngle(float angle, float min, float max)
	{
		if(angle < -360) angle += 360;
		if(angle > 360) angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}
