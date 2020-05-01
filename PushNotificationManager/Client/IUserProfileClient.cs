using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public interface IUserProfileClient
    {
        public Task<IEnumerable<string>> GetGCMClientIdsByUserIds(IEnumerable<Guid> userIds);

    }
}
