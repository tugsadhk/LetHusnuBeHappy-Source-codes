using System.Collections.Generic;

using Unity.AI.Navigation;

using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance;

    [SerializeField]
    private List<NavMeshSurface> Surfaces;

    private void Awake()
    {
        Instance = this;
    }

    public void BuildNavMesh()
    {
        foreach (var surface in Surfaces)
        {
            surface.BuildNavMesh();
        }
    }

    public void UpdateNavMesh()
    {
        foreach (var surface in Surfaces)
        {
            surface.UpdateNavMesh(surface.navMeshData);
        }
    }

    public void AddNewNavMeshSurface(NavMeshSurface surface)
    {
        Surfaces.Add(surface);
        surface.BuildNavMesh();
    }

    public void RemoveNavMeshSurface(NavMeshSurface surface)
    {
        Debug.Assert(Surfaces.Contains(surface), "Trying to remove surface that does not exist in the list, this should not possible");
        Surfaces.Remove(surface);
    }
}