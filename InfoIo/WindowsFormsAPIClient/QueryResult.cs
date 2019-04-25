using System;
using System.Collections.Generic;
using System.Data;

namespace WindowsFormsAPIClient
{
    public class QueryResult
    {

        public List<ColumnInfo> Columns { get; set; }
        public DataTable Data { get; set; }

    }
}