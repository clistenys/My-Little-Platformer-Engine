using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public enum CharacterConditions
    {
        Normal,
        ControlledMovement,
        Paused,
        Dead
    }

    /// The possible Movement States the character can be in. These usually correspond to their own class, 
    /// but it's not mandatory
    public enum MovementStates
    {
        Null,
        Idle,
        Walking,
        Falling,
        Running,
        Crouching,
        Dashing,
        WallClinging,
        Jumping,
        DoubleJumping,
        WallJumping,
        LadderClimbing
    }
}
