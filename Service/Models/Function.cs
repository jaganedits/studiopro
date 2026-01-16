using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class Function
{
    public int Functionid { get; set; }

    public string Functionname { get; set; } = null!;

    public string? Functionurl { get; set; }

    public string? Menuicon { get; set; }

    public string? Rellink { get; set; }

    public int Parentid { get; set; }

    public bool Isexternal { get; set; }

    public int Screenorder { get; set; }

    public int Isapprovalneeded { get; set; }

    public int Status { get; set; }

    public int Createdby { get; set; }

    public DateTime Createdon { get; set; }

    public int Modifiedby { get; set; }

    public DateTime Modifiedon { get; set; }

    public int? Mobilescreen { get; set; }
}
