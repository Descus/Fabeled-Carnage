using Rewrite.Handlers;

namespace Rewrite.GameObjects.Actors.Enemies
{
    public class Kotlett : Animal
    {
        protected override void OnEnable()
        {
            EventHandler.SubscribeKillSteakEvent(KillSteak);
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            EventHandler.UnSubscribeKillSteakEvent(KillSteak);
            base.OnDisable();
        }

        public void KillSteak()
        {
            Kill(gameObject);
        }
    }
}
