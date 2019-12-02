namespace Interfaces
{
    public interface ISScrollable
    {
            void Move(float speed);
            ISScrollable OutOfScreen();
    }
}
