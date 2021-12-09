using System;
using UnityEngine;

namespace Game
{
    public abstract class WeaponColorSetter : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color white = Color.white;
        [SerializeField] private Color yellow = Color.yellow;
        [SerializeField] private Color green = Color.green;
        [SerializeField] private Color blue = Color.blue;
        [SerializeField] private Color red = Color.red;
        [SerializeField] private Color orange = new Color(1f, 0.61f, 0f);
        [SerializeField] private Color none = Color.gray;

        protected Color ToColor(GravityState gravityState)
        {
            switch (gravityState)
            {
                case GravityState.White:
                    return white;
                case GravityState.Yellow:
                    return yellow;
                case GravityState.Blue:
                    return blue;
                case GravityState.Green:
                    return green;
                case GravityState.Red:
                    return red;
                case GravityState.Orange:
                    return orange;
                case GravityState.None:
                    return none;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravityState), gravityState, null);
            }
        }
    }
}