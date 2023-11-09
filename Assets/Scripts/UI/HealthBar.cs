using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class HealthBar<T> where T : IEntity 
{
    public class HealthBarEnemy : HealthBar<EnemyBehavior>
    {

    }

    public class HealthBarUnit : HealthBar<PlayableUnit>
    {

    }
}


