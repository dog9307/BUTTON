using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDissolveController : MonoBehaviour
{
    [SerializeField]
    private Renderer[] _renderers;
    private List<Material> _dissolves = new List<Material>();

    [SerializeField]
    private float _dissolveOutTime = 0.5f;

    private Collider _col;
    private Rigidbody _rigid;
    private Vector3 _force;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var ren in _renderers)
            _dissolves.Add(ren.material);

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        Vector3 dir = new Vector3(x, y, z);
        dir = dir.normalized;

        float force = Random.Range(3.0f, 10.0f);

        _force = dir * force;

        StartCoroutine(Exploring());
    }

    IEnumerator Exploring()
    {
        _col = GetComponent<Collider>();
        _col.enabled = false;

        _rigid = GetComponent<Rigidbody>();
        _rigid.isKinematic = true;

        yield return new WaitForSeconds(3.0f);

        _rigid.isKinematic = false;
        _rigid.AddForce(_force, ForceMode.Impulse);

        _col.enabled = true;
    }

    public bool DissolveOut()
    {
        if (!gameObject.activeSelf) return false;

        StartCoroutine(Dissolve());
        return true;
    }

    IEnumerator Dissolve()
    {
        GetComponent<Collider>().enabled = false;

        float currentTime = 0.0f;
        float currentDissolve = 0.0f;
        while (currentTime < _dissolveOutTime)
        {
            float ratio = currentTime / _dissolveOutTime;
            if (ratio > 1.0f)
                ratio = 1.0f;

            currentDissolve = 1.0f - ratio;

            foreach (var dis in _dissolves)
            {
                Color color = dis.GetColor("_BaseColor");
                color.a = currentDissolve;
                dis.SetColor("_BaseColor", color);
            }

            yield return null;

            currentTime += Time.deltaTime;
        }
        currentDissolve = 0.0f;

        foreach (var dis in _dissolves)
        {
            Color color = dis.GetColor("_BaseColor");
            color.a = currentDissolve;
            dis.SetColor("_BaseColor", color);
        }

        gameObject.SetActive(false);
    }
}
