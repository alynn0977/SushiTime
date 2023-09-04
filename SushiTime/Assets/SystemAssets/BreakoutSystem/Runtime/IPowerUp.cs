using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp
{
    /// <summary>
    /// Name of the powerup.
    /// </summary>
    public string PowerName { get;}

    /// <summary>
    /// The prefab that should appear in the power up panel.
    /// </summary>
    public GameObject PowerPrefab { get;}

    /// <summary>
    /// How long will this power-up last?
    /// </summary>
    public float Time { get; }
}
