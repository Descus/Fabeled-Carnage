﻿namespace Rewrite.Handlers
{
    public static class EventHandler
    {
        public delegate void MoveSubsriber(float speed);

        public delegate void SpeedDeviancySubcriber(float deviancy, int lane);

        public delegate void KillSteakSubscriber();

        public delegate void SwitchToSteak();

        public delegate bool PushSubscriber(int lane, float distance);
        
        private static event MoveSubsriber OnActorMoveUpdate;
        private static event MoveSubsriber OnBackgroundMoveUpdate;

        private static event SwitchToSteak OnFuryEnterEvent;
        private static event PushSubscriber PushEvent;
        private static event SpeedDeviancySubcriber DeviancySetEvent;

        private static event KillSteakSubscriber KillSteakEvent;

        public static void BroadcastActorMove(float speed) { if (OnActorMoveUpdate != null) OnActorMoveUpdate(speed); }

        public static void BroadcastBackgroundMove(float speed) { if (OnBackgroundMoveUpdate != null) OnBackgroundMoveUpdate(speed); }

        public static void BroadcastSteakKill() { if (KillSteakEvent != null) KillSteakEvent(); }
        
        public static void BroadcastFuryEvent(){ if(OnFuryEnterEvent != null) OnFuryEnterEvent();}

        public static void OnPushEvent(int lane, float distance) { if (PushEvent != null) PushEvent(lane, distance);}
        
        public static void OnDeviacySetEvent(float deviancy, int lane) { if (DeviancySetEvent != null) DeviancySetEvent(deviancy, lane); }

        public static void SubscribeSpeedDeviancyEvent(SpeedDeviancySubcriber add) { DeviancySetEvent += add; }
        
        public static void UnSubscribeSpeedDeviancyEvent(SpeedDeviancySubcriber sub) { DeviancySetEvent -= sub; }

        public static void SubscribeActorMoveEvent(MoveSubsriber add) { OnActorMoveUpdate += add; }

        public static void UnSubscribeActorMoveEvent(MoveSubsriber sub) { OnActorMoveUpdate -= sub; }

        public static void SubscribeBackgroundMoveEvent(MoveSubsriber add) { OnBackgroundMoveUpdate += add; }

        public static void UnSubscribeBackgroundMoveEvent(MoveSubsriber sub) { OnBackgroundMoveUpdate -= sub; }

        public static void SubscribePushEvent(PushSubscriber sub) { PushEvent += sub;}
        
        public static void UnSubscribePushEvent(PushSubscriber sub) { PushEvent -= sub;}
        
        public static void SubscribeKillSteakEvent(KillSteakSubscriber sub) { KillSteakEvent += sub;}
        
        public static void UnSubscribeKillSteakEvent(KillSteakSubscriber sub) { KillSteakEvent -= sub;}
        
        public static void SubscribeFuryEnterEvent(SwitchToSteak sub) { OnFuryEnterEvent += sub;}
        
        public static void UnSubscribeFuryEnterEvent(SwitchToSteak sub) { OnFuryEnterEvent -= sub;}
        
    }
}
