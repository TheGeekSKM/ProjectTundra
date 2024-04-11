using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChestManager : Tile
{
    [SerializeField] private ItemContainer _itemContainer;

	//Audio
	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;

    public override void Highlight()
    {
		audioSource.Play();

        ChestViewManager.Instance.OpenChest(_itemContainer, Player.Instance.PlayerStats.ItemContainer);
    }

    public override void Deselect()
    {
        ChestViewManager.Instance.CloseChest();
    }
}
