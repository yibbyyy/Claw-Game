using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class customPost : MonoBehaviour
{
    public Material postProcessMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (postProcessMaterial != null)
        {
            Graphics.Blit(source, destination, postProcessMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}