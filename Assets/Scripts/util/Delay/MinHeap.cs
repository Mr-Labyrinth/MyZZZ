using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MinHeap<T> where T : IComparable<T>
{
    private readonly List<T> _heap = new();
    public int Count => _heap.Count;

    public void Enqueue(T element)
    {
        _heap.Add(element);
        BubbleUp(_heap.Count - 1); // 从最后一个元素开始执行上滤操作
    }

    public T Peek() => _heap[0];

    public T Dequeue()
    {
        int lastIndex = _heap.Count - 1;
        T frontItem = _heap[0]; // 记录堆顶元素
        _heap[0] = _heap[lastIndex];   // 将最后一个元素移动到堆顶
        _heap.RemoveAt(lastIndex);  // 移除原来的最后元素
        if(_heap.Count > 0)
        {
            BubbleDown(0);     // 从堆顶进行下沉操作
        }
        return frontItem; // 返回出堆元素
    }

    public void Clear() => _heap.Clear();

    private void BubbleUp(int index)
    {
        while(index > 0)
        {
            int parentIndex = (index - 1)/2; // 找到父节点，此算法无需考虑自身是左还是右
            if (_heap[index].CompareTo(_heap[parentIndex]) >= 0) break; // 如果自己比父节点大就退出循环

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void BubbleDown(int index)
    {
        int lastIndex = _heap.Count-1;
        while (true)
        {
            int leftChildIndex = index * 2 + 1; // 获取左子结点
            int rightChildIndex = index * 2 + 2; // 获取右子节点
            int smallestIndex = index;           // 设置小节点

            // 这段判断用于获取两个子叶中的较小节点
            if(leftChildIndex <= lastIndex && _heap[leftChildIndex].CompareTo(_heap[smallestIndex]) < 0) // 先和左叶子比较，完全二叉树的要求
                smallestIndex = leftChildIndex;
            if(rightChildIndex <= lastIndex && _heap[rightChildIndex].CompareTo(_heap[smallestIndex]) < 0) // 和右叶子比较
                smallestIndex = rightChildIndex;

            if (smallestIndex == index) break;// 如果比较后没有交换就退出循环

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }

    private void Swap(int a, int b)
    {
        (_heap[a], _heap[b]) = (_heap[b], _heap[a]);
    }
}
