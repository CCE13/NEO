using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    public T[] items;
    int currentItemCount;

    List<T> closedList = new List<T>();

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        if(currentItemCount >= items.Length)
        {
            ResizeHeap();
        }
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item; 
        SortUp(item);
        currentItemCount++;

        closedList.Add(item);
    }

    private void ResizeHeap()
    {
        T[] newNodes = new T[items.Length + 100];

        for (int i = 0; i < items.Length; i++)
        {
            newNodes[i] = items[i];
        }


        items = newNodes;

    }

    void SortUp(T item)
    {
        //finds the parent of the item 
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parent = items[parentIndex];
            //if it gots a higher priotiry it returns one if it got low priority it returns -1 if its same returns 0 
            if(item.CompareTo(parent) > 0)
            {
                //if items have a higher priority swap it with its paren
                Swap(item, parent);
            }
            else
            {
                break;
            }

            //recalculate the index to get the next parent if the loop does not break
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get { return currentItemCount; }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public void ResetNodes()
    {
        foreach (T Node in closedList)
        {
            Node.HeapIndex = 0;
        }
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;    
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;


            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                
                if(childIndexRight < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return ;
            }
        }
    }

    void Swap(T itemA, T itemB)
    {
        //Swaps the location in the array
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        //swap the index values
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex; 
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
