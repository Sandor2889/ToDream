using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEvent : MonoBehaviour
{
    [SerializeField] private Texture2D _cusorArrow;
    [SerializeField] private Texture2D _cusorHand;

    private void Awake()
    {
        _cusorArrow = Resources.Load<Texture2D>("Cursor/Arrow");
        _cusorHand = Resources.Load<Texture2D>("Cursor/Hand");
    }

    public void OnMouseEnter()
    {
        Cursor.SetCursor(_cusorHand, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(_cusorArrow, Vector2.zero, CursorMode.Auto);
    }
}
