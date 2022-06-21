Imports System.Data.SqlClient
'Imports System.Data.OleDb
'Imports System.IO


Public Class frmEditMap
    Protected Friend gdtMap As DataTable
    Protected Friend FormLoad As Boolean
    Dim ldaMap As SqlDataAdapter
    Dim ldsMap = New DataSet
    Private SQLCtl As New SQLControl
    Private Sub EditMap_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLoad = True
        SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'")
        With cboMap
            .DataSource = SQLCtl.sqlds.Tables(0)
            .DisplayMember = "MapDesc"
            .ValueMember = "MapCode"

        End With


        'Dim lServer As String = "SDN-ENVY-2020\SDN_HPENVY"
        'Dim lDatabase As String = "TestDB"
        'Dim lSqlConnection As New SqlClient.SqlConnection
        'Dim lConnectionString As String
        'FormLoad = True

        'lConnectionString = "Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;"
        'lSqlConnection.ConnectionString = lConnectionString
        'lSqlConnection.Open()
        'Dim lCmdText As String = "Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'"

        'Dim ldaMaps As New SqlDataAdapter(lCmdText, lSqlConnection)
        'Dim ldtMaps As New DataTable
        'ldaMaps.Fill(ldtMaps)

        'With cboMap
        '    .DataSource = ldtMaps
        '    .DisplayMember = "MapDesc"
        '    .ValueMember = "MapCode"

        'End With
        FormLoad = False
    End Sub
    Private Sub cboMap_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedIndexChanged
        ' cboMap has all defined mappings.  When selected, load the details for that mapping to the gdtMap Data table
        If Not FormLoad Then
            Dim lServer As String = "SDN-ENVY-2020\SDN_HPENVY"
            Dim lDatabase As String = "TestDB"
            Dim lSqlConnection As New SqlClient.SqlConnection
            Dim lConnectionString As String
            lConnectionString = "Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;"
            lSqlConnection.ConnectionString = lConnectionString
            lSqlConnection.Open()
            Dim lCmdText As String = ""

            lCmdText = "Select RowUno, Mapcode, ApplicationType, TargetColumn, SourceColumnLabel, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;"
            '"Select MapCode, MapDesc, FileMask, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
            '"Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
            ldaMap = New SqlDataAdapter(lCmdText, lSqlConnection)
            ldaMap.Fill(ldsMap)

            gdtMap = ldsMap.Tables(0)
            Dim DataTypeCol As New DataGridViewComboBoxColumn
            DataTypeCol.Items.Add("K")
            DataTypeCol.Items.Add("L")
            DataTypeCol.Items.Add("R")
            DataTypeCol.Items.Add(" ")
            Dim TextCol0, TextCol1, TextCol2, TextCol3, TextCol4 As New DataGridViewTextBoxColumn

            With dbMap
                .AutoGenerateColumns = False
                .Columns.Add(TextCol0)
                .Columns.Add(TextCol1)
                .Columns.Add(TextCol2)
                .Columns.Add(TextCol3)
                .Columns.Add(TextCol4)
                .Columns.Add(DataTypeCol)
                .Columns(0).DataPropertyName = "RowUno"
                .Columns(1).DataPropertyName = "MapCode"
                .Columns(2).DataPropertyName = "ApplicationType"
                .Columns(3).DataPropertyName = "TargetColumn"
                .Columns(4).DataPropertyName = "SourceColumnLabel"
                .Columns(5).DataPropertyName = "DataType"

                .Columns(0).ReadOnly = True
                .Columns(3).ReadOnly = True
                .Columns(0).DefaultCellStyle.BackColor = Color.Silver
                .Columns(3).DefaultCellStyle.BackColor = Color.Silver
                .Columns(0).Visible = False

                .DataSource = gdtMap


            End With

            ldaMap.UpdateCommand = New SqlCommandBuilder(ldaMap).GetUpdateCommand

            'dbMapz.DataSource = ldsMap.Tables(1)
            'ReceiptId = ldsMap.Tables(1).Rows(0)("ReceiptID") 'Get identifier for receipt object
            'OpenFileDialog1.Filter = "CR IMport Files|" & ldsMap.Tables(1).Rows(0)("FileMask") 'Get identifier for receipt object

            'If ldsMap.Tables(2).Rows.Count > 0 Then
            '    gImportNum = ldsMap.Tables(2).Rows(0)(0)
            'End If

            'btnOpen.Enabled = True
            'txtCRFile.Enabled = True

        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ldaMap.Update(ldsMap)
    End Sub
End Class