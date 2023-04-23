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
    private bool canJump = true;

    float timer = 0f;
    private float maxTimer = 2f;

    public Vector3 Velocity
    {
        get { return velocity; }
        set 
        {
            if (!canJump) return;

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

    private void FixedUpdate()
    {
        if(PauseController.isPaused) return;
        if (!isOn)
        {
            velocity.y = 0;
            timer = 0f;
            return;
        }

        if (timer < maxTimer)
        {
            timer += Time.fixedDeltaTime;
        }
        
        velocity.y -= gravityCurve.Evaluate(gravity * timer);
        body.transform.Translate(velocity * Time.fixedDeltaTime);
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

    #region Velocity Aux
    public void SetYVelocity(float value)
    {
        velocity = new Vector3(velocity.x, value, velocity.z);
    }

    public void SetCanJump(bool value = false)
    {
        canJump = value;
    }
    #endregion
}
