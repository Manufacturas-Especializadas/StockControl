﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StockControl.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int? RoleId { get; set; }

    public virtual Rol Role { get; set; }
}