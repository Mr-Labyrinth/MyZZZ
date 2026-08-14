using UnityEngine;
using System;
using System.Collections.Generic;

public class TestMinHeap : MonoBehaviour
{
    void Start()
    {
        var heap = new MinHeap<int>();

        heap.Enqueue(3);
        heap.Enqueue(1);
        heap.Enqueue(4);
        heap.Enqueue(2);

        Debug.Log("出堆顺序（必须从小到大）：");
        while (heap.Count > 0)
        {
            Debug.Log(heap.Dequeue());
        }
    }

}