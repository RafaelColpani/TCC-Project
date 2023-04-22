using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderInteractive : MonoBehaviour
{
    private GameObject playerTarget;
    [SerializeField] Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("TargetPlayer") == null) return;

        playerTarget = GameObject.FindWithTag("TargetPlayer");
        print("Tag:" + playerTarget);

        Vector3 trackerPos = playerTarget.GetComponent<Transform>().position;
        mat.SetVector("_Player", trackerPos);
    }
}
