using Business.Models;
using CsvHelper.Configuration;

namespace Business.Services
{
    public sealed class SupplierBarcodeMap : ClassMap<SupplierBarcode>
    {
        public SupplierBarcodeMap()
        {
            Map(x => x.SupplierID).Name("SupplierID");
            Map(x => x.SKU).Name("SKU");
            Map(x => x.Barcode).Name("Barcode");
        }
    }
    public sealed class CatalogMap : ClassMap<Catalog>
    {
        public CatalogMap()
        {
            Map(x => x.SKU).Name("SKU");
            Map(x => x.Description).Name("Description");
        }
    }
}
