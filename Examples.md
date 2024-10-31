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

### Using MERGE Statement for Data Insertion and Output

This example demonstrates how to use the `MERGE` statement in SQL to insert data into a target table while also capturing additional output data that may not be directly inserted. The `OUTPUT` clause allows retrieval of information from both the inserted rows and the original source rows.

```sql
MERGE INTO TargetTable AS Target
USING
(
    SELECT 15 AS ProcessId,
           195 AS StepId,
           'N' AS OpenNewLien,
           'N' AS MonthlyFollowUp,
           @LienId AS LienId,
           @CaseNumber AS CaseNumber,
           Source.NrDocument AS DocumentNumber,
           Source.EntryDate AS EntryDate,
           '1900-01-01' AS BlockLimitDate,
           Adjustment.DesiredBlockValue AS DesiredBlockValue,
           'T' AS BlockType,
           100 AS RevenuePercentage,
           0 AS AvailableBlockValue,
           0 AS RealizedBlockValue,
           @ProcessId AS ProcessId,
           Source.DocumentBatchId AS DocumentBatchId,
           Adjustment.TaxId,
           Adjustment.EstablishmentId, 
           Adjustment.EstablishmentName
    FROM @SourceAdjustments Adjustment
        JOIN @AuxiliaryData Source
            ON 1 = 1
) AS Source
ON 1 = 0 -- Forces the operation to always perform an INSERT
WHEN NOT MATCHED THEN
    INSERT
    (
        ProcessId,
        StepId,
        OpenNewLien,
        MonthlyFollowUp,
        LienId,
        CaseNumber,
        DocumentNumber,
        EntryDate,
        BlockLimitDate,
        DesiredBlockValue,
        BlockType,
        RevenuePercentage,
        AvailableBlockValue,
        RealizedBlockValue,
        ProcessId,
        DocumentBatchId
    )
    VALUES
    (Source.ProcessId,
     Source.StepId,
     Source.OpenNewLien,
     Source.MonthlyFollowUp,
     Source.LienId,
     Source.CaseNumber,
     Source.DocumentNumber,
     Source.EntryDate,
     Source.BlockLimitDate,
     Source.DesiredBlockValue,
     Source.BlockType,
     Source.RevenuePercentage,
     Source.AvailableBlockValue,
     Source.RealizedBlockValue,
     Source.ProcessId,
     Source.DocumentBatchId
    )
OUTPUT INSERTED.AdjustmentId,
       Source.TaxId,
       Source.EstablishmentId,
       Source.EstablishmentName
INTO @AuxiliaryAdjustments
(
    AdjustmentId,
    TaxId,
    EstablishmentId,
    EstablishmentName
);

-- Insert adjustment for the establishment lien
INSERT INTO AdjustmentEstablishment
(
    AdjustmentId,
    EstablishmentId,
    EstablishmentName,
    TaxId,
    TotalAgendaValue,
    AdjustmentValueRealized,
    AdjustmentSituationId,
    HoldSituationId
)
SELECT AUX.AdjustmentId AS AdjustmentId,
       AUX.EstablishmentId AS EstablishmentId,
       AUX.EstablishmentName AS EstablishmentName,
       AUX.TaxId AS TaxId,
       0 AS TotalAgendaValue,
       0 AS AdjustmentValueRealized,
       0 AS AdjustmentSituationId,
       3 AS HoldSituationId
FROM @AuxiliaryAdjustments AUX;
