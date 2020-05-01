using System.Collections.Generic;

namespace Notification.DataContract
{
    public class Reminder
    {
        public string EventName { get; set; }
        public string EventId { get; set; }
        public string RequestorId { get; set; }
        public string RequestorName { get; set; }
        public List<string> UserIdsForRemind { get; set; }
        //public List<PhoneContact> ContactNumbersForRemindFormatted { get; set; }
    }
}
