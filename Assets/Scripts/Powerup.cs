using UnityEngine;
public enum PowerUpType
{
    //Maybe add a speed powerup?
    //I also could've made all powerups in player controller couroutines instead of methods
    GrenadePowerup,
    SpinningBlades,
    FireRateBoost,
    DamageBoost
}
public class Powerup : MonoBehaviour
{
    public PowerUpType powerUpType;
}
