using System;
using UnityEngine;

public class ChangeTransparency : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material origin1;
    [SerializeField] private Material origin2;
    [SerializeField] private Material transparent;
    
    private bool isPlayerInRoom = false;
    private bool isTransparent = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = true;
            ChangeToTransparent(); // Make sure walls stay transparent in rooms
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            RestoreOriginal(); // Restore walls when leaving a room
        }
    }

    public void ChangeToTransparent()
    {
        if (isTransparent) return; // Avoid unnecessary calls

        Material[] mats = meshRenderer.materials;
        mats[0] = transparent;
        mats[1] = transparent;
        meshRenderer.materials = mats;
        
        isTransparent = true;
    }

    public void RestoreOriginal()
    {
        if (!isTransparent || isPlayerInRoom) return; // Avoid unnecessary calls or restoring while in a room

        Material[] mats = meshRenderer.materials;
        mats[0] = origin1;
        mats[1] = origin2;
        meshRenderer.materials = mats;

        isTransparent = false;
    }

    public bool IsPlayerInRoom()
    {
        return isPlayerInRoom;
    }
}