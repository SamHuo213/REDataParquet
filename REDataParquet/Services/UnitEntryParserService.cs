using REDataParquet.DataObjects;
using SalesParser.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SalesParser.Services {

    public class UnitEntryParserService {
        private const string UpdatedString = "updated";
        private const int EntryPropertyCount = 138;

        public UnitEntryParserService() {
        }

        public void ParseUnitEntires(
            IEnumerable<string> rawEntries,
            IDictionary<string, SoldUnit> soldUnits,
            IDictionary<string, Inventory> inventory,
            IDictionary<string, MonthlyInventory> monthlyInventory,
            int year,
            int month
        ) {
            foreach ( var entry in rawEntries ) {
                if ( string.IsNullOrEmpty(entry) ) {
                    continue;
                }

                var firstWord = entry.Substring(0, 7).ToLower();
                if ( firstWord == UpdatedString ) {
                    continue;
                }

                var entryArray = GetStringArray(entry);
                if ( entryArray.Length < EntryPropertyCount ) {
                    continue;
                }

                var isSoldEntry = IsSoldEntry(entryArray[33]);
                if ( isSoldEntry ) {
                    HandleSoldEntry(entryArray, soldUnits);
                } else if ( !string.IsNullOrEmpty(entryArray[106]) ) {
                    HandleInventoryEntry(entryArray, inventory);
                    HandleMonthlyInventoryEntry(entryArray, monthlyInventory, year, month);
                }
            }
        }

        private void HandleSoldEntry(string[] entryArray, IDictionary<string, SoldUnit> soldUnits) {
            var soldUnit = new SoldUnit() {
                MlsId = entryArray[0],
                Pid = int.Parse(entryArray[106]),
                Address = entryArray[5],
                Type = entryArray[9],
                SoldPrice = GetSoldPrice(entryArray[32]),
                FinalAskingPrice = Double.Parse(entryArray[7]) * 1000,
                OriginalAskingPrice = Double.Parse(entryArray[28]) * 1000,
                SoldDate = GetDate(entryArray[33]),
                ReportDate = GetDate(entryArray[34]),
                City = entryArray[134],
                Board = GetBoardFromRaw(entryArray[124])
            };

            if ( soldUnits.ContainsKey(soldUnit.Id) ) {
                soldUnits.Remove(soldUnit.Id);
            }

            soldUnits.Add(soldUnit.Id, soldUnit);
        }

        private void HandleMonthlyInventoryEntry(string[] entryArray, IDictionary<string, MonthlyInventory> monthlyInventory, int year, int month) {
            var inventory = new MonthlyInventory() {
                MlsId = entryArray[0],
                Pid = int.Parse(entryArray[106]),
                Address = entryArray[5],
                Type = entryArray[9],
                FinalAskingPrice = Double.Parse(entryArray[7]) * 1000,
                OriginalAskingPrice = Double.Parse(entryArray[28]) * 1000,
                City = entryArray[134],
                Board = GetBoardFromRaw(entryArray[124]),
                Year = year,
                Month = month
            };

            if ( monthlyInventory.ContainsKey(inventory.UniqueKey) ) {
                monthlyInventory.Remove(inventory.UniqueKey);
            }

            monthlyInventory.Add(inventory.UniqueKey, inventory);
        }

        private void HandleInventoryEntry(string[] entryArray, IDictionary<string, Inventory> inventory) {
            if ( inventory == null ) {
                return;
            }

            var newIntentory = new Inventory() {
                MlsId = entryArray[0],
                Pid = int.Parse(entryArray[106]),
                Address = entryArray[5],
                Type = entryArray[9],
                FinalAskingPrice = Double.Parse(entryArray[7]) * 1000,
                OriginalAskingPrice = Double.Parse(entryArray[28]) * 1000,
                City = entryArray[134],
                Board = GetBoardFromRaw(entryArray[124])
            };

            if ( inventory.ContainsKey(newIntentory.Id) ) {
                inventory.Remove(newIntentory.Id);
            }

            inventory.Add(newIntentory.Id, newIntentory);
        }

        private string[] GetStringArray(string rawEntry) {
            return rawEntry.Split("\t");
        }

        private bool IsSoldEntry(string soldPrice) {
            if ( string.IsNullOrEmpty(soldPrice) ) {
                return false;
            }

            return true;
        }

        private double GetSoldPrice(string soldPrice) {
            return Double.Parse(soldPrice) * 1000;
        }

        private Board GetBoardFromRaw(string boardString) {
            if ( boardString.ToLower() == "v" ) {
                return Board.GreaterVancouver;
            } else if ( boardString.ToLower() == "f" ) {
                return Board.FraserVally;
            } else if ( boardString.ToLower() == "h" ) {
                return Board.Chilliwack;
            }

            return Board.Unknown;
        }

        private DateTime GetDate(string dateTimeString) {
            return DateTime.Parse(
                dateTimeString,
                new CultureInfo("en-US")
            );
        }
    }
}