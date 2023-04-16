using UnityEngine;

public class SphereExploder : MonoBehaviour
{
    public int numSlices = 10;
    public float explosionForce = .1f;
    public float explosionRadius = 5f;

    private void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < numSlices; i++)
        {
            GameObject slice = new GameObject("Slice " + i);
            slice.transform.position = transform.position;
            slice.transform.parent = transform;

            MeshFilter sliceMF = slice.AddComponent<MeshFilter>();
            MeshRenderer sliceMR = slice.AddComponent<MeshRenderer>();
            Material sliceMaterial = new Material(Shader.Find("Standard"));
            sliceMR.material = sliceMaterial;

            Mesh sliceMesh = new Mesh();
            sliceMF.mesh = sliceMesh;

            Vector3[] sliceVertices = new Vector3[vertices.Length];
            int[] sliceTriangles = new int[triangles.Length];

            for (int j = 0; j < vertices.Length; j++)
            {
                Vector3 vertex = vertices[j];
                float angle = Mathf.Atan2(vertex.y, vertex.x);
                float distance = Mathf.Sqrt(vertex.x * vertex.x + vertex.y * vertex.y);
                float sliceAngle = angle + (i * Mathf.PI * 2 / numSlices);
                Vector3 sliceVertex = new Vector3(Mathf.Cos(sliceAngle) * distance, Mathf.Sin(sliceAngle) * distance, vertex.z);
                sliceVertices[j] = sliceVertex;
            }

            for (int j = 0; j < triangles.Length; j += 3)
            {
                int triangleIndex = j / 3;
                sliceTriangles[j] = triangles[j];
                sliceTriangles[j + 1] = triangles[j + 1];
                sliceTriangles[j + 2] = triangles[j + 2];

                Vector3 triangleCenter = (vertices[triangles[j]] + vertices[triangles[j + 1]] + vertices[triangles[j + 2]]) / 3;
                Vector3 sliceCenter = new Vector3(0, 0, triangleCenter.z);
                float distanceToCenter = Vector3.Distance(sliceCenter, triangleCenter);
                if (distanceToCenter < explosionRadius)
                {
                    Vector3 explosionDirection = triangleCenter - sliceCenter;
                    float explosionDistance = explosionRadius - distanceToCenter;
                    float explosionPower = explosionForce * (explosionDistance / explosionRadius);
                    Vector3 explosionForceVector = explosionDirection.normalized * explosionPower;
                    //print(slice)
                    if (slice.GetComponent<Rigidbody>() == null)
                    {
                        Rigidbody rb = slice.AddComponent<Rigidbody>();
                        print(rb);
                        print(slice.name);
                        rb.AddForce(explosionForceVector, ForceMode.Impulse);
                    }
                        
                }
            }

            sliceMesh.vertices = sliceVertices;
            sliceMesh.triangles = sliceTriangles;
            sliceMesh.RecalculateNormals();
        }

        Destroy(gameObject, 3);
    }
}