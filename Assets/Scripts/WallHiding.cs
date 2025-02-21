using UnityEngine;

public class WallHiding : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayer;

    private ChangeTransparency lastObstruction = null;

    void Update()
    {
        DetectObstruction();
    }

    void DetectObstruction()
    {
        if (player == null) return;

        Vector3 cameraPos = transform.position;
        Vector3 direction = (player.position - cameraPos).normalized;
        float distance = Vector3.Distance(player.position, cameraPos);

        // Raycast to detect obstruction
        if (Physics.Raycast(cameraPos, direction, out RaycastHit hit, distance, obstacleLayer))
        {
            GameObject obstructingObject = hit.collider.gameObject;
            ChangeTransparency changeTransparency = obstructingObject.GetComponent<ChangeTransparency>();

            if (changeTransparency != null && changeTransparency != lastObstruction)
            {
                // Restore the last obstruction if it's different
                if (lastObstruction != null)
                {
                    lastObstruction.RestoreOriginal();
                }

                // Apply transparency to the new obstruction
                changeTransparency.ChangeToTransparent();
                lastObstruction = changeTransparency;
            }
        }
        else if (lastObstruction != null)
        {
            // Restore the last obstruction when no object is blocking
            lastObstruction.RestoreOriginal();
            lastObstruction = null;
        }
    }
}