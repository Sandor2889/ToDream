using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VehicleWheelButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Animator _animator;
	private bool _selected = false;

	//public string _id;
	public VehicleType _vehicleType;
	public string _itemName;
	public Sprite _icon;
	public Text _itemText;
	
	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	public void Test()
	{
		Debug.Log("Test");		
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
	    _animator.SetBool("Hover", true);
		VehicleWheelController._vehicleType = _vehicleType;
		//VehicleWheelController._vehicleID = _id;
	}

    public void OnPointerExit(PointerEventData eventData)
    {
	    _animator.SetBool("Hover", false);
		VehicleWheelController._vehicleType = VehicleType.None;
		//VehicleWheelController._vehicleID = "";
	}
}
