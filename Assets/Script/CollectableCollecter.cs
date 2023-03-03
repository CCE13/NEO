using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "CollectableCollector")]
public class CollectableCollecter : ScriptableObject
{

    public event Action ValueChanged;

    public List<CollectableType> collectedItems;
    public int Count => collectedItems.Count;



    public void AddToList(Collectable collectable)
    {

        collectedItems.Add(collectable.CollectableType);
        ValueChanged?.Invoke();
        
    }
    public void AddToList(EnemyCollectable collectable)
    {

        collectedItems.Add(collectable.CollectableType);
        ValueChanged?.Invoke();

    }

    public void Remove(CollectableType collectableType)
    {
        collectedItems.Remove(collectableType);
        ValueChanged?.Invoke();
    }

    private void OnDisable()
    {
        collectedItems.Clear();
    }

    public int CountOf(CollectableType collectableType)
    {
        return collectedItems.Count(t => t == collectableType);
    }
}
