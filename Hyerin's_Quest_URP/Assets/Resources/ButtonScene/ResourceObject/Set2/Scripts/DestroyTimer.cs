using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 3.0f;

    [SerializeField]
    private float _dissolvingTime = 0.3f;

    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        Invoke("DestroyStart", _lifeTime);
    }

    void DestroyStart()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(Dissolving());
    }

    IEnumerator Dissolving()
    {
        float currentTime = 0.0f;
        while (currentTime < _dissolvingTime)
        {
            currentTime += Time.deltaTime;

            float ratio = currentTime / _dissolvingTime;
            ratio = (ratio > 1.0f ? 1.0f : ratio);

            _renderer.material.SetFloat("_Ratio", ratio);

            yield return null;
        }
        _renderer.material.SetFloat("_Ratio", 1.0f);

        Destroy(gameObject);
    }
}
