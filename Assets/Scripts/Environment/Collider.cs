using UnityEngine;

public class ColliderAssigner : MonoBehaviour
{
    public string targetLayer = "Ground";

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer(targetLayer))
            {
                AssignCollider(obj);
            }
        }
    }

    void AssignCollider(GameObject obj)
    {
        if (obj.GetComponent<Collider>() != null) return; // Skip if collider exists

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh != null && mesh.name.ToLower().Contains("sphere"))
            {
                obj.AddComponent<SphereCollider>();
            }
            else if (mesh != null && mesh.name.ToLower().Contains("capsule"))
            {
                obj.AddComponent<CapsuleCollider>();
            }
            else
            {
                obj.AddComponent<MeshCollider>();
            }
        }
        else if (obj.GetComponent<RectTransform>() != null)
        {
            obj.AddComponent<BoxCollider>();
        }
        else
        {
            obj.AddComponent<BoxCollider>();
        }
    }
}
