using UnityEngine;
public enum PowerUpType
{
    GrenadePowerup,
    SpinningBlades,
    FireRateBoost,
    DamageBoost
}
public class Powerup : MonoBehaviour
{
    public PowerUpType powerUpType;
}
