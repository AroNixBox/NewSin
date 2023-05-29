using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //assign all renderers through inspector
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private Color color = Color.white;

    //helper list to cache all materials of this obj
    private List<Material> materials;

    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }
    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //Need to enable emission
                material.EnableKeyword("_EMISSION");
                //Before color can be set.
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //we can disable emission if we dont want to use emission color anywhere else.
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
