using UnityEngine;
public enum PowerUpType
{
    Grenade,
    SpinningBlades,
    FireRateBoost,
    DamageBoost
}
public class Powerup : MonoBehaviour
{
    public PowerUpType powerUpType;
}
