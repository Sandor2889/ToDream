using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _cusorArrow;
    [SerializeField] private Texture2D _cusorHand;

    private void Awake()
    {
        _cusorArrow = Resources.Load<Texture2D>("Cursor/Arrow");
        _cusorHand = Resources.Load<Texture2D>("Cursor/Hand");
        CursorVisible(true);
    }

    public void CursorVisible(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.SetCursor(_cusorArrow, Vector2.zero, CursorMode.Auto);
        }

        Cursor.visible = visible;
    }
}
