using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshUtilities : MonoBehaviour
{
    public Vector3 GetRandomPointOnNavmesh(NavMeshSurface navmeshSurface)
    {
        NavMeshData navMeshData = navmeshSurface.navMeshData;
        
        if (navMeshData == null)
        {
            Debug.LogError("NavMesh data isn't available for some reason. Maybe you forgot to bake it");
            return Vector3.zero;
        }

        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        Vector3[] vertices = triangulation.vertices;
        Bounds bounds = new Bounds(vertices[0], Vector3.zero);

        foreach (var vertex in vertices)
        {
            bounds.Encapsulate(vertex);
        }

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        Debug.LogWarning("The point chosen was invalid");
        return randomPoint;
    }
}
