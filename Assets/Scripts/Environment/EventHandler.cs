namespace Environment
{
    public class EventHandler
    {
        public delegate void MoveSubsriber(float speed);

        public delegate void PushSubscriber(int lane, float distance);
        
        public static event MoveSubsriber OnActorMoveUpdate;
        public static event MoveSubsriber OnBackgroundMoveUpdate;

        public static event PushSubscriber PushEvent;

        public static void BroadcastActorMove(float speed)
        {
            if (OnActorMoveUpdate != null)
                OnActorMoveUpdate(speed);
        }

        public static void BroadcastBackgroundMove(float speed)
        {
            if (OnBackgroundMoveUpdate != null)
                OnBackgroundMoveUpdate(speed);
        }

        public static void OnPushEvent(int lane, float distance)
        {
            if (PushEvent != null)
                PushEvent(lane, distance);
        }

        public static void SubscribeActorMoveEvent(MoveSubsriber add)
        {
            OnActorMoveUpdate += add;
        }

        public static void UnSubscribeActorMoveEvent(MoveSubsriber sub)
        {
            OnActorMoveUpdate -= sub;
        }

        public static void SubscribeBackgroundMoveEvent(MoveSubsriber add)
        {
            OnBackgroundMoveUpdate += add;
        }

        public static void UnSubscribeBackgroundMoveEvent(MoveSubsriber sub)
        {
            OnBackgroundMoveUpdate -= sub;
        }

        public static void SubscribePushEvent(PushSubscriber sub)
        {
            PushEvent += sub;
        }
        
        public static void UnSubscribePushEvent(PushSubscriber sub)
        {
            PushEvent -= sub;
        }
    }
}