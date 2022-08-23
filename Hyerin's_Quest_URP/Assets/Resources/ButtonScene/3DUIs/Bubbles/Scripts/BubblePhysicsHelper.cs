using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePhysicsHelper : MonoBehaviour
{
    [SerializeField]
    private BubbleGravityHelper _gravity;
    [SerializeField]
    private float _gravityScale = 0.98f;
    [SerializeField]
    private float _gravityModifier = 1.0f;

    void Start()
    {
        ScreenFader.instance.AddEvent(AnimStart, FaderEventType.Post);
    }

    public void AnimStart()
    {
        Animation anim = GetComponent<Animation>();
        if (anim)
            anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("bubble");
        foreach (var bubble in bubbles)
        {
            Rigidbody rigid = bubble.GetComponent<Rigidbody>();
            if (!rigid) continue;

            rigid.AddForce(_gravity.dir * _gravityScale * _gravityModifier * Time.deltaTime);
        }
    }
}
