using SalesParser.Enums;

namespace REDataParquet.DataObjects {

    public class Inventory {

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
    }
}