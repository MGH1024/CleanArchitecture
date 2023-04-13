﻿namespace Domain.Base;

public class AuditableEntity
{
    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string DeletedBy { get; set; }

    public int Order { get; set; } = 10000;

    public bool? IsActive { get; set; }

    public bool? IsUpdated { get; set; }

    public bool? IsDeleted { get; set; }
}