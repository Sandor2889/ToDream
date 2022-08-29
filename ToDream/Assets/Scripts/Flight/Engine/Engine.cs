using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Flight/Engine/Create New Engine")]
public class Engine : ScriptableObject
{
	public float _MaxForce = 200f;
	public float _MaxRPM = 2550f;
	public float _ShutOffSpeed = 2f;
	public AnimationCurve _PowerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
