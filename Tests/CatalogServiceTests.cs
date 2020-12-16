using Business.Models;
using Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class CatalogServiceTests
    {
        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_SameSKU_DifferentProducts()
        {
            //Product codes might be same, but they are different products.
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "1111", Description = "product XYZ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID="1", SKU= "1111", Barcode="BAR-CODE-ABC" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-XYZ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.FirstOrDefault().SKU, result.LastOrDefault().SKU);
            Assert.AreEqual(result.FirstOrDefault().Source, "A");
            Assert.AreEqual(result.LastOrDefault().Source, "B");
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_DifferentSKU_SameProduct()
        {
            //Product codes are different, but they are same product.
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.FirstOrDefault().Source, "A");
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_No_Duplicated_Products()
        {
            //You should not be duplicating product records in merged catalog
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            catalog_A.Add(new Catalog() { SKU = "1111-1", Description = "product DEF" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            catalog_B.Add(new Catalog() { SKU = "2222-2", Description = "product OPQ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-DEF" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222-2", Barcode = "BAR-CODE-OPQ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0].SKU, "1111");
            Assert.AreEqual(result[1].SKU, "1111-1");
            Assert.AreEqual(result[2].SKU, "2222-2");
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_Has_Company_Information()
        {
            //Product on merged catalog must have information about the company it belongs to originally.
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            catalog_A.Add(new Catalog() { SKU = "1111-1", Description = "product DEF" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            catalog_B.Add(new Catalog() { SKU = "2222-2", Description = "product OPQ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-DEF" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222-2", Barcode = "BAR-CODE-OPQ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result[0].Source, "A");
            Assert.AreEqual(result[1].Source, "A");
            Assert.AreEqual(result[2].Source, "B");
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_When_Supplier_Have_1_Barcode()
        {
            //supplier provides 1 barcode for a product
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            catalog_A.Add(new Catalog() { SKU = "1111-1", Description = "product DEF" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            catalog_B.Add(new Catalog() { SKU = "2222-2", Description = "product OPQ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "2", SKU = "1111", Barcode = "BAR-CODE-DEF" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "2", SKU = "2222-2", Barcode = "BAR-CODE-OPQ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_When_Supplier_Have_Many_Barcodes()
        {
            //supplier provides many barcode for a product
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            catalog_A.Add(new Catalog() { SKU = "1111-1", Description = "product DEF" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            catalog_B.Add(new Catalog() { SKU = "2222-2", Description = "product OPQ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-2" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-3" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-4" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-5" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-6" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-7" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-8" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "BAR-CODE-9" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "2", SKU = "1111", Barcode = "BAR-CODE-DEF" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "2", SKU = "2222-2", Barcode = "BAR-CODE-OPQ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod]
        public void CatalogServiceTests_GetMergedCatalogList_When_Have_Many_Suppliers()
        {
            //A product may have many suppliers,
            var catalog_A = new List<Catalog>();
            catalog_A.Add(new Catalog() { SKU = "1111", Description = "product ABC" });
            catalog_A.Add(new Catalog() { SKU = "1111-1", Description = "product DEF" });
            var catalog_B = new List<Catalog>();
            catalog_B.Add(new Catalog() { SKU = "2222", Description = "product XYZ" });
            catalog_B.Add(new Catalog() { SKU = "2222-2", Description = "product OPQ" });
            var barcodes_A = new List<SupplierBarcode>();
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "1", SKU = "1111", Barcode = "SAME-BAR-CODE" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "2", SKU = "1111", Barcode = "BAR-CODE-2" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "3", SKU = "1111", Barcode = "BAR-CODE-3" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "4", SKU = "1111", Barcode = "BAR-CODE-4" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "5", SKU = "1111", Barcode = "BAR-CODE-5" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "6", SKU = "1111", Barcode = "BAR-CODE-6" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "7", SKU = "1111", Barcode = "BAR-CODE-7" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "8", SKU = "1111", Barcode = "BAR-CODE-8" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "9", SKU = "1111", Barcode = "BAR-CODE-9" });
            barcodes_A.Add(new SupplierBarcode() { SupplierID = "10", SKU = "1111", Barcode = "BAR-CODE-DEF" });
            var barcodes_B = new List<SupplierBarcode>();
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "1", SKU = "2222", Barcode = "SAME-BAR-CODE" });
            barcodes_B.Add(new SupplierBarcode() { SupplierID = "2", SKU = "2222-2", Barcode = "BAR-CODE-OPQ" });
            var catalogService = new CatalogService();
            var result = catalogService.GetMergedCatalogList(barcodes_A, barcodes_B, catalog_A, catalog_B);
            Assert.AreEqual(result.Count(), 3);
        }
    }
}
