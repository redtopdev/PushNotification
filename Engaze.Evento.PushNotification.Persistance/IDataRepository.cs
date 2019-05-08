using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engaze.Evento.PushNotification.Persistance
{
    public interface IDataRepository
    {
        Task<IEnumerable<string>> GetAffectedUserIdList(Guid eventId);
    }
}
