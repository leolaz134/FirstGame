using UnityEngine;
public enum PowerUpType
{
    Multishot,
    SpinningBlades,
    FireRateBoost,
    DamageBoost
}
public class Powerup : MonoBehaviour
{
    public PowerUpType powerUpType;
}
