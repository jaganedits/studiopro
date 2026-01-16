using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class SystemMetadatum
{
    public int MetadataId { get; set; }

    public string Category { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? NumericValue { get; set; }

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }

    public bool IsSystem { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
