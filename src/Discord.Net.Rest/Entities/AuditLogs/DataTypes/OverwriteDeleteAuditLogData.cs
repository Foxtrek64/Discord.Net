using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class OverwriteDeleteAuditLogData : IAuditLogData
    {
        private OverwriteDeleteAuditLogData(Overwrite deletedOverwrite)
        {
            Overwrite = deletedOverwrite;
        }

        internal static OverwriteDeleteAuditLogData Create(BaseDiscordClient discord, Model log, EntryModel entry)
        {
            var changes = entry.Changes;

            var denyModel = changes.FirstOrDefault(x => x.ChangedProperty == "deny");
            var typeModel = changes.FirstOrDefault(x => x.ChangedProperty == "type");
            var idModel = changes.FirstOrDefault(x => x.ChangedProperty == "id");
            var allowModel = changes.FirstOrDefault(x => x.ChangedProperty == "allow");

            var deny = denyModel.OldValue.ToObject<ulong>();
            var type = typeModel.OldValue.ToObject<string>();
            var id = idModel.OldValue.ToObject<ulong>();
            var allow = allowModel.OldValue.ToObject<ulong>();

            PermissionTarget target = type == "member" ? PermissionTarget.User : PermissionTarget.Role;

            return new OverwriteDeleteAuditLogData(new Overwrite(id, target, new OverwritePermissions(allow, deny)));
        }

        public Overwrite Overwrite { get; }
    }
}
