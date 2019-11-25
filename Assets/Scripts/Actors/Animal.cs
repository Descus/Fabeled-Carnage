using UnityEngine;

public abstract class Animal : MonoBehaviour, IsMovable
    {
        private float moveSpeedModifier;
        public abstract void Move(float speed);
    }
