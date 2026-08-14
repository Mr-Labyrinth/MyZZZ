using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ObjectPool<T> : IDisposable where T : class
{
    private readonly List<T> m_List;
    private readonly Func<T> m_CreateFunc;
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;
    private readonly Action<T> m_ActionOnDestroy;
    private int m_MaxSize;
    private bool m_CollectionCheck; // 是否开启重复回收检查器，会影响性能
    private T m_FreshlyReleased;

    /// <summary>
    /// 获取池对象数量
    /// </summary>
    public int CountAll { get; private set; }

    /// <summary>
    /// 活跃对象，也就是池外使用中的对象
    /// </summary>
    public int CountActive { get { return CountAll - CountInactive; } }

    /// <summary>
    /// 非活跃对象，就是池内存的对象
    /// </summary>
    public int CountInactive { get { return m_List.Count + (m_FreshlyReleased != null ? 1 : 0); } }


    /// <summary>
    /// 构造对象池函数
    /// </summary>
    /// <param name="createFunc">在空池获取时，用于创建新对象。</param>
    /// <param name="actionOnGet">在获取对象时执行的初始化函数。</param>
    /// <param name="actionOnRelease">在回收对象时，执行的状态重置函数。</param>
    /// <param name="actionOnDestroy">在满池时，用于销毁对象。</param>
    /// <param name="maxSize">池的最大容量。</param>
    /// <param name="collectionCheck">是否开启重复回收的检查，开启会在回收时产生性能损耗，但更安全。</param>
    /// <param name="defaultCapacity">默认池列表的初始大小</param>
    /// <exception cref="ArgumentNullException">createFunc不能为空，对象池将无法创建对象。</exception>
    /// <exception cref="ArgumentException">maxSize必须大于零。</exception>
    public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, int maxSize = 10000, bool collectionCheck = true, int defaultCapacity = 10)
    {
        if (createFunc == null)
        {
            throw new ArgumentNullException(nameof(createFunc));
        }
        if (maxSize <= 0)
        {
            throw new ArgumentException("maxSize mast be greater than 0." + nameof(maxSize));
        }
        m_List = new List<T>();
        this.m_CreateFunc = createFunc;
        this.m_ActionOnGet = actionOnGet;
        this.m_ActionOnRelease = actionOnRelease;
        this.m_ActionOnDestroy = actionOnDestroy;
        this.m_MaxSize = maxSize;
        this.m_CollectionCheck = collectionCheck;
    }


    /// <summary>
    /// 从池中获取对象。
    /// </summary>
    /// <returns>池中的对象</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get()
    {
        T element;
        if (m_FreshlyReleased != null)
        {
            element = m_FreshlyReleased;
            m_FreshlyReleased = null;
        }
        else if (m_List.Count == 0)
        {
            element = m_CreateFunc();
            CountAll++;
        }
        else
        {
            var idx = m_List.Count - 1;
            element = m_List[idx];
            m_List.RemoveAt(idx);
        }
        m_ActionOnGet?.Invoke(element);
        return element;
    }

    /// <summary>
    /// 回收对象到池内。
    /// </summary>
    /// <param name="element">需要回收的对象。</param>
    /// <exception cref="InvalidOperationException">开启重复回收检查时，出现了重复回收。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Release(T element)
    {
        if(m_CollectionCheck && (m_List.Count>0 || m_FreshlyReleased != null))
        {
            if(ReferenceEquals(element, m_FreshlyReleased))
            {
                throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
            }
            for(int i = 0; i < m_List.Count; i++)
            {
                if(ReferenceEquals(element, m_List[i]))
                {
                    throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
                }
            }
        }

        m_ActionOnRelease?.Invoke(element);
        if(m_FreshlyReleased != null)
        {
            m_FreshlyReleased = element;
        }
        else if(CountInactive < m_MaxSize)
        {
            m_List.Add(element);
        }
        else
        {
            CountAll--;
            m_ActionOnDestroy?.Invoke(element);
        }
    }

    /// <summary>
    /// 清除对象池。
    /// </summary>
    public void Clear()
    {
        if(m_ActionOnDestroy != null)
        {
            if(m_FreshlyReleased != null)
            {
                m_ActionOnDestroy(m_FreshlyReleased);
            }
            foreach (var item in m_List)
            {
                m_ActionOnDestroy(item);
            }
        }

        m_FreshlyReleased = null;
        m_List.Clear();
        CountAll = 0;
    }

    public void Dispose()
    {
        Clear();
    }
}
