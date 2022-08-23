using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartButtonObjectManager : MonoBehaviour
{
    [SerializeField]
    private List<StartButtonObject> _buttons;

    private bool _prevPressed = false;

    public UnityEvent OnPressed;

    // Update is called once per frame
    void Update()
    {
        bool currentPress = IsAllButtonPressed();
        if (!_prevPressed && currentPress)
        {
            if (OnPressed != null)
                OnPressed.Invoke();
        }
    }

    bool IsAllButtonPressed()
    {
        foreach (var b in _buttons)
        {
            if (!b.isPressed)
                return false;
        }

        return true;
    }
}
