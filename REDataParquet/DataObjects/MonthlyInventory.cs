using SalesParser.Enums;

namespace REDataParquet.DataObjects {

    public class MonthlyInventory {

        public string Id {
            get {
                return $"{MlsId}_{Pid}";
            }
        }

        public string MlsId { get; set; }

        public int Pid { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public double FinalAskingPrice { get; set; }

        public double OriginalAskingPrice { get; set; }

        public string City { get; set; }

        public Board Board { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string UniqueKey {
            get {
                return $"{Id}_{Year}_{Month}";
            }
        }
    }
}