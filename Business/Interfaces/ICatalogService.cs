using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    interface ICatalogService
    {
        List<ResultOutput> GetMergedCatalogList(List<SupplierBarcode> barcodes_A,
            List<SupplierBarcode> barcodes_B,
            List<Catalog> catalog_A,
            List<Catalog> catalog_B);
    }
}
