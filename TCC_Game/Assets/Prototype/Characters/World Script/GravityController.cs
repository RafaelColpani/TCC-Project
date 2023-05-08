using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterManager))]
public class GravityController : MonoBehaviour
{
    #region Inspector
    [Tooltip("Sets the value of gravity. Standard is 9.81.")]
    [SerializeField] float gravity = 9.81f;
    [Tooltip("The curve of gravity calculation.")]
    [SerializeField] AnimationCurve gravityCurve;
    #endregion

    #region VARs
    private CharacterManager characterManager;

    Vector3 velocity = Vector3.zero;

    private Transform body;

    private bool isOn = false;
    private bool jumped = false;
    private bool canJump = true;

    private float timer = 0f;
    private float maxTimer = 2f;
    #endregion

    #region Getters Setters
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
    #endregion

    #region Unity Methods
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        this.body = characterManager.Body;
    }

    private void FixedUpdate()
    {
        if(PauseController.GetIsPaused()) return;
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
    #endregion

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
