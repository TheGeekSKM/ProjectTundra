using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EntityAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;

    [SerializeField] EntityStatsContainer _entityStatsContainer;

    private static readonly int RangerIdle = Animator.StringToHash("RangerIdle");
    private static readonly int RangerWalk = Animator.StringToHash("RangerWalk");
    private static readonly int RangerAttack = Animator.StringToHash("RangerAttack");

    void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        // look for the EntityStatsContainer component in the parent GameObject
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponentInParent<EntityStatsContainer>();
    }

    void Start()
    {
        
        
    }

    public void Initialize()
    {
        // set the animator's controller to the one in the EntityStatsContainer
        _animator.runtimeAnimatorController = _entityStatsContainer.PlayerStatsData.AnimatorController;
        HandleAnimation(AnimType.Idle);
    }

    void HandleAnimation(AnimType animType)
    {
        #region Idle Animation
        if (animType == AnimType.Idle)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerIdle, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        break;
                    case PlayerClass.Mage:
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        break;
                    case EnemyClass.Skeleton:
                        break;
                }
            }

            return;
        }
        #endregion

        #region Walk Animation
        if (animType == AnimType.Walk)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerWalk, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        break;
                    case PlayerClass.Mage:
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        break;
                    case EnemyClass.Skeleton:
                        break;
                }
            }

            return;
        }
        #endregion

        #region Attack Animation
        if (animType == AnimType.Attack)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerAttack, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        break;
                    case PlayerClass.Mage:
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        break;
                    case EnemyClass.Skeleton:
                        break;
                }
            }

            return;
        }
        #endregion
        
    }

}
public enum AnimType
{
    Idle,
    Walk,
    Attack
}
