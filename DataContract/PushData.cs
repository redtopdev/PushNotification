namespace Pushnotification.Contract
{
    public class PushData
    {
        public PushData(string type)
        {
            this.Type = type;
        }
        public string Type { get; private set; }
    }
}
