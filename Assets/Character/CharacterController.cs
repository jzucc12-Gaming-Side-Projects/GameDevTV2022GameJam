using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    #region //Cached components
    private PlayerControls controls = null;
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    #endregion

    #region //Movement parameters
    [SerializeField, Min(0)] private float accelerationTime = 0;
    [SerializeField, Min(0)] private float stopTime = 0;
    [SerializeField, Min(1)] private float maxSpeed = 5;
    [SerializeField] private int bufferFrames = 10;
    #endregion

    #region //Movement state
    private Vector2 movementVector = new Vector2();
    private Vector2 stopVelocity = new Vector2();
    private Vector2 currentVelocity = new Vector2();
    private float stopSpeed = 0;
    private bool isMoving => movementVector != Vector2.zero;
    private float moveTime = 0;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnStopMove;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnStopMove;
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            currentVelocity = movementVector * maxSpeed * GetSpeed(accelerationTime, 0 ,1);
        }
        else
        {
            currentVelocity = stopVelocity * GetSpeed(stopTime, 1, 0);
        }

        if(currentVelocity.x > 0) sr.flipX = true;
        if(currentVelocity.x < 0) sr.flipX = false;
        rb.velocity = currentVelocity;
    }
    #endregion

    #region //Callbacks
    private void OnMove(InputAction.CallbackContext context)
    {
        if(!isMoving)
            LogInput(context);
        else
            StartCoroutine(InputBuffer(context));
    }

    private IEnumerator InputBuffer(InputAction.CallbackContext context)
    {
        for (int ii = 0; ii < bufferFrames; ii++)
            yield return new WaitForEndOfFrame();

        LogInput(context);
    }

    private void LogInput(InputAction.CallbackContext context)
    {
        var newVector = context.ReadValue<Vector2>();
        if(newVector == Vector2.zero) return;
        if (!isMoving) moveTime = Time.time;
        movementVector = newVector.normalized;
    }

    private void OnStopMove(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        stopSpeed = currentVelocity.magnitude;
        stopVelocity = currentVelocity;
        movementVector = Vector2.zero;
        moveTime = Time.time;
    }
    #endregion

    #region //Acceleration
    private float GetSpeed(float refTime, float min, float max)
    {
        if(refTime == 0) return max;
        float dT = (Time.time - moveTime) / refTime;
        float value = Mathf.Lerp(min, max, dT);
        return value;
    }
    #endregion
}
