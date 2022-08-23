using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField]
    private BubbleDissolveController[] _ones;
    [SerializeField]
    private BubbleDissolveController[] _twoes;
    [SerializeField]
    private BubbleDissolveController[] _threes;
    [SerializeField]
    private BubbleDissolveController[] _fours;

    private int _oneIndex;
    private int _twoIndex;
    private int _threeIndex;
    private int _fourIndex;

    void Start()
    {
        Shuffle(_ones);
        Shuffle(_twoes);
        Shuffle(_threes);
        Shuffle(_fours);

        _oneIndex = -1;
        _twoIndex = -1;
        _threeIndex = -1;
        _fourIndex = -1;
    }

    private void Shuffle(BubbleDissolveController[] bubbles)
    {
        for (int i = 0; i < 777; ++i)
        {
            int sour = Random.Range(0, bubbles.Length);
            int dest = Random.Range(0, bubbles.Length);

            BubbleDissolveController temp = bubbles[dest];
            bubbles[dest] = bubbles[sour];
            bubbles[sour] = temp;
        }
    }

    void Update()
    {
        BubbleDissolveController bubble = null;
        if (KeyManager.instance.IsOnceKeyDown("bubble_one"))
            bubble = GetRandomOne(_ones, ref _oneIndex);
        if (KeyManager.instance.IsOnceKeyDown("bubble_two"))
            bubble = GetRandomOne(_twoes, ref _twoIndex);
        if (KeyManager.instance.IsOnceKeyDown("bubble_three"))
            bubble = GetRandomOne(_threes, ref _threeIndex);
        if (KeyManager.instance.IsOnceKeyDown("bubble_four"))
            bubble = GetRandomOne(_fours, ref _fourIndex);

        if (bubble)
            bubble.DissolveOut();
    }

    private BubbleDissolveController GetRandomOne(BubbleDissolveController[] bubbles, ref int index)
    {
        BubbleDissolveController bubble = null;

        int checkCount = 0;
        do
        {
            index = (index + 1) % bubbles.Length;
            checkCount++;

            bubble = bubbles[index];

        } while (checkCount < bubbles.Length && !bubble);

        return bubble;
    }
}
