using CleanArcTest.Core.Base;

namespace CleanArcTest.Core.Events.Trademarks
{
    public class TrademarkNameChanged : DomainEvent
    {
        public TrademarkNameChanged(int trademarkId, string oldName, string newName)
        {
            TrademarkId = trademarkId;
            OldName = oldName;
            NewName = newName;
        }

        public int TrademarkId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
