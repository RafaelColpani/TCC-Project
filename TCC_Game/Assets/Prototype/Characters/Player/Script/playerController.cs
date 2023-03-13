using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Bool
    [Header("Bools")] [Space(5)]
    [SerializeField] bool isAim = false;

    [Header("--")]
    [SerializeField] GameObject target;
    [SerializeField] float mouseSpeed = 1f;

    private void Update() {

        //Bool
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Mouse 0");
            isAim = true;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Mouse 1");
            isAim = false;
        }

    }

    private void FixedUpdate() {
        if(isAim == true){
            TailMove();
        }
    }


    void TailMove(){
        //Vector3 targetPos = target.transform.position;
        
        Vector3 targetPos = Input.mousePosition;
        targetPos.z = mouseSpeed;
        
        target.transform.position = Camera.main.ScreenToWorldPoint(targetPos);
    }
}
