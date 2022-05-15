namespace CleanArcTest.Core.Base
{
    public abstract class DeletableEntity
    {
        public DateTime? Deleted { get; set; }
        public string? DeletedBy { get; set; }
    }
}
