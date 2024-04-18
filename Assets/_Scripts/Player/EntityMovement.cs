using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    [SerializeField] int _movementCost;
    public int MovementCost => _movementCost;

    private EntityStamina _entityStamina;

    PlayerStatsData _playerStatsData;
    Rigidbody2D _rb;

    private CameraMovement _cameraM;
    private bool _cameraFinished;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
        _entityStamina = GetComponent<EntityStamina>();
        _cameraM = Camera.main.GetComponent<CameraMovement>();
    }

    void OnEnable()
    {
        _cameraM.OnCameraMovementFinish += CameraDone;
    }
    void OnDisable()
    {
        _cameraM.OnCameraMovementFinish -= CameraDone;
    }


    /// <summary>
    ///  Move the entity in the specified direction
    /// </summary>
    /// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
    public void Move(Vector2 direction)
    {
        if (_entityStamina.CurrentActionPoints <= 0) return;

        var ray = Physics2D.Raycast(_rb.position, direction, 1, LayerMask.GetMask("Walls"));
        if (ray.collider != null && !ray.collider.CompareTag("RoomEdges"))
            return;
        else
        {
            if (ray.collider.CompareTag("RoomEdges"))
            {
                direction.Normalize();
                StartCoroutine(CameraInterupt(direction));
            }
            else
            {
                direction.Normalize();
                _rb.MovePosition(_rb.position + direction);
            }
        }
        
		// Debug.Log("Subtracted " + _movementCost + " movement point from total action points, resulting in " + _playerStatsData.CurrentActionPoints + " total points!");
    }
    IEnumerator CameraInterupt(Vector2 direction)
    {
        _rb.MovePosition(_rb.position + direction / 2);
        //_cameraM.Move();
        yield return new WaitUntil(() => _cameraFinished);
        _rb.MovePosition(_rb.position + direction / 2);
        _cameraFinished = false;
        yield break;
    }

    void CameraDone()
    {
        _cameraFinished = true;
    }

    void SubtractAP()
    {
        _entityStamina.SubtractAP(_movementCost);
    }

    [ContextMenu("Move Up")]
    public void MoveUp(bool subtractAP)
    {
        Move(Vector2.up);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Down")]
    public void MoveDown(bool subtractAP)
    {
        Move(Vector2.down);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Left")]
    public void MoveLeft(bool subtractAP)
    {
        Move(Vector2.left);
        if (subtractAP) SubtractAP();
    }

    [ContextMenu("Move Right")]
    public void MoveRight(bool subtractAP)
    {
        Move(Vector2.right);
        if (subtractAP) SubtractAP();
    }
}
