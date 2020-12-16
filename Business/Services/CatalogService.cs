using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class CatalogService : ICatalogService
    {
        public List<ResultOutput> GetMergedCatalogList(List<SupplierBarcode> barcodes_A,
            List<SupplierBarcode> barcodes_B,
            List<Catalog> catalog_A,
            List<Catalog> catalog_B)
        {
            var unique_A = barcodes_A.Select(x => x.SKU).Distinct().ToList();

            var commonBarcodes = barcodes_A.Select(s1 => s1.Barcode)
                .Intersect(barcodes_B.Select(s2 => s2.Barcode)).ToList();

            var commonSupplierSKU_B = barcodes_B.Where(x => commonBarcodes.Contains(x.Barcode))
                .Select(x => new { x.SupplierID, x.SKU }).Distinct();

            var only_B = barcodes_B.Where(x => !commonSupplierSKU_B.Any(
                c => c.SupplierID == x.SupplierID && c.SKU == x.SKU))
                .Select(x => new { x.SKU }).Distinct().ToList();

            var result = catalog_A.Select(x =>
                new ResultOutput() { SKU = x.SKU, Description = x.Description, Source = "A" }).ToList();

            var only_B_Catalog = catalog_B.Where(x => only_B.Any(o => o.SKU == x.SKU)).Select(x =>
                  new ResultOutput() { SKU = x.SKU, Description = x.Description, Source = "B" }).ToList();

            result.AddRange(only_B_Catalog);

            return result;
        }
    }
}
