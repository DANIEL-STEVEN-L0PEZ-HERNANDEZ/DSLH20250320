using System;
using System.Collections.Generic;

namespace DSLH20250320.AppWebMVC.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
