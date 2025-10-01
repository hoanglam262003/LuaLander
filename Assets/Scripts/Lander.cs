using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_SCALE = 0.7f;

    public static Lander Instance { get; private set; }
    public float force = 700f;
    public float turnLeftSpeed = 100f;
    public float turnRightSpeed = -100f;
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickUp;
    public event EventHandler OnFuelPickUp;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float landingAngle;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public enum LandingType
    {
        Success,
        Crash,
        BadAngle,
        WrongArea,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D rb;
    private float fuel;
    private float maxFuel = 10f;
    private float fuelConsumption = 1f;
    private float addFuel = 10f;
    private State state;
    private void Awake()
    {
        Instance = this;
        fuel = maxFuel;
        state = State.WaitingToStart;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    rb.gravityScale = GRAVITY_SCALE;
                    SetState(State.Normal);
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
                break;
            case State.Normal:
                if (fuel <= 0f) return;
                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    ConsumeFuel();
                }
                if (GameInput.Instance.IsUpActionPressed())
                {
                    rb.AddForce(transform.up * Time.deltaTime * force);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                else if (GameInput.Instance.IsLeftActionPressed())
                {
                    rb.AddTorque(turnLeftSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                else if (GameInput.Instance.IsRightActionPressed())
                {
                    rb.AddTorque(turnRightSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad) && !collision.gameObject.TryGetComponent(out BulletCannon bulletCannon))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongArea,
                landingAngle = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        if (collision.gameObject.TryGetComponent(out bulletCannon))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.Crash,
                landingAngle = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        float softLanding = 4f;
        float relativeVelocity = collision.relativeVelocity.magnitude;
        if (relativeVelocity > softLanding)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.Crash,
                landingAngle = 0f,
                landingSpeed = relativeVelocity,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.BadAngle,
                landingAngle = dotVector,
                landingSpeed = relativeVelocity,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float maxScoreLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreLandingAngle;

        float maxScoreLandingSpeed = 100f;
        float landingSpeedScore = (softLanding - relativeVelocity) * maxScoreLandingSpeed;

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            landingAngle = dotVector,
            landingSpeed = relativeVelocity,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickUp fuelPickUp))
        {
            fuel += addFuel;
            if (fuel > maxFuel) fuel = maxFuel;
            OnFuelPickUp?.Invoke(this, EventArgs.Empty);
            fuelPickUp.SelfDestroy();
        }

        if (collider2D.gameObject.TryGetComponent(out Coins coins))
        {
            OnCoinPickUp?.Invoke(this, EventArgs.Empty);
            coins.SelfDestroy();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }

    private void ConsumeFuel()
    {
        fuel -= fuelConsumption * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return rb.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return rb.linearVelocityY;
    }

    public float GetFuel()
    {
        return fuel;
    }

    public float GetFuelAmountNormalized()
    {
        return fuel / maxFuel;
    }

    public State GetCurrentState()
    {
        return state;
    }
}