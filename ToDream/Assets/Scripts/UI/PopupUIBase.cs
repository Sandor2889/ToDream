using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUIBase : MonoBehaviour
{
    public Canvas _canvas;  // Order �Ӽ� ����

    public abstract void OpenControllableUI();

    public abstract void CloseControllableUI();
}
