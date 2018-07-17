using UnityEngine;
using System.Collections;

public class GetAnimatedMesh : MonoBehaviour
{

    public SkinnedMeshRenderer meshRenderer;
    public Mesh mesh;
    void Update()
    {
        meshRenderer.BakeMesh(mesh);
    }
}
