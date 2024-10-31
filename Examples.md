### Filtering and Extracting Data from Datagridview.Datasource / DataTable to a List
```C#
List<(int Id, string Status)> lstIdMalaDireta = (dgvMain.DataSource as DataTable).AsEnumerable()
    .Where(row => (bool)row["Flag"])
    .Select(row => (Id: (int)row["Id"], Status: row["Status"].ToString()))
    .ToList();
```
