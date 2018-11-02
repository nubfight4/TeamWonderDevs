using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoTransparent : MonoBehaviour {
    private Material m_OriMat = null;
    private Color m_OldColor = Color.black;
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.3f;
    private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec
    public Material m_TransMat;

    public void BeTransparent(Material transMat)
    {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OriMat == null)
        {
            // Save the current shader
            m_OriMat = GetComponent<MeshRenderer>().material;
            m_TransMat = transMat;
            GetComponent<MeshRenderer>().material = m_TransMat;
            m_OldColor = GetComponent<MeshRenderer>().material.color;
        }
    }


    void Update()
    {
        if (m_Transparency < 1.0f)
        {      
            Color C = GetComponent<MeshRenderer>().material.color;
            C.a = m_Transparency;
            GetComponent<MeshRenderer>().material.color = C;
        }

        else
         {
            // Reset the shader
            GetComponent<MeshRenderer>().material = m_OriMat;
            GetComponent<MeshRenderer>().material.color = m_OldColor;
            
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}
