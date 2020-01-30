namespace Interfaces
{
    public interface IScrollable
    {
        void Move(float speed);
        void SetSpeedDeviancyforLane(float deviancy, int lane);
    }
}