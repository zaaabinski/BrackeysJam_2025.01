using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnLayouts : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navSurfaceBuyer;
    [SerializeField] private NavMeshSurface navSurfaceGhost;

    [Header("LayoutPlaces")] [SerializeField]
    List<GameObject> layouts = new List<GameObject>();

    [Header("Spawn Layouts")] 

    [SerializeField] private GameObject[] libraryTypes;
    [SerializeField] private GameObject[] livingRoomTypes;
    [SerializeField] private GameObject[] kitchenTypes;
    [SerializeField] private GameObject[] topBedroomTypes;
    [SerializeField] private GameObject[] bottomBedroomTypes;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            for (int i = 0; i < layouts.Count; i++)
            {
                GameObject[] selectedRoomType;

                if (i == 0) selectedRoomType = libraryTypes;
                else if (i == 1) selectedRoomType = livingRoomTypes;
                else if (i == 2) selectedRoomType = kitchenTypes;
                else if (i == 3) selectedRoomType = topBedroomTypes;
                else if (i == 4) selectedRoomType = bottomBedroomTypes;
                else selectedRoomType = bottomBedroomTypes;

                if (selectedRoomType.Length > 0)
                {
                    Instantiate(selectedRoomType[Random.Range(0, selectedRoomType.Length)]);
                }

                navSurfaceBuyer.BuildNavMesh();
                navSurfaceGhost.BuildNavMesh();
            }
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < layouts.Count; i++)
            {
                GameObject[] selectedRoomType;

                if (i == 0)
                {
                    selectedRoomType = libraryTypes;
                }
                else if (i == 1)
                {
                    selectedRoomType = libraryTypes;
                    rotation = Quaternion.Euler(0, 270, 0);
                }
                else if (i == 2)
                {
                    selectedRoomType = topBedroomTypes;
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (i == 3)
                {
                    selectedRoomType = topBedroomTypes;
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (i == 4)
                {
                    selectedRoomType = bottomBedroomTypes;
                    rotation = Quaternion.Euler(0, 180, 0);
                    
                }
                else if (i == 5) 
                {
                    selectedRoomType = bottomBedroomTypes;
                    rotation = Quaternion.Euler(0, 270, 0);
                }
                else if (i == 6)
                {
                    selectedRoomType = kitchenTypes;
                    rotation=Quaternion.Euler(0, 180, 0);

                }
                else if (i == 7)
                {
                    selectedRoomType = livingRoomTypes;
                }

                else selectedRoomType = livingRoomTypes;

                if (selectedRoomType.Length > 0)
                {
                    Instantiate(selectedRoomType[Random.Range(0, selectedRoomType.Length)], layouts[i].transform.position,rotation);
                }

                navSurfaceBuyer.BuildNavMesh();
                navSurfaceGhost.BuildNavMesh();
            }
        }
    }
}