using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeDissolve : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private float _dissolveStartTime = 5.0f;
    [SerializeField]
    private float _dissolvingTime = 2.0f;

    [SerializeField]
    private float _startSpeed = 30.0f;

    [SerializeField]
    private Transform _com;

    private bool _isDissolveStart = false;

    [SerializeField]
    private float _force = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.down * _startSpeed;

        if (_com)
            rigid.centerOfMass = _com.position;
        
        _renderer.material.SetFloat("_Ratio", 0.0f);
    }

    public void DissolveOut()
    {
        if (_isDissolveStart) return;

        _isDissolveStart = true;

        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.useGravity = false;
        }

        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        float time = 0.0f;
        float ratio = 0.0f;
        while (time < _dissolvingTime)
        {
            time += Time.deltaTime;
            ratio = time / _dissolvingTime;
            _renderer.material.SetFloat("_Ratio", ratio);

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            Rigidbody other = collision.transform.GetComponent<Rigidbody>();
            if (other)
            {
                Vector3 dir = (other.position - rigid.position).normalized;
                other.AddForce(dir * _force * rigid.velocity.magnitude, ForceMode.Impulse);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("wall"))
                rigid.velocity = Vector3.zero;
        }
    }
}
