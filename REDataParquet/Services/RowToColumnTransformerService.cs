using Parquet.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace REDataParquet.Services {

    public class RowToColumnTransformerService {

        public static IEnumerable<DataColumn> RowToColumn(IEnumerable<PropertyInfo> properties, IEnumerable<object> values) {
            var columns = new List<DataColumn>();
            foreach ( var property in properties ) {
                DataField dataField;
                DataColumn column;

                if ( property.PropertyType == typeof(string) ) {
                    dataField = new DataField<string>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => property.GetValue(x)).Cast<string>().ToArray()
                    );
                } else if ( property.PropertyType == typeof(int) ) {
                    dataField = new DataField<int>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => property.GetValue(x)).Cast<int>().ToArray()
                    );
                } else if ( property.PropertyType == typeof(double) ) {
                    dataField = new DataField<double>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => property.GetValue(x)).Cast<double>().ToArray()
                    );
                } else if ( property.PropertyType == typeof(DateTime) ) {
                    dataField = new DataField<string>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => ((DateTime) property.GetValue(x)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Cast<string>().ToArray()
                    );
                } else if ( property.PropertyType.IsEnum ) {
                    dataField = new DataField<string>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => property.GetValue(x).ToString()).Cast<string>().ToArray()
                    );
                } else {
                    dataField = new DataField<string>(property.Name);
                    column = new DataColumn(
                       dataField,
                       values.Select(x => property.GetValue(x).ToString()).Cast<string>().ToArray()
                    );
                }

                columns.Add(column);
            }

            return columns;
        }
    }
}