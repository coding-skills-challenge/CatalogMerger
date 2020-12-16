using Business.Services;

namespace CatalogMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvDataService = new CsvDataService();
            var barcodesPath_A = @"..\input\barcodesA.csv";
            var barcodes_A = csvDataService.ReadSupplierBarcode(barcodesPath_A);
            var barcodesPath_B = @"..\input\barcodesB.csv";
            var barcodes_B = csvDataService.ReadSupplierBarcode(barcodesPath_B);
            var catalogPath_A = @"..\input\catalogA.csv";
            var catalog_A = csvDataService.ReadCatalog(catalogPath_A);
            var catalogPath_B = @"..\input\catalogB.csv";
            var catalog_B = csvDataService.ReadCatalog(catalogPath_B);

            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            
            var outputPath = @"..\output\result_output.csv";
            csvDataService.WriteResultOutput(outputPath, result);
        }
    }
}
