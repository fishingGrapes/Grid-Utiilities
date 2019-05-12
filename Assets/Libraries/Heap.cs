using System;

public class Heap<T> where T : IHeapNode<T>
{
    private T[] nodes;
    private int nCurrentNodeCount;

    public Heap(int MaxHeapSize)
    {
        nodes = new T[MaxHeapSize];
    }

    public void Add(T node)
    {
        node.HeapIndex = nCurrentNodeCount;
        nodes[nCurrentNodeCount] = node;
        SortUp(node);

        nCurrentNodeCount += 1;
    }

    private T firstNode;
    public T RemoveFirst()
    {
        firstNode = nodes[0];
        nCurrentNodeCount -= 1;

        nodes[0] = nodes[nCurrentNodeCount];
        nodes[0].HeapIndex = 0;

        this.SortDown(nodes[0]);

        return firstNode;
    }

    public void Update(T node)
    {
        this.SortUp(node);
    }

    public bool Contains(T node)
    {
        return Equals(nodes[node.HeapIndex], node);
    }

    public int Count { get { return nCurrentNodeCount; } }

    private T parentNode;
    private int nParentIndex;
    private void SortUp(T node)
    {
        nParentIndex = (node.HeapIndex - 1) / 2;

        while (true)
        {
            parentNode = nodes[nParentIndex];
            if (node.CompareTo(parentNode) > 0)
            {
                this.Swap(node, parentNode);
            }
            else
            {
                break;
            }

            nParentIndex = (node.HeapIndex - 1) / 2;
        }
    }

    private int nLeftChildIndex;
    private int nRightChildIndex;
    private int nSwapIndex;
    private void SortDown(T node)
    {
        while (true)
        {
            nLeftChildIndex = node.HeapIndex * 2 + 1;
            nRightChildIndex = node.HeapIndex * 2 + 2;
            nSwapIndex = 0;

            if (nLeftChildIndex < nCurrentNodeCount)
            {
                nSwapIndex = nLeftChildIndex;


                if (nRightChildIndex < nCurrentNodeCount)
                {
                    if (nodes[nLeftChildIndex].CompareTo(nodes[nRightChildIndex]) < 0)
                    {
                        nSwapIndex = nRightChildIndex;
                    }
                }

                if (node.CompareTo(nodes[nSwapIndex]) < 0)
                {
                    this.Swap(node, nodes[nSwapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private int nTempIndex;
    private void Swap(T nodeA, T nodeB)
    {
        nodes[nodeA.HeapIndex] = nodeB;
        nodes[nodeB.HeapIndex] = nodeA;

        nTempIndex = nodeA.HeapIndex;
        nodeA.HeapIndex = nodeB.HeapIndex;
        nodeB.HeapIndex = nTempIndex;
    }
}


public interface IHeapNode<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}