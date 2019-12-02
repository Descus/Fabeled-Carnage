using System;
using Environment;
using Interfaces;
 using Structs;
 using UnityEngine;
 
 namespace Actors
 {
     public abstract class GameActor : MonoBehaviour, ISScrollable
     {
         public Effect effect;
         
         public abstract void Move(float speed);

         public ISScrollable OutOfScreen()
         {
             return transform.position.x <= -LaneManager.Spawnx ? this : null ;
         }

         public Effect GetEffect()
         {
             return effect;
         }

         private void OnEnable()
         {
             Scroller.onMoveUpdate += Move;
         }

         private void OnDisable()
         {
             Scroller.onMoveUpdate -= Move;
         }
     }
 }