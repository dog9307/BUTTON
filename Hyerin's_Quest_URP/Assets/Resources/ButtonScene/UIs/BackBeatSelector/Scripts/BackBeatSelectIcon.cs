using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackBeatSelectIcon : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private BackBeatSelector _selector;
    [SerializeField]
    private int _index = 0;
    public int index { get { return _index; } }

    [SerializeField]
    private GameObject _outline;

    private bool _isSelected;
    public bool isSelected { get { return _isSelected; } }

    [SerializeField]
    private SelectArrowMovement _arrow;

    void Start()
    {
        _outline.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _outline.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.SetActive(false);
    }

    public void Select(bool isSound = true, bool isArrowMove = true, bool isIgnoreSelected = false)
    {
        if (BackBeatManager.isChanging) return;
        if (!isIgnoreSelected)
        {
            if (_isSelected)
                return;
        }

        _isSelected = true;
        _selector.ChangeVideo(this);

        _outline.SetActive(true);

        if (_arrow)
            _arrow.MoveStart(GetComponent<RectTransform>(), isArrowMove);

        if (isSound)
            ButtonSFXManager.instance.PlayNext();
    }

    public void Deselect()
    {
        _isSelected = false;
        _outline.SetActive(false);
    }
}
