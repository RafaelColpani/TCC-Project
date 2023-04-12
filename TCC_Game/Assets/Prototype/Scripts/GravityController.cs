using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Tooltip("Sets the value of gravity. Standard is 9.81.")]
    [SerializeField] float gravity = 9.81f;
    [Tooltip("The body to be affected by gravity.")]
    [SerializeField] Transform body;
    [Tooltip("The curve of gravity calculation.")]
    [SerializeField] AnimationCurve gravityCurve;

    Vector3 velocity = Vector3.zero;
    private bool isOn = false;
    private bool jumped = false;
    float timer = 0f;
    private float maxTimer = 2f;

    public Vector3 Velocity
    {
        get { return velocity; }
        set 
        {
            isOn = true;
            jumped = true;
            velocity = value; 
        }
    }

    public bool Jumped
    {
        get { return jumped; }
        set { jumped = value; }
    }

    private void Update()
    {
        if (!isOn)
        {
            velocity.y = 0;
            timer = 0f;
            return;
        }

        if (timer < maxTimer)
        {
            timer += Time.deltaTime;
        }
        
        velocity.y -= gravityCurve.Evaluate(gravity * timer);
        body.transform.Translate(velocity * Time.deltaTime);
    }

    #region isOn Aux
    public bool GetIsOn()
    {
        return isOn;
    }

    public void ToggleIsOn()
    {
        isOn = !isOn;
    }

    public void SetIsOn(bool value)
    {
        isOn = value;
    }
    #endregion
}
