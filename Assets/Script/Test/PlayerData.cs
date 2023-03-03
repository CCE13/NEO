using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [System.Serializable]
    public class PlayerData
    {
        public float speed;
        public float dashStrength;

        [HideInInspector]public Rigidbody2D rb;

    }
}
