using System;
using System.Collections.Generic;

namespace GPNA.AlarmControlSystem
{
    public partial class Event
    {
        public string? AccessReason { get; set; }
        public string? Action { get; set; }
        public string? Actor { get; set; }
        public double? AlarmLimit { get; set; }
        public int? AlertAccess { get; set; }
        public int? AreaCode { get; set; }
        public string? AreaName { get; set; }
        public string? Author { get; set; }
        public string? Block { get; set; }
        public string? BlockTypeName { get; set; }
        public short? CardEncodingId { get; set; }
        public string? CardHolderFirstName { get; set; }
        public int? CardHolderId { get; set; }
        public string? CardHolderLastName { get; set; }
        public string? CardHolderType { get; set; }
        public string? CardNumber { get; set; }
        public int? Category { get; set; }
        public int? ChangedBias { get; set; }
        public long? ChangedTime { get; set; }
        public string? Classification { get; set; }
        public string? Comment { get; set; }
        public string? ConditionName { get; set; }
        public string? Criticality { get; set; }
        public int? CriticalityIndex { get; set; }
        public string? Description { get; set; }
        public string? Dsaserver { get; set; }
        public int? Eecode { get; set; }
        public long? EeinitId { get; set; }
        public string? EquipmentFullName { get; set; }
        public long EventId { get; set; }
        public decimal? ExecutionId { get; set; }
        public int? FieldBias { get; set; }
        public long? FieldTime { get; set; }
        public int? Flags { get; set; }
        public string? GdaconnectionName { get; set; }
        public int? Gdaquality { get; set; }
        public string? GdaserverName { get; set; }
        public string? Link1 { get; set; }
        public int? Link1Type { get; set; }
        public string? Link2 { get; set; }
        public int? Link2Type { get; set; }
        public string? Link3 { get; set; }
        public int? Link3Type { get; set; }
        public long LocalDate { get; set; }
        public long LocalTime { get; set; }
        public string? LocationFullName { get; set; }
        public string? LocationTagName { get; set; }
        public string? PointRoleLabel { get; set; }
        public string? PointRoleQualifier { get; set; }
        public string? PrevValue { get; set; }
        public int? PrevValueType { get; set; }
        public string? PrimaryEquipment { get; set; }
        public int? Priority { get; set; }
        public string? PublicName { get; set; }
        public string? Reason { get; set; }
        public int? ReceivedDelay { get; set; }
        public long? SequenceId { get; set; }
        public int? Severity { get; set; }
        public string? ShelvedReason { get; set; }
        public string? SignatureMeaning { get; set; }
        public string? Source { get; set; }
        public string? SourceEntityName { get; set; }
        public string? SourceParameter { get; set; }
        public string? SourceSort { get; set; }
        public string? Station { get; set; }
        public string? SubConditionName { get; set; }
        public string? SuppressionGroup { get; set; }
        public long Time { get; set; }
        public int? TimeBias { get; set; }
        public long TransactionId { get; set; }
        public string? TransitDirection { get; set; }
        public string? TransitType { get; set; }
        public string? Units { get; set; }
        public string? Value { get; set; }
        public int? ValueType { get; set; }
        public string? ZoneEntered { get; set; }
        public string? AlarmHelp { get; set; }
        public string? AuxUnit { get; set; }
        public string? EmailAddress { get; set; }
        public string? Length { get; set; }
        public string? Recipe { get; set; }
        public string? RollId { get; set; }
        public string? SectionId { get; set; }
        public int? TsReliable { get; set; }
    }
}
