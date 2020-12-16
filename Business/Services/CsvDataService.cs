using Business.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Business.Services
{
    public class CsvDataService
    {
        public List<SupplierBarcode> ReadSupplierBarcode(string location)
        {
            try
            {
                using var reader = new StreamReader(location, Encoding.Default);
                using var csv = new CsvReader(reader, Thread.CurrentThread.CurrentCulture);
                csv.Configuration.RegisterClassMap<SupplierBarcodeMap>();
                var records = csv.GetRecords<SupplierBarcode>().ToList();
                return records;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Catalog> ReadCatalog(string location)
        {
            try
            {
                using var reader = new StreamReader(location, Encoding.Default);
                using var csv = new CsvReader(reader, Thread.CurrentThread.CurrentCulture);
                csv.Configuration.RegisterClassMap<CatalogMap>();
                var records = csv.GetRecords<Catalog>().ToList();
                return records;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void WriteResultOutput(string path, List<ResultOutput> result)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true));
                using CsvWriter cw = new CsvWriter(sw, Thread.CurrentThread.CurrentCulture);
                cw.WriteHeader<ResultOutput>();
                cw.NextRecord();
                foreach (var e in result)
                {
                    cw.WriteRecord(e);
                    cw.NextRecord();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
