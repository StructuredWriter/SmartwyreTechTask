﻿using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public decimal Price { get; set; }
    public string Uom { get; set; }
    public ESupportedIncentiveType SupportedIncentives { get; set; }
}
