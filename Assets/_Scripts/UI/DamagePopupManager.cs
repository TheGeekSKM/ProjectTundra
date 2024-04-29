using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance { get; private set; }
    [SerializeField] private DamagePopupController _damagePopupPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DisplayDamage(int damage, Vector3 position)
    {
        // create a new damage popup
        var damagePopup = Instantiate(_damagePopupPrefab, position, Quaternion.identity);
        damagePopup.Setup(damage, position);
    }
}
