using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private Transform[] _createPos;
    private float _scaleFactor;

    public bool isReverseMove { get; set; }

    [SerializeField]
    private float _zSpeed = 5.0f;
    [SerializeField]
    private float _zMax = 15.0f;
    [SerializeField]
    private float _zMin = -5.0f;

    void Start()
    {
        float fov = Camera.main.fieldOfView;
        float angle = fov / 2.0f;

        float z = 8.18f + _zMax;
        float y = z * Mathf.Tan(angle * Mathf.Deg2Rad);
        float x = ((float)Screen.width / (float)Screen.height) * y;

        for (int i = 0; i < _createPos.Length; ++i)
        {
            Vector3 pos = _createPos[i].position;
            if (i % 2 == 0)
                pos.x = -x;
            else
                pos.x = x;
            _createPos[i].position = pos;
        }
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.z -= _zSpeed * Time.deltaTime;
        if (pos.z < _zMin)
            pos.z = _zMax;
        transform.localPosition = pos;
    }

    public void Init(float scale)
    {
        _scaleFactor = scale;
        isReverseMove = (Random.Range(0, 2) % 2 == 0);
    }

    public void CreateObject(List<GameObject> trains)
    {
        for (int i = 0; i < _createPos.Length; ++i)
        {
            GameObject newTrain = Instantiate(_prefab);
            newTrain.transform.position = _createPos[i].position;
            newTrain.transform.parent = transform;
            newTrain.transform.localScale = new Vector3(_scaleFactor * 0.25f, _scaleFactor * 0.25f, 1.0f);

            TrainController controller = newTrain.GetComponent<TrainController>();
            if (controller)
            {
                if (i % 2 == 0)
                    controller.isMoveToRight = !isReverseMove;
                else
                    controller.isMoveToRight = isReverseMove;
            }

            if (isReverseMove)
            {
                Vector3 newPos = newTrain.transform.position;
                newPos.x *= -1.0f;
                newTrain.transform.position = newPos;
            }


            if (trains != null)
                trains.Add(newTrain);
        }
    }
}
