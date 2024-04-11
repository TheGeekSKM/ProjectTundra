using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class AttackObjectController : MonoBehaviour
{
    [Header("Attack Object Settings")]
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private int _damage;
    [SerializeField] private EntityType _spawnerType;
    [SerializeField] private WeaponItemData _weaponItemData;

    [Header("Attack Object Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    bool _isInitialized = false;

	//Audio
	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip _audioEnemyHit;
	[SerializeField] private AudioClip _audioPlayerHit;

    private void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    /// <summary>
    ///  Initialize the attack object with the given parameters
    /// </summary>
    /// <param name="speed">attack object movement speed </param>
    /// <param name="lifetime">attack object lifetime</param>
    /// <param name="scale">attack object size</param>
    /// <param name="damage">attack object total damage</param>
    /// <param name="spawnerType">the type of spawner</param>
    public void Initialize(float speed, float lifetime, float scale, int damage, WeaponItemData weaponItemData,EntityType spawnerType)
    {
        _speed = speed;
        _lifetime = lifetime;
        _scale = scale;
        _damage = damage;
        _spawnerType = spawnerType;
        _weaponItemData = weaponItemData;

        transform.localScale = new Vector3(_scale, _scale, _scale);
        _isInitialized = true;
    }

    void FixedUpdate()
    {
        if (_isInitialized) _rigidbody2D.velocity = transform.right * _speed;
        else _rigidbody2D.velocity = Vector2.zero;
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var entityStatsContainer = other.GetComponent<EntityStatsContainer>();
        if (entityStatsContainer != null && entityStatsContainer.EntityType != _spawnerType)
        {
            entityStatsContainer.PlayerStatsData.Health -= _damage;

			//Audio
			if (entityStatsContainer.EntityType == EntityType.Enemy)
				audioSource.clip = _audioEnemyHit;
			else
				audioSource.clip = _audioPlayerHit;
			audioSource.Play();

			if (_weaponItemData.HitEffectPrefab != null)
                Instantiate(_weaponItemData.HitEffectPrefab, transform.position, Quaternion.identity);

			Destroy(gameObject);
		}
    }
}
