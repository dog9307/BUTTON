using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public bool isMoveToRight { get; set; }

    [SerializeField]
    private float _speed = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if (isMoveToRight)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);

            if (transform.position.x > 35.0f)
                Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);

            if (transform.position.x < -35.0f)
                Destroy(gameObject);
        }

        Vector3 pos = transform.position;
        pos.z = transform.parent.position.z;
        transform.position = pos;
    }
}
