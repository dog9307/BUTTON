using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme4RewardHelper : MonoBehaviour
{
    [SerializeField]
    private Theme4RewardController _controller;

    public void SetHSV(int h, int s, int v, float intensity)
    {
        _controller.SetHSV(h, s, v, intensity);
    }

    public void SetMainTex(Texture tex)
    {
        _controller.SetMainTex(tex);
    }

    public void CardOn(int index)
    {
        _controller.CardOn(index);
    }
}
