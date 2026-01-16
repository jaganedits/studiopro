using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class Numberingschema
{
    public int Numberschemaid { get; set; }

    public int Documentid { get; set; }

    public string? Prefix { get; set; }

    public bool? Isdateformat { get; set; }

    public int? Symbolid { get; set; }

    public string? Suffix { get; set; }

    public int Status { get; set; }

    public int Createdby { get; set; }

    public DateTime Createdon { get; set; }

    public int Modifiedby { get; set; }

    public DateTime Modifiedon { get; set; }

    public string? Sequencename { get; set; }

    public byte[] Rv { get; set; } = null!;
}
