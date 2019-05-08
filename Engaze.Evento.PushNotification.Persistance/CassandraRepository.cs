using Engaze.Core.Persistance.Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engaze.Evento.PushNotification.Persistance
{
    public class CassandraRepository : IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;
        internal static string SELECTUSERIDSTATEMENT = "select userid from ez_event " +
            "WHEre id=? ALLOW FILTERING";

        private string keySpace;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager, string keySpace)
        {
            this.sessionCacheManager = sessionCacheManager;
            this.keySpace = keySpace;
        }

        public async Task<IEnumerable<string>> GetAffectedUserIdList(Guid eventId)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var result = await session.ExecuteAsync(session.Prepare(SELECTUSERIDSTATEMENT).Bind(eventId));
            return result.GetRows().Select(row => row.GetValue<Guid>("userid").ToString());
        }
    }
}
