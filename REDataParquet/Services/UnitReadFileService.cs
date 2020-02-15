using RealEstateDataParser.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace REDataParquet.Services {

    public class UnitReadFileService {
        private string fileDirectory;

        public string FileDirectory {
            set => fileDirectory = value;
            get {
                if ( string.IsNullOrEmpty(fileDirectory) ) {
                    fileDirectory = ConfigurationService.Configuration["dataFilePath"];
                }
                return fileDirectory;
            }
        }

        public IEnumerable<string> ReadAllLines(DateTime dateTime) {
            var DateString = dateTime.Date.ToString("yyyy-MM-dd");
            var DateFile = $"{DateString}.txt";
            var filePath = Path.Combine(FileDirectory, DateString, DateFile);
            return File.ReadAllLines(filePath);
        }

        public IEnumerable<string> ReadAllLines(string fileName) {
            var DateFile = $"{fileName}.txt";
            var filePath = Path.Combine(FileDirectory, fileName, DateFile);
            return File.ReadAllLines(filePath);
        }
    }
}