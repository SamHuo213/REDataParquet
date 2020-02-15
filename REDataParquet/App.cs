using REDataParquet.Services;
using System;

namespace REDataParquet {

    public class App {
        private readonly UnitBuilderService unitBuilderService;
        private readonly UnitWriteFileService unitWriteFileService;

        public App() {
            unitBuilderService = new UnitBuilderService();
            unitWriteFileService = new UnitWriteFileService();
        }

        public void Run() {
            try {
                RunInternal();
            } catch ( Exception e ) {
                Console.WriteLine($"Failed message: {e.Message}");
            }
        }

        public void RunInternal() {
            var totalParsedDataResults = unitBuilderService.GetParsedDataResults();

            unitWriteFileService.WrtieData(
                totalParsedDataResults.SoldUnits,
                totalParsedDataResults.Inventory,
                totalParsedDataResults.MonthlyInventory
            );
        }
    }
}