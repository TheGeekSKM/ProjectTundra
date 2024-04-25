using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStatsContainer))]
public class EntityMovement : MonoBehaviour
{
    private EntityStamina _entityStamina;

    PlayerStatsData _playerStatsData;
    Rigidbody2D _rb;

    private CameraMovement _cameraM;

	[SerializeField] LayerMask _obstacleLayerMask;

	void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsData = GetComponent<EntityStatsContainer>().PlayerStatsData;
        _entityStamina = GetComponent<EntityStamina>();
        _cameraM = Camera.main.GetComponent<CameraMovement>();
    }


	/// <summary>
	///  Move the entity in the specified direction
	/// </summary>
	/// <param name="direction">Input a Vector2 Direction that you want the entity to move</param>
	void Move(Vector2 direction)
    {
        if (_entityStamina.CurrentActionPoints <= 0) return;

		direction.Normalize();

		var ray = Physics2D.Raycast(_rb.position+(direction/2), direction, 0.5f, _obstacleLayerMask);

		if (ray.collider != null && !ray.collider.CompareTag("RoomEdges")) return;
        else
        {
			if (ray.collider != null && ray.collider.CompareTag("RoomEdges"))
			{
				if (gameObject.CompareTag("Player"))
				{
					_rb.DOMove(_rb.position + (direction*2), _cameraM.duration);
					_cameraM.Move(direction);
				}
				else return;
			}
			else
				_rb.MovePosition(_rb.position + direction);
        }
        
		// Debug.Log("Subtracted " + _movementCost + " movement point from total action points, resulting in " + _playerStatsData.CurrentActionPoints + " total points!");
    }

    void SubtractAP()
    {
		Debug.Log("Subtracting " + _playerStatsData.MovementCost + " AP from entity");
        _entityStamina.SubtractAP(_playerStatsData.MovementCost);
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
