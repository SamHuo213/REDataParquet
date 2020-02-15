using RealEstateDataParser.Services;
using REDataParquet.DataObjects;
using SalesParser.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace REDataParquet.Services {

    public class UnitBuilderService {
        private readonly UnitReadFileService unitFileService;
        private readonly UnitEntryParserService unitEntryParserService;

        public UnitBuilderService() {
            unitFileService = new UnitReadFileService();
            unitEntryParserService = new UnitEntryParserService();
        }

        public ParsedFileResult GetParsedDataResults() {
            unitFileService.FileDirectory = ConfigurationService.Configuration["rawDataFilePath"];
            var directories = Directory.GetDirectories(ConfigurationService.Configuration["rawDataFilePath"]);

            var soldUnits = new Dictionary<string, SoldUnit>();
            var inventory = new Dictionary<string, Inventory>();
            var inventoryByMonth = new Dictionary<string, MonthlyInventory>();

            DateTime latestDateTime = DateTime.MinValue;
            var directoryDateMap = new Dictionary<string, Tuple<int, int>>();
            foreach ( var directory in directories ) {
                var directoryName = new DirectoryInfo(directory).Name;
                var currentDateTime = DateTime.ParseExact(directoryName, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                directoryDateMap.Add(directoryName, new Tuple<int, int>(currentDateTime.Year, currentDateTime.Month));
                if ( currentDateTime > latestDateTime ) {
                    latestDateTime = currentDateTime;
                }
            }

            var latestDateTimeString = latestDateTime.ToString("yyyy-MM-dd");
            foreach ( var directory in directories ) {
                var directoryName = new DirectoryInfo(directory).Name;
                var allLines = unitFileService.ReadAllLines(directoryName);

                var yearMonth = directoryDateMap.GetValueOrDefault(directoryName);
                if ( latestDateTimeString == directoryName ) {
                    unitEntryParserService.ParseUnitEntires(allLines, soldUnits, inventory, inventoryByMonth, yearMonth.Item1, yearMonth.Item2);
                } else {
                    unitEntryParserService.ParseUnitEntires(allLines, soldUnits, null, inventoryByMonth, yearMonth.Item1, yearMonth.Item2);
                }
            }

            return new ParsedFileResult() {
                SoldUnits = soldUnits,
                Inventory = inventory,
                MonthlyInventory = inventoryByMonth
            };
        }
    }
}