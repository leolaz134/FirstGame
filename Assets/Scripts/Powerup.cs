using UnityEngine;
public enum PowerUpType
{
    //Maybe add a speed powerup?
    GrenadePowerup,
    SpinningBlades,
    FireRateBoost,
    DamageBoost
}
public class Powerup : MonoBehaviour
{
    public PowerUpType powerUpType;
}
