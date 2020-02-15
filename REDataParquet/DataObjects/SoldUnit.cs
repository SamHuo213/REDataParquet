using SalesParser.Enums;
using System;

namespace REDataParquet.DataObjects {

    public class SoldUnit {

        public string Id {
            get {
                return $"{MlsId}_{Pid}";
            }
        }

        public string MlsId { get; set; }

        public int Pid { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public double SoldPrice { get; set; }

        public double FinalAskingPrice { get; set; }

        public double OriginalAskingPrice { get; set; }

        public DateTime SoldDate { get; set; }

        public DateTime ReportDate { get; set; }

        public string City { get; set; }

        public Board Board { get; set; }
    }
}