using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    [Header("Permissions")]
    public bool AbilityPermitted = true;
    public CharacterStates.MovementStates[] BlockingMovementStates;
    public CharacterStates.CharacterConditions[] BlokingConditions;
    public virtual bool AbilityAutorized
    {
        get
        {
            if (_character != null)
            {
                if ((BlockingMovementStates != null) && (BlockingMovementStates.Length > 0))
                {
                    for (int i = 0; i < BlockingMovementStates.Length; i++)
                    {
                        if (BlockingMovementStates[i] == _character.MovementStateMachine.CurrentState)
                        {
                            return false;
                        }
                    }
                }
            }
            return AbilityPermitted;
        }
    }

    protected Character _character;
    protected Health _health;
    protected CharacterController _controller;
    protected InputManager _inputManager;
    protected Animator _animator;
    protected CharacterStates _state;
    protected bool _abilityInitialized = false;
    protected float _verticalInput;
    protected float _horizontalInput;
}
