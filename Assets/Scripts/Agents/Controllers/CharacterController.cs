using KdClistenysPlatform.Controllers;
using KdClistenysPlatform.Managers;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
/// <summary>
/// This script handles the character's gravity and collisions.
/// </summary>
public class CharacterController : MonoBehaviour
{
    public StateController State { get; protected set; }

    [Header("Raycasting")]
    public float MaxDistanceToTheGround = 100f;

    protected float _distanceToTheGround;

    protected Vector2 _raycastOrigin = Vector2.zero;

    public float DistanceToTheGround
    {
        get
        {
            return _distanceToTheGround;
        }
    }

    void Awake()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        State = new StateController();
    }

    protected virtual void ComputeDistanceToTheGround()
    {
        if (MaxDistanceToTheGround <= 0)
        {
            return;
        }

        if (State.IsGrounded)
        {
            _distanceToTheGround = 0f;
            return;
        }

        _raycastOrigin.x = transform.position.x;
        _raycastOrigin.y = transform.position.y;

        RaycastHit2D distanceToTheGroundHit = Physics2D.Raycast(_raycastOrigin, Vector2.down, MaxDistanceToTheGround, LayerManager.ObstaclesLayerMask);

        if (distanceToTheGroundHit)
        {
            _distanceToTheGround = distanceToTheGroundHit.distance;
            return;
        }
        else
        {
            _distanceToTheGround = -1;
        }
    }
}
