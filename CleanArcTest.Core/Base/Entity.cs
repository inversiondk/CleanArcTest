namespace CleanArcTest.Core.Base
{
    public abstract class Entity // <-- Could be made into a generic class if needed (Entity<T>)
    {
        public int Id { get; private set; } 
    }
}
