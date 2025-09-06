using UnityEngine;

namespace Content.Scripts.SnowSimulation
{
    public class RenderCloudVolume : MonoBehaviour
    {
        public int horizontalStackSize = 20;
        public float cloudHeight;
        public Mesh quadMesh;
        public Material cloudMaterial;
    
        private float offset;
        private Matrix4x4 _matrix4;
    
        void Update()
        {
            cloudMaterial.SetFloat("_midYValue", transform.position.y);
            cloudMaterial.SetFloat("_cloudHeight", cloudHeight);
        
            RenderParams rp = new RenderParams(cloudMaterial);
            offset = cloudHeight / horizontalStackSize / 2f;
            Vector3 startPos = transform.position + (Vector3.up * (offset * horizontalStackSize / 2f));
            for (int i = 0; i < horizontalStackSize; i++)
            {
                _matrix4 = Matrix4x4.TRS(startPos - (Vector3.up * offset * i), transform.rotation, transform.localScale);
                Graphics.RenderMesh(rp, quadMesh, 0, _matrix4);
            }
        }
    }
}
