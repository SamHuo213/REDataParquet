using System.Collections.Generic;

namespace REDataParquet.DataObjects {

    public class ParsedFileResult {
        public IDictionary<string, SoldUnit> SoldUnits { get; set; }

        public IDictionary<string, Inventory> Inventory { get; set; }

        public IDictionary<string, MonthlyInventory> MonthlyInventory { get; set; }
    }
}