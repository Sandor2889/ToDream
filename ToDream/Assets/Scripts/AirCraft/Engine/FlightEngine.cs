using UnityEngine;

public class FlightEngine : MonoBehaviour
{
	[Header("Engine Properties")]
	public Engine _engine;
	private bool _isShutOff = false;
	private float _lastThrottleValue;
	public bool ShutEngineOff
	{
		set { _isShutOff = value; }
	}
	private float _currentRPM;
	public float _CurrentRPM
	{
		get { return _currentRPM; }
	}
	
	public Vector3 CalculateForce(float speed)
	{
		float finalSpeed = Mathf.Clamp01(speed);
		if (!_isShutOff)
		{
			finalSpeed = _engine._PowerCurve.Evaluate(finalSpeed);
			_lastThrottleValue = finalSpeed;
		}
		else
		{
			_lastThrottleValue -= Time.deltaTime * _engine._ShutOffSpeed;
			_lastThrottleValue = Mathf.Clamp01(_lastThrottleValue);
			finalSpeed = _engine._PowerCurve.Evaluate(_lastThrottleValue);
		}
		// rpm
		_currentRPM = finalSpeed * _engine._MaxRPM;
		
		float finalPower = finalSpeed * _engine._MaxForce;
		Vector3 finalForce = transform.forward * finalPower;
		return finalForce;
	}
}
