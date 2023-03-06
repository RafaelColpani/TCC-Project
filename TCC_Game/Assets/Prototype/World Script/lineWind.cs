using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

#region RequireComponent
    [RequireComponent(typeof(LineRenderer))]
#endregion

public class lineWind : MonoBehaviour
{
    [Header("Script windManager ")] [Space(5)]
    public windManager script;
    public float _intensity;
    public Vector3 _position;
    [Space(10)]

    [Header("LineRenderer Component")]
    LineRenderer lineRender; 
    
    [Header("Join")] [Space(5)]
    //[HeaderPlus(" ", "Joins", (int)HeaderPlusColor.green)]
    [SerializeField] int length;
    [Tooltip(" É o segmento das vertices ")] [SerializeField] Vector3[] joinPoints; 
    [Tooltip(" // ")] [SerializeField] Vector3[] joinPointV; 
    [Space(10)]

    [Header("Distance")] [Space(5)] 
    [SerializeField] Transform targetDir;
    [Tooltip(" distancia do target ate o spawn do lineRender / default = 0.2 ")] [Range(0f, 1f)] [SerializeField] float targetDist;
    [Space(5)]

    [Header("Speed")]
    [Tooltip(" speed do lineRender / default = 0.2 ")] [Range(0f, 1f)] [SerializeField] float SmoothSpeed;

    private void Start() 
    {
        lineRender = GetComponent<LineRenderer>();
    
        //
       lineRender.positionCount = length;
       joinPoints = new Vector3[length];
       joinPointV = new Vector3[length];
    } 

    private void Update() 
    {
        //Variaveis para do script "windManager"
        _intensity = script.intensity;
        _position = script.position;

        //this.Log(_intensity);
        print("_intensity , _position [ " + _intensity + " , " + _position + " ]");

        //Criação dos joins do line render + o line viewer
        joinPoints[0] = targetDir.position;

        for(int i = 1; i < joinPoints.Length; i++){
            joinPoints[i] = Vector3.SmoothDamp(joinPoints[i], joinPoints[i - 1] + targetDir.up * 1.0f * targetDist, ref joinPointV[i], SmoothSpeed);
            
            // wind
            
        }
        
        lineRender.SetPositions(joinPoints);


    }
}
