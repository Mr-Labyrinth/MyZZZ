using System;
using System.Collections.Generic;
using UnityEngine;

public class DelayManager
{
    public static DelayManager Instance { get; } = new DelayManager();

    private struct TimerEntry : IComparable<TimerEntry>
    {
        public double ExecuteTime;
        public long TaskId;
        public int CompareTo(TimerEntry other) => ExecuteTime.CompareTo(other.ExecuteTime);
    }

    private readonly MinHeap<TimerEntry> scaledHeap = new();
    private readonly MinHeap<TimerEntry> unscaledHeap = new();
    private readonly Dictionary<long, Action> tasks = new();
    private long nextTaskId = 1;

    private DelayManager() { }

    public void Update()
    {
        ProcessHeap(scaledHeap, Time.timeAsDouble);
        ProcessHeap(unscaledHeap, Time.realtimeSinceStartupAsDouble);
    }

    public long AddScaledDelay(float delay, Action callback)
    {
        if (callback == null) return -1;
        double time = Time.timeAsDouble + delay;
        return AddTask(time, callback, scaledHeap);
    }

    public long AddUnScaledDelay(float delay, Action callback)
    {
        if(callback == null) return -1;
        double time = Time.realtimeSinceStartupAsDouble + delay;
        return AddTask(time, callback, unscaledHeap);
    }

    public void Cancel(long taskId) { tasks.Remove(taskId); }

    public void ClearAll()
    {
        scaledHeap.Clear();
        unscaledHeap.Clear();
        tasks.Clear();
    }

    private long AddTask(double time, Action callback, MinHeap<TimerEntry> heap)
    {
        long id = nextTaskId++;
        tasks[id] = callback;
        heap.Enqueue(new TimerEntry { ExecuteTime = time, TaskId = id });
        return id;
    }

    private void ProcessHeap(MinHeap<TimerEntry> heap, double currentTime)
    {
        while(heap.Count > 0 && heap.Peek().ExecuteTime <= currentTime)
        {
            var entry = heap.Dequeue();
            if(tasks.TryGetValue(entry.TaskId, out var callback))
            {
                tasks.Remove(entry.TaskId);
                callback?.Invoke();
            }
        }
    }
}
