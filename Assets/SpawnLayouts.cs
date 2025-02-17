using System.Collections.Generic;
using UnityEngine;

public class SpawnLayouts : MonoBehaviour
{
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
   }
}
