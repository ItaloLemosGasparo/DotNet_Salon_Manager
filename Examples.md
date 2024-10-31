### Filtering and Extracting Data from Datagridview.Datasource / DataTable to a List
```C#
List<(int Id, string Status)> lstId = (dgvMain.DataSource as DataTable).AsEnumerable()
    .Where(row => (bool)row["Flag"])
    .Select(row => (Id: (int)row["Id"], Status: row["Status"].ToString()))
    .ToList();
```

### Filtering DataGridView Rows and Adding to DataTable

This example filters rows from a `DataGridView` where the `Flag` column is set to true, then adds the `Id` and `Status` values from each row to a `DataTable`.

```C#
DataTable dtIdMalaDireta = new DataTable();
dtId.Columns.Add("Id", typeof(int));
dtIdM.Columns.Add("Status", typeof(string));

dgvMain.Rows.Cast<DataGridViewRow>()
    .Where(r => Convert.ToBoolean(r.Cells["Flag"].Value))
    .ToList()
    .ForEach(row => dtId.Rows.Add(row.Cells["Id"].Value, row.Cells["Status"].Value));
```
