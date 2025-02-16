using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class AnomalyActivation : MonoBehaviour
{
    [SerializeField] private int chanceForSpawn=25;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {   
            Random random = new Random();
            if (random.Next(0, 100) < chanceForSpawn)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
