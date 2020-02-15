using Parquet;
using Parquet.Data;
using RealEstateDataParser.Services;
using REDataParquet.DataObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace REDataParquet.Services {

    public class UnitWriteFileService {

        public UnitWriteFileService() {
        }

        public void WrtieData(
            IDictionary<string, SoldUnit> soldUnits,
            IDictionary<string, Inventory> inventory,
            IDictionary<string, MonthlyInventory> monthlyInventory
        ) {
            HandleSoldUnits(soldUnits);
            HandleInventoryUnits(inventory);
            HandleMonthlyInventoryUnits(monthlyInventory);
        }

        private void HandleSoldUnits(IDictionary<string, SoldUnit> soldUnits) {
            var outputFilePath = ConfigurationService.Configuration["outputFilePath"];
            var path = Path.Combine(outputFilePath, "ReSales.parquet");
            using Stream fileStream = File.Open(path, FileMode.OpenOrCreate);

            var properties = typeof(SoldUnit).GetProperties();
            var columns = RowToColumnTransformerService.RowToColumn(properties, soldUnits.Values);

            WriteColumns(columns, fileStream);
        }

        private void HandleInventoryUnits(IDictionary<string, Inventory> inventory) {
            var outputFilePath = ConfigurationService.Configuration["outputFilePath"];
            var path = Path.Combine(outputFilePath, "ReInventory.parquet");
            using Stream fileStream = File.Open(path, FileMode.OpenOrCreate);

            var properties = typeof(Inventory).GetProperties();
            var columns = RowToColumnTransformerService.RowToColumn(properties, inventory.Values);

            WriteColumns(columns, fileStream);
        }

        private void HandleMonthlyInventoryUnits(IDictionary<string, MonthlyInventory> monthlyInventory) {
            var outputFilePath = ConfigurationService.Configuration["outputFilePath"];
            var path = Path.Combine(outputFilePath, "ReMonthlyInventory.parquet");
            using Stream fileStream = File.Open(path, FileMode.OpenOrCreate);

            var properties = typeof(MonthlyInventory).GetProperties();
            var columns = RowToColumnTransformerService.RowToColumn(properties, monthlyInventory.Values);

            WriteColumns(columns, fileStream);
        }

        private void WriteColumns(IEnumerable<DataColumn> columns, Stream fileStream) {
            var schema = new Schema(columns.Select(x => x.Field).ToList());

            using var parquetWriter = new ParquetWriter(schema, fileStream);

            using ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup();
            foreach ( var column in columns ) {
                groupWriter.WriteColumn(column);
            }
        }
    }
}