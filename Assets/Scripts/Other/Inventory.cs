using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<CreatureInfo> _creatureList = new List<CreatureInfo>();
    public List<CreatureInfo> CreatureList => _creatureList;

    public static Action<CreatureInfo> OnCreatureAdded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddCreature(CreatureInfo creature)
    {
        _creatureList.Add(creature);
        OnCreatureAdded?.Invoke(creature);
    }
}
