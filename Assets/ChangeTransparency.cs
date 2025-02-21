using UnityEngine;

public class ChangeTransparency : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material origin1;
    [SerializeField] private Material origin2;
    [SerializeField] private Material transparent;

    private bool isTransparent = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
        if (!isTransparent) return; // Avoid unnecessary calls

        Material[] mats = meshRenderer.materials;
        mats[0] = origin1;
        mats[1] = origin2;
        meshRenderer.materials = mats;

        isTransparent = false;
    }
}