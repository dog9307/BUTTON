using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDesc : MonoBehaviour
{
    [SerializeField]
    private Image _currentImage;
    [SerializeField]
    private Text _count;

    public ResourceObjectPlayerBase relativePlayer { get; set; }

    public float scaleFactor
    {
        get
        {
            if (!relativePlayer)
                return 1.0f;

            return relativePlayer.resultScaleFactor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!relativePlayer)
        {
            _currentImage.rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            _currentImage.sprite = null;
            _currentImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            _count.text = "None";
            return;
        }

        Renderer renderer = relativePlayer.GetComponent<Renderer>();
        if (renderer)
        {
            if (typeof(SpriteRenderer).IsInstanceOfType(renderer))
            {
                SpriteRenderer spriteRenderer = renderer as SpriteRenderer;

                _currentImage.sprite = spriteRenderer.sprite;
                _currentImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _currentImage.rectTransform.sizeDelta = new Vector2(spriteRenderer.sprite.rect.width * scaleFactor, spriteRenderer.sprite.rect.height * scaleFactor);
            }
        }

        _count.text = relativePlayer.inputCount.ToString();
    }
}
