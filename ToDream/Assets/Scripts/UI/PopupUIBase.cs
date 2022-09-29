using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUIBase : MonoBehaviour
{
    public Canvas _canvas;  // Order 속성 참조

    public abstract void OpenControllableUI();

    public abstract void CloseControllableUI();
}
