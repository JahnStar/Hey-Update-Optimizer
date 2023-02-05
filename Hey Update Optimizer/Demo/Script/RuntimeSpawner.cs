// Github: @JahnStar
using JahnStar.Optimization;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSpawner : MonoBehaviour
{
    public bool spawnRuntime;
    public GameObject clock;
    public float distance = 0.5f;
    public int groupCount, membersPerGroup;
    public List<GameObject> clones;
    private void Awake()
    {
        if (spawnRuntime) Create();
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        for (int i = 0; i < clones.Count; i++) DestroyImmediate(clones[i]);
        clones.Clear();
    }
    [ContextMenu("Create")]
    public void Create()
    {
        Clear();
        for (int i = 0; i < membersPerGroup; i++)
            for (int s = 0; s < groupCount; s++)
                clones.Add(Instantiate(clock, new Vector3(clock.transform.position.x + distance * s, clock.transform.position.y, clock.transform.position.z + distance * i), clock.transform.rotation, base.transform));
        DestroyImmediate(clones[0]);
        clones.RemoveAt(0);
        HeyUpdateManager.ReLoad();
    }
}
