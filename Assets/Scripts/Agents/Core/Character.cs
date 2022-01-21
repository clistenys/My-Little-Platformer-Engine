using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region variables
    public enum CharacterTypes { Player, AI }
    public enum FacingDirections { Left, Right }
    public enum SpawnFacingDirections { Default, Left, Right }

    public CharacterTypes CharacterType = CharacterTypes.AI;
    public CharacterStates CharacterState { get; protected set; }

    [Header("Direction")]
    public FacingDirections InitialFacingDirection = FacingDirections.Right;
    public SpawnFacingDirections DirectionOnSpawn = SpawnFacingDirections.Default;
    public bool IsFacingRight { get; set; }
    public bool FlipModelOnDirectionChange = true;
    public Vector3 ModelFlipValue = new Vector3(-1, 1, 1);

    [Header("Animator")]
    public Animator CharacterAnimator;
    public bool UseDefaultMecanim = true;
    /// will be performed to make sure animator parameters exist, but lose performance.
    public bool PerformAnimatorSanityChecks = true;
    public bool DisabeAnimatorLogs = false;

    [Header("Model")]
    /// the 'model' used to manipulate the character. 
    public GameObject CharacterModel;
    public GameObject CameraTarget;
    public float CameraTargetSpeed = 5f;

    [Header("Health")]
    public Health CharacterHealth;

    [Header("Airborne")]
    public float AirborneDistance = 0.5f;
    public float AirboneMinimunTime = 0.1f;
    public bool Airborne { get { return (_controller.DistanceToTheGround > AirborneDistance); } }

    // State Machines
    public StateMachine<CharacterStates.MovementStates> MovementStateMachine;
    public StateMachine<CharacterStates.CharacterConditions> ConditionStateMachine;

    public Camera SceneCamera { get; protected set; }
    public InputManager AssociatedInputManager { get; protected set; }
    public Animator _animator { get; protected set; }
    public HashSet<int> _animatorParameters { get; set; }
    public bool CanFlip { get; set; }

    // animation Animationparameters
    protected const string _idleAnimationParameterName = "Idle";
    protected const string _jumpingAnimationParameterName = "Jumping";
    protected const string _groundedAnimationParameterName = "Grounded";
    protected const string _flipAnimationParameterName = "Flip";
    protected const string _xSpeedAnimationParameterName = "xSpeed";
    protected const string _ySpeedAnimationParameterName = "ySpeed";

    protected int _idleAnimationParameter;
    protected int _jumpingAnimationParameter;
    protected int _groundedAnimationParameter;
    protected int _flipAnimationParameter;
    protected int _xSpeedAnimationParameter;
    protected int _ySpeedAnimationParameter;

    protected CharacterController _controller;
    protected SpriteRenderer _spriteRenderer;
    protected Color _initialColor;
    protected CharacterAbility[] _characterAbilities;
    protected bool _spawnDirectionForced = false;
    protected Vector3 _cameraTargetInitialPosition;
    protected Vector3 _cameraOffset = Vector3.zero;
    #endregion

    protected virtual void Awake()
    {
        Initialization();
    }

    public virtual void Initialization()
    {
        MovementStateMachine = new StateMachine<CharacterStates.MovementStates>(this.gameObject);
        ConditionStateMachine = new StateMachine<CharacterStates.CharacterConditions>(this.gameObject);

        MovementStateMachine.ChangeState(CharacterStates.MovementStates.Idle);

        if (InitialFacingDirection == FacingDirections.Left)
        {
            IsFacingRight = false;
        }
        else
        {
            IsFacingRight = true;
        }

        // instantiate camera target
        if (CameraTarget == null)
        {
            CameraTarget = new GameObject();
            CameraTarget.transform.SetParent(this.transform);
            CameraTarget.transform.localPosition = Vector3.zero;
            CameraTarget.name = "CameraTarget";
        }
        _cameraTargetInitialPosition = CameraTarget.transform.localPosition;

        // we get the current input manager
        SetInputManager();
        GetMainCamera();

        // we store our components for further use 
        CharacterState = new CharacterStates();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _controller = this.gameObject.GetComponent<CharacterController>();

        // Store the character abilities in a array
        GetAbilities();

        if(CharacterHealth == null)
        {
            CharacterHealth = this.gameObject.GetComponent<Health>();
        }
        
        CanFlip = true;
        AssignAnimator();

        ForceSpawnDirection();
    }

    public virtual void GetMainCamera()
    {
        if (Camera.main != null)
        {
            SceneCamera = Camera.main.GetComponent<Camera>();
        }
    }

    public void GetAbilities()
    {
        _characterAbilities = this.gameObject.GetComponents<CharacterAbility>();
    }

    public virtual void AssignAnimator()
    {
        if (CharacterAnimator != null)
        {
            _animator = CharacterAnimator;
        }
        else
        {
            _animator = this.gameObject.GetComponent<Animator>();
        }

        if (_animator != null)
        {
            InitializeAnimatorParameters();
        }
    }

    public virtual void SetInputManager()
    {
        if (CharacterType == CharacterTypes.AI)
        {
            AssociatedInputManager = null;
            return;
        }

        AssociatedInputManager = GetComponent<InputManager>();
    }

    public virtual void SetInputManager(InputManager inputManager)
    {
        AssociatedInputManager = inputManager;
    }

    protected virtual void InitializeAnimatorParameters()
    {
        if (_controller == null || _animator == null || AssociatedInputManager == null) { return; }

        _idleAnimationParameter = Animator.StringToHash(_idleAnimationParameterName);
        _groundedAnimationParameter = Animator.StringToHash(_groundedAnimationParameterName);
        _jumpingAnimationParameter = Animator.StringToHash(_jumpingAnimationParameterName);

        _xSpeedAnimationParameter = Animator.StringToHash(_xSpeedAnimationParameterName);
        _ySpeedAnimationParameter = Animator.StringToHash(_ySpeedAnimationParameterName);
    }

    protected virtual void ForceSpawnDirection()
    {
        if ((DirectionOnSpawn == SpawnFacingDirections.Default) || _spawnDirectionForced)
        {
            return;
        }
        else
        {
            _spawnDirectionForced = true;
            if (DirectionOnSpawn == SpawnFacingDirections.Left)
            {
                Face(FacingDirections.Left);
            }
            if (DirectionOnSpawn == SpawnFacingDirections.Right)
            {
                Face(FacingDirections.Right);
            }
        }
    }

    /// <summary>
	/// Forces the character to face right or left
	/// </summary>
    /// <param name="facingDirection">Facing direction.</param>
    public virtual void Face(FacingDirections facingDirection)
    {
        if (!CanFlip)
        {
            return;
        }

        // Flips the character horizontally
        if (facingDirection == FacingDirections.Right)
        {
            if (!IsFacingRight)
            {
                Flip(true);
            }
        }
        else
        {
            if (IsFacingRight)
            {
                Flip(true);
            }
        }
    }

    public virtual void Flip(bool IgnoreFlipOnDirectionChange = false)
    {
        // if we don't want the character to flip, we do nothing and exit
        if (!FlipModelOnDirectionChange && !IgnoreFlipOnDirectionChange)
        {
            return;
        }

        if (!CanFlip)
        {
            return;
        }

        if (!FlipModelOnDirectionChange && IgnoreFlipOnDirectionChange)
        {
            if (CharacterModel != null)
            {
                CharacterModel.transform.localScale = Vector3.Scale(CharacterModel.transform.localScale, ModelFlipValue);
            }
            else
            {
                // if we're sprite renderer based, we revert the flipX attribute
                if (_spriteRenderer != null)
                {
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;
                }
            }
        }

        // Flips the character horizontally
        FlipModel();

        if (_animator != null)
        {
            AnimatorExtensions.SetAnimatorTrigger(_animator, _flipAnimationParameter, _animatorParameters, PerformAnimatorSanityChecks);
        }

        IsFacingRight = !IsFacingRight;
    }

    public virtual void FlipModel()
    {
        if (FlipModelOnDirectionChange)
        {
            if (CharacterModel != null)
            {
                CharacterModel.transform.localScale = Vector3.Scale(CharacterModel.transform.localScale, ModelFlipValue);
            }
            else
            {
                // if we're sprite renderer based, we revert the flipX attribute
                if (_spriteRenderer != null)
                {
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;
                }
            }
        }
    }
}
