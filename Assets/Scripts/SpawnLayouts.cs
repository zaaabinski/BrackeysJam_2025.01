using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SpawnLayouts : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navSurfaceBuyer;
    [SerializeField] private NavMeshSurface navSurfaceGhost;

    [Header("LayoutPlaces")] [SerializeField]
    List<GameObject> layouts = new List<GameObject>();

    [Header("Spawn Layouts")] [SerializeField]
    private GameObject[] roomTypeOne;

    [SerializeField] private GameObject[] roomTypeTwo;
    [SerializeField] private GameObject[] roomTypeThree;
    [SerializeField] private GameObject[] roomTypeFour;
    [SerializeField] private GameObject[] roomTypeFive;

    private void Start()
    {
        for (int i = 0; i < layouts.Count; i++)
        {
            GameObject[] selectedRoomType;

            if (i == 0) selectedRoomType = roomTypeOne;
            else if (i == 1) selectedRoomType = roomTypeTwo;
            else if (i == 2) selectedRoomType = roomTypeThree;
            else if (i == 3) selectedRoomType = roomTypeFour;
            else if (i == 4) selectedRoomType = roomTypeFive;
            else selectedRoomType = roomTypeFive;

            if (selectedRoomType.Length > 0)
            {
                Instantiate(selectedRoomType[Random.Range(0, selectedRoomType.Length)]);
            }

            navSurfaceBuyer.BuildNavMesh();
            navSurfaceGhost.BuildNavMesh();
        }
    }
}