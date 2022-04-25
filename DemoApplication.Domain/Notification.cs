using DemoApplication.Enums;
using Abstractions.Domain;
using System;

namespace DemoApplication.Domain
{
    public class Notification : Entity<Guid>
    {
        #region Constructor
        public Notification
        (
            Guid originator,
            SeverityEnum severity,
            string description,
            DateTimeOffset date,
            bool read,
            Guid seenBy,
            DateTimeOffset? seenAt,
            string relatedId,
            EntityEnum entity,
            RolesEnum forRole
        )
        {
            Originator = originator;
            Severity = severity;
            Description = description;
            Date = date;
            Read = read;
            SeenBy = seenBy;
            SeenAt = seenAt;
            RelatedId = relatedId;
            ForRole = forRole;
            Entity = entity;
        }
        public Notification(Guid id) : base(id) { }
        #endregion

        #region Private Members
        public Guid Originator { get; private set; }
        public SeverityEnum Severity { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool Read { get; private set; }
        public Guid SeenBy { get; private set; }
        public DateTimeOffset? SeenAt { get; private set; }
        public RolesEnum ForRole { get; private set; }
        public string RelatedId { get; private set; }
        public EntityEnum Entity { get; private set; }
        #endregion

        #region Interactions
        public void ReadNotification(Guid user)
        {
            Read = true;
            SeenBy = user;
            SeenAt = DateTimeOffset.UtcNow;
        }
        #endregion
    }
}
