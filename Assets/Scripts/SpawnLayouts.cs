using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SpawnLayouts : MonoBehaviour
{ 
    [SerializeField] private NavMeshSurface navSurfaceBuyer;
    [SerializeField] private NavMeshSurface navSurfaceGhost;
    
   [Header("LayoutPlaces")]
   [SerializeField] List<GameObject> layouts = new List<GameObject>();

   [Header("Spawn Layouts")] 
   [SerializeField] private GameObject[] roomTypeOne;
   [SerializeField] private GameObject[] roomTypeTwo;
   [SerializeField] private GameObject[] roomTypeThree;
   [SerializeField] private GameObject[] roomTypeFour;
   
   private void Start()
   {
      foreach (GameObject roomType in layouts)
      {
          Instantiate(roomTypeOne[Random.Range(0,roomTypeOne.Length)], roomType.transform.position, Quaternion.identity);
      }
      navSurfaceBuyer.BuildNavMesh();
      navSurfaceGhost.BuildNavMesh();
   }
}
