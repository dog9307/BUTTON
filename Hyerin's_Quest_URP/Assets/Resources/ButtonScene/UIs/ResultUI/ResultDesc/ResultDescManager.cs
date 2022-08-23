using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDescManager : MonoBehaviour
{
    [SerializeField]
    private ResultDesc[] _descs;

    void Update()
    {
        if (KeyManager.instance.IsOnceKeyDown("retry"))
        {
            BackBeatManager manager = FindObjectOfType<BackBeatManager>();
            manager.Retry();
        }
    }

    public void SetDesc(BackBeatController currentController)
    {
        for (int i = 0; i < _descs.Length; ++i)
        {
            if (i >= currentController.currentSet.resources.Length)
            {
                _descs[i].relativePlayer = null;
                continue;
            }

            _descs[i].relativePlayer = currentController.currentSet.resources[i];
        }
    }
}
