// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 12/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBrain : EntityBrain
{
    public Player Player => Entity as Player;

    [SerializeField] private int _sceneToLoadOnDeath = 0;

    public new enum AnimatorCondition
    {
        IsIdle,
        IsRun,
        IsTalk,
        IsReload,
        IsShoot,
        IsDead
    }

    [Header("Inputs")]
    [SerializeField] private InputActionReference _pauseInput;
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _shootInput;
    [SerializeField] private InputActionReference _reloadInput;
    [SerializeField] private InputActionReference _minimapInput;
    [SerializeField] private InputActionReference _dashInput;

    public GameObject Minimap
    {
        get
        {
            if (_minimap == null)
                _minimap = GetExternal<Transform>("Minimap").gameObject;

            return _minimap;
        }
    }
    private GameObject _minimap;
    [SerializeField] private GameObject _light;

    private bool _shoot;

    private string[] _animatorConditionNames;

    private void Awake()
    {
        _animatorConditionNames = Enum.GetNames(typeof(AnimatorCondition));
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _pauseInput.action.started += OnPauseStarted;

        _moveInput.action.performed += OnMovePerformed;
        _moveInput.action.canceled += OnMoveCanceled;
        _shootInput.action.performed += OnShootPerformed;
        _shootInput.action.canceled += OnShootCanceled;
        _reloadInput.action.performed += OnReloadPerformed;
        _minimapInput.action.performed += OnMinimapPerformed;
        _minimapInput.action.canceled += OnMinimapCanceled;
        _dashInput.action.started += OnDashStarted;

        Entity.OnDeath += OnDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _pauseInput.action.started -= OnPauseStarted;

        _moveInput.action.performed -= OnMovePerformed;
        _moveInput.action.canceled -= OnMoveCanceled;
        _shootInput.action.performed -= OnShootPerformed;
        _shootInput.action.canceled -= OnShootCanceled;
        _reloadInput.action.performed -= OnReloadPerformed;
        _minimapInput.action.performed -= OnMinimapPerformed;
        _minimapInput.action.canceled -= OnMinimapCanceled;
        _dashInput.action.started -= OnDashStarted;

        Entity.OnDeath -= OnDeath;

        CancelMove();
        CancelShoot();
        CancelMinimap();
    }

    private void OnDeath()
    {
        PlayerManager.Instance.Stopwatch.StopTime();
        SceneManager.Instance.LoadScene(_sceneToLoadOnDeath);
    }

    private void OnPauseStarted(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Game)
        {
            GameManager.Instance.Pause();
        }
        else if (GameManager.Instance.CurrentGameState == GameManager.GameState.Pause)
        {
            GameManager.Instance.Resume();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MovementAction.MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        CancelMove();
    }
    private void CancelMove()
    {
        MovementAction.MoveInput = Vector2.zero;
        SetAnimatorCondition(AnimatorCondition.IsIdle);
        MovementAction.ResetAnimationSpeed();
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        _shoot = context.ReadValue<float>() > 0;

        ShootAction.StartShooting();
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        CancelShoot();
    }
    private void CancelShoot()
    {
        _shoot = false;
        SetAnimatorCondition(AnimatorCondition.IsIdle);
        ShootAction.ResetAnimationSpeed();
        ShootAction.StopShooting();
    }
    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        SetAnimatorCondition(AnimatorCondition.IsReload);
    }

    private void OnMinimapPerformed(InputAction.CallbackContext obj)
    {
        Minimap.SetActive(true);
    }

    private void OnMinimapCanceled(InputAction.CallbackContext obj)
    {
        CancelMinimap();
    }
    private void CancelMinimap()
    {
        Minimap.SetActive(false);
    }

    private void OnDashStarted(InputAction.CallbackContext obj)
    {
        DashingAction.TryDash();
    }

    void Update()
    {
        if (Entity.IsDead)
        {
            SetAnimatorCondition(AnimatorCondition.IsDead);
            return;
        }

        if (!_shoot)
        {
            ShootAction.ResetAnimationSpeed();
        }
        else
        {
            SetAnimatorCondition(AnimatorCondition.IsShoot);
            ShootAction.SetAnimationSpeed();
        }

        if (MovementAction.MoveInput == Vector2.zero)
        {
            MovementAction.ResetAnimationSpeed();
        }
        else
        {
            SetAnimatorCondition(AnimatorCondition.IsRun);
            MovementAction.SetAnimationSpeed();
        }

        // Flip the game object based on the direction of the mouse
        //var aimPosition = Player.Crosshair.transform.position;
        //var direction = aimPosition - transform.position;

        //int degree = direction.x > 0 ? 0 : 180;

        //Render.transform.rotation = Quaternion.Euler(0, degree, 0);
        //Physics.transform.rotation = Quaternion.Euler(0, degree, 0);

        //if (_light != null)
        //    _light.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    public void FixedUpdate()
    {
        if (MovementAction.MoveInput != Vector2.zero)
        {
            MovementAction.FixedMove();
        }
    }

    public void StopReloading()
    {
        SetAnimatorCondition(AnimatorCondition.IsIdle);
    }

    private void SetAnimatorCondition(AnimatorCondition trueCondition)
    {
        var trueConditionName = _animatorConditionNames[(int)trueCondition];
        foreach (var conditionName in _animatorConditionNames)
        {
            Animator.SetBool(conditionName, conditionName == trueConditionName);
        }
    }
}