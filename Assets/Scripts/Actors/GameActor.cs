using Interfaces;
 using Structs;
 using UnityEngine;
 
 namespace Actors
 {
     public abstract class GameActor : MonoBehaviour, ISMovable
     {
         public Effect effect;
         
         public abstract void Move(float speed);
         
         public Effect GetEffect()
         {
             return effect;
         }
     }
 }