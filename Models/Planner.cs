﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StockControl.Models;

public partial class Planner
{
    public int Id { get; set; }

    public int? ShopOrder { get; set; }

    public string Codigo { get; set; }

    public DateTime? Fecha { get; set; }
}