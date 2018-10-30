using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

    public float curveRadius, pipeRadius; 
    public int curveSegCount, pipeSegCount;

    Mesh mesh;
    Vector3[] verts;
    int[] triangles;

    private void Awake(){
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";
        SetVertices();
        SetTriangles();
        mesh.RecalculateNormals();
    }

    void SetVertices(){
        verts = new Vector3[pipeSegCount * curveSegCount * 4];
        float uStep = (2f * Mathf.PI) / curveSegCount;
        CreateFirstQuadRing(uStep);
        int iDelta = pipeSegCount * 4;
        for (int u = 2, i = iDelta; u <= curveSegCount; u ++, i += iDelta){
            CreateQuadRing(u * uStep, i);
        }
        mesh.vertices = verts;
    }

    void SetTriangles() {
        triangles = new int[pipeSegCount * curveSegCount * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4){
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }
        mesh.triangles = triangles;

    }

    void CreateFirstQuadRing(float u){
        float vStep = (2f * Mathf.PI) / pipeSegCount;

        //getting 2 vertices along u
        Vector3 vertA = GetPointOnTorus(0f, 0f);
        Vector3 vertB = GetPointOnTorus(u, 0f);
        for (int v = 1, i = 0; v <=pipeSegCount; v++, i += 4){
            //step along v, grab next pair of verts along u.  assign prev & new points to current quad's verts.
            verts[i] = vertA;
            verts[i + 1] = vertA = GetPointOnTorus(0f, v * vStep);
            verts[i + 2] = vertB;
            verts[i + 3] = vertB = GetPointOnTorus(u, v * vStep);
        }
    }

    void CreateQuadRing(float u, int i)
    {
        float vStep = (2f * Mathf.PI) / pipeSegCount;
        int ringOffset = pipeSegCount * 4;

        Vector3 vert = GetPointOnTorus(u, 0f);
        for (int v = 1; v  <= pipeSegCount; v++, i += 4){
            verts[i] = verts[i - ringOffset + 2];
            verts[i + 1] = verts[i - ringOffset + 3];
            verts[i + 2] = vert;
            verts[i + 3] = vert = GetPointOnTorus(u, v * vStep);
        }
    }

    Vector3 GetPointOnTorus (float u, float v){
        /*Torus can be described w/following function:
         * x = (R + rcos v) cos u
         * y = (R + rcos v) sin u
         * z = r cos v
         * 
         * r = radius of pipe
         * R = rad of curve
         * u = angle along curve in radians
         * v = angle along pipe
         * */
        Vector3 p;

        float r = (curveRadius + pipeRadius * Mathf.Cos(v));
        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = pipeRadius * Mathf.Sin(v);

        return p;
    }

    void OnDrawGizmos(){
        float vStep = (2f * Mathf.PI) / pipeSegCount;
        float uStep = (2f * Mathf.PI) / curveSegCount;

        for (int u = 0; u < curveSegCount; u++){
            for (int v = 0; v < pipeSegCount; v++){
                Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
                Gizmos.color = new Color(
                    1f,
                (float)v / pipeSegCount,
                (float)u / curveSegCount);
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
