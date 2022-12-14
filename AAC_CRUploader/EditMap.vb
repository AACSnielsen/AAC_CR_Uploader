Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Xml
'Imports System.Data.OleDb
'Imports System.IO


Public Class frmEditMap
    Protected Friend gdtMap As DataTable
    Protected Friend gdtTargetColumns As DataTable
    Protected Friend gdtAvailableColumns As DataTable
    Protected Friend FormLoad As Boolean
    Dim ldaMap As SqlDataAdapter
    Dim ldsMap = New DataSet
    Private SQLCtl As New SQLControl
    Protected Friend gSQLConnection As SqlConnection
    Private Sub EditMap_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLoad = True
        Dim lServer As String = ""
        Dim lDatabase As String = ""
        Dim AppPath As String = System.AppDomain.CurrentDomain.BaseDirectory()
        If File.Exists(AppPath & "Instance.config") Then 'Get connection info
            Dim ConfigXML As New XmlDocument()
            ConfigXML.Load(AppPath & "Instance.config")
            Dim RepositoryNode As XmlNode = ConfigXML.GetElementsByTagName("repository")(0)
            lServer = (RepositoryNode.Attributes("server").Value)
            lDatabase = (RepositoryNode.Attributes("name").Value)
        End If
        If lServer = "" Then ' Error - no config found
            MsgBox("No connection info found in Instance.config or file is missing." & vbCrLf & "(" + AppPath & ")" & vbCrLf & "<instanceMetadataConfigurationSection><repository name={database} server={server}...>", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "No valid connection information")
            End
        End If
        gSQLConnection = New SqlClient.SqlConnection
        Dim lConnectionString As String
        lConnectionString = "Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;"
        gSQLConnection.ConnectionString = lConnectionString
        gSQLConnection.Open()


        SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
        With cboMap
            .DataSource = SQLCtl.sqlds.Tables(0)
            .DisplayMember = "MapDesc"
            .ValueMember = "MapCode"
            .Text = UploadCRFile.CurrSelectedMap
        End With

        btnSave.Enabled = False

        FormLoad = False
        cboMap_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub cboMap_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedIndexChanged
        ' cboMap has all defined mappings.  When selected, load the details for that mapping to the gdtMap Data table
        If Not FormLoad Then


            ' Dim lSqlConnection As New SqlClient.SqlConnection

            Dim lCmdText As String = ""
            lCmdText = "Select 	MapCode,MapDesc,FileMask,FileType,Inactive,ReceiptID,TargetTable,XLSheetName from _aac_CRMAPZ where mapcode = '" & cboMap.SelectedValue & "'"
            SQLCtl.ExecQuery(lCmdText, gSQLConnection)
            txtReceiptID.Text = SQLCtl.sqlds.Tables(0).Rows(0)("ReceiptID").ToString
            txtFileMask.Text = SQLCtl.sqlds.Tables(0).Rows(0)("FileMask").ToString
            txtTargetTable.Text = SQLCtl.sqlds.Tables(0).Rows(0)("TargetTable").ToString
            CBOSheetName.Text = SQLCtl.sqlds.Tables(0).Rows(0)("XLSheetName").ToString
            cboFileType.Text = SQLCtl.sqlds.Tables(0).Rows(0)("FileType").ToString

            lCmdText = "Select RowUno, Mapcode, ApplicationType, TargetColumn, SourceColumnLabel, DataType from _aac_CRMAP where isnull(applicationtype,'') <> '' and mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;"
            '"Select MapCode, MapDesc, FileMask, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
            '"Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
            ldaMap = New SqlDataAdapter(lCmdText, gSQLConnection)
            ldsMap.clear

            ldaMap.Fill(ldsMap)
            ' If source file was identified on previous screen, load columns into AvailableFields list
            ' gdsSelectedCRSourceFile is a DataSet object for the selected file
            With lbAvailableFields
                gdtAvailableColumns = UploadCRFile.gdsSelectedCRSourceFile.Tables(0)
                .Items.Clear()
                If UploadCRFile.txtCRFile.Text <> "" Then
                    For Each col In UploadCRFile.gdsSelectedCRSourceFile.Tables(0).Columns
                        .Items.Add(col.ToString)
                    Next
                Else
                    .Items.Add("{Select file on previous screen}")
                End If
                .Items.Add("%User%")
                .Items.Add("%FileName%")
                .Items.Add("%ImportNum%")
                .Items.Add("%TranNum%")
                .Items.Add("%TranLine%")
                .Items.Add("%Time%")
            End With



            gdtMap = ldsMap.Tables(0)
            Dim DataTypeCol As New DataGridViewComboBoxColumn
            DataTypeCol.Items.Add("K")
            DataTypeCol.Items.Add("L")
            DataTypeCol.Items.Add("R")
            DataTypeCol.Items.Add(" ")
            Dim TextCol0, TextCol1, TextCol2, TextCol3, TextCol4 As New DataGridViewTextBoxColumn

            With dbMap
                '.DataSource = Nothing
                '.Rows.Clear()
                .AutoGenerateColumns = False
                .Columns.Add(TextCol0)
                .Columns.Add(TextCol1)
                .Columns.Add(TextCol2)
                .Columns.Add(TextCol3)
                'If UploadCRFile.txtCRFile.Text <> "" Then
                '    .Columns.Add(SourceDataColLookup)
                'Else
                .Columns.Add(TextCol4)
                'End If

                .Columns.Add(DataTypeCol)
                .Columns(0).DataPropertyName = "RowUno"
                .Columns(1).DataPropertyName = "MapCode"
                .Columns(2).DataPropertyName = "ApplicationType"
                .Columns(3).DataPropertyName = "TargetColumn"
                .Columns(4).DataPropertyName = "SourceColumnLabel"
                .Columns(5).DataPropertyName = "DataType"
                .Columns(1).HeaderText = "Map Code"
                .Columns(2).HeaderText = "Application Type"
                .Columns(3).HeaderText = "Target Column"
                .Columns(4).HeaderText = "Source"
                .Columns(5).HeaderText = "'L'abel" + vbCrLf + "'K'onstant" + vbCrLf + "'R'eplacement"
                .Columns(0).ReadOnly = True
                .Columns(3).ReadOnly = True
                .Columns(0).DefaultCellStyle.BackColor = Color.Silver
                .Columns(3).DefaultCellStyle.BackColor = Color.Silver
                .Columns(0).Visible = False
                .Columns(1).Visible = False

                .DataSource = gdtMap
                For R As Integer = 0 To .RowCount - 1
                    If .Rows(R).Cells(2).Value = "R" Then
                        .Rows(R).DefaultCellStyle.BackColor = Color.Honeydew
                    Else
                        .Rows(R).DefaultCellStyle.BackColor = Color.LightCyan
                    End If

                Next

            End With

            ldaMap.UpdateCommand = New SqlCommandBuilder(ldaMap).GetUpdateCommand
            btnSave.Enabled = False
            txtTargetTable_Leave(sender, e)

        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'Dim myCommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(ldaMap)
        'Dim lrow As DataGridViewRow
        'For Each lrow In dbMap.Rows
        '    If String.IsNullOrEmpty(lrow.Cells(1).Value) Then
        '        lrow.Cells(1).Value = cboMap.Text
        '    End If
        'Next
        ldaMap.Update(ldsMap) ' THis sill update the map from the data bound grid
        Dim lCmd As String = "Update _aac_CRMAPZ set receiptid = '" & txtReceiptID.Text & "', FileMask = '" & txtFileMask.Text &
                            "', TargetTable = '" & txtTargetTable.Text &
                            "', FileType = '" & cboFileType.Text &
                            "', XLSheetName = '" & CBOSheetName.Text &
                            "' where mapcode = '" &
            cboMap.SelectedValue & "'"
        SQLCtl.ExecCmd(lCmd, gSQLConnection)
        btnSave.Enabled = False
        cboMap_SelectedIndexChanged(sender, e)
        UploadCRFile.CurrSelectedMap = cboMap.SelectedText
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If btnSave.Enabled Then
            If MsgBox("Discard Pending Changes?", MsgBoxStyle.OkCancel, "Unsaved Changes") = MsgBoxResult.Ok Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub


    Private Sub dbMap_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dbMap.CellValueChanged
        btnSave.Enabled = True
    End Sub

    Private Sub lbAvailableFields_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbAvailableFields.SelectedIndexChanged
        If lbTargetColumns.SelectedItems.Count = 1 And lbAvailableFields.SelectedItems.Count = 1 Then
            btnAddPair.Enabled = True
        Else
            btnAddPair.Enabled = False
        End If
    End Sub

    Private Sub lbAvailableFields_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lbAvailableFields.MouseDoubleClick
        Debug.Print(lbAvailableFields.SelectedItem)
        If dbMap.SelectedCells.Count = 1 AndAlso dbMap.SelectedCells(0).ColumnIndex = 4 Then
            dbMap.SelectedCells(0).Value = lbAvailableFields.SelectedItem


        End If
    End Sub

    Private Sub BtnAddMapping_Click(sender As Object, e As EventArgs)
        Dim NewRow As DataRow
        NewRow = gdtMap.NewRow
        NewRow("MapCode") = "R"
        gdtMap.Rows.Add(NewRow)
    End Sub



    Private Sub dbMap_DefaultValuesNeeded(sender As Object, e As DataGridViewRowEventArgs) Handles dbMap.DefaultValuesNeeded
        e.Row.Cells(1).Value = Me.cboMap.SelectedValue
        e.Row.Cells(5).Value = ""
        e.Row.Cells(3).ReadOnly = False

    End Sub


    Private Sub txtTargetTable_Leave(sender As Object, e As EventArgs) Handles txtTargetTable.Leave
        lbTargetColumns.Items.Clear()
        Dim lCmdText As String = ""
        lCmdText = "select c.name from sys.columns c join sys.objects o on c.object_id = o.object_id  where o.name = '" + txtTargetTable.Text + "'"
        SQLCtl.ExecQuery(lCmdText, gSQLConnection)
        If txtTargetTable.Text = "" Or SQLCtl.sqlds.Tables(0).Rows.Count = 0 Then
            lbTargetColumns.Items.Add("{Enter Valid Target}")
        Else
            gdtTargetColumns = SQLCtl.sqlds.Tables(0)
            For lRow = 0 To SQLCtl.sqlds.Tables(0).Rows.Count - 1
                lbTargetColumns.Items.Add(SQLCtl.sqlds.Tables(0).Rows(lRow)(0))
            Next
        End If
    End Sub

    Private Sub lbTargetColumns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbTargetColumns.SelectedIndexChanged
        If lbTargetColumns.SelectedItems.Count = 1 And lbAvailableFields.SelectedItems.Count = 1 Then
            btnAddPair.Enabled = True
        Else
            btnAddPair.Enabled = False
        End If
    End Sub
    Private Sub lbTargetColumns_DoubleClick(sender As Object, e As EventArgs) Handles lbTargetColumns.DoubleClick
        Debug.Print(lbAvailableFields.SelectedItem)
        If dbMap.SelectedCells.Count = 1 AndAlso dbMap.SelectedCells(0).ColumnIndex = 3 Then
            dbMap.SelectedCells(0).Value = lbTargetColumns.SelectedItem


        End If
    End Sub

    Private Sub btnAddPair_Click(sender As Object, e As EventArgs) Handles btnAddPair.Click
        Dim NewRow As DataRow
        NewRow = gdtMap.NewRow
        NewRow("ApplicationType") = "?"
        NewRow("MapCode") = cboMap.SelectedValue
        NewRow("SourceColumnLabel") = lbAvailableFields.SelectedItem
        NewRow("TargetColumn") = lbTargetColumns.SelectedItem
        NewRow("DataType") = "L"
        gdtMap.Rows.Add(NewRow)

        btnSave.Enabled = True

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If txtNewCode.Text <> "" And txtNewDesc.Text <> "" Then
            Dim lcmd As String = ""
            lcmd = "insert into _AAC_CRMAP (MapCode, ApplicationType, StageTable, SourceColumnLabel, TargetColumn, DataType) " &
                " select '" & txtNewCode.Text & "',	ApplicationType	,StageTable	,SourceColumnLabel	,TargetColumn	,DataType from _AAC_CRMAP where mapcode = '" & cboMap.SelectedValue & "'" &
                " insert into _AAC_CRMAPZ (MapCode, MapDesc, FileMask, FileType, Inactive, ReceiptID, TargetTable, XLSheetName) " &
                " select '" & txtNewCode.Text & "','" & txtNewDesc.Text & "' ,FileMask	,FileType	,Inactive	,ReceiptID	,TargetTable, XLSheetName from _AAC_CRMAPZ where mapcode = '" & cboMap.SelectedValue & "'"
            SQLCtl.ExecCmd(lcmd, gSQLConnection)

            SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, FileType, ReceiptID, XLSheetName from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
            FormLoad = True
            With cboMap
                .DataSource = SQLCtl.sqlds.Tables(0)
                .DisplayMember = "MapDesc"
                .ValueMember = "MapCode"
            End With
            FormLoad = False
        End If
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label8_Click_1(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim mans As MsgBoxResult = MsgBox("This will permanently delete the mapping " & cboMap.SelectedValue & vbCrLf & "Are you sure?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, "Warning!")
        If mans = MsgBoxResult.Yes Then
            Dim lcmd As String = "Delete from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "';" &
                                 "Delete from _aac_crmapZ where mapcode = '" & cboMap.SelectedValue & "';"
            SQLCtl.ExecCmd(lcmd, gSQLConnection)
            FormLoad = True
            SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
            With cboMap
                .DataSource = SQLCtl.sqlds.Tables(0)
                .DisplayMember = "MapDesc"
                .ValueMember = "MapCode"
            End With
            FormLoad = False
        End If
    End Sub

    Private Sub txtFileMask_TextChanged(sender As Object, e As EventArgs) Handles txtFileMask.TextChanged
        btnSave.Enabled = True
    End Sub

    Private Sub cboFileType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFileType.SelectedIndexChanged
        btnSave.Enabled = True
    End Sub

    Private Sub CBOSheetName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CBOSheetName.SelectedIndexChanged
        btnSave.Enabled = True
    End Sub

    Private Sub txtReceiptID_TextChanged(sender As Object, e As EventArgs) Handles txtReceiptID.TextChanged
        btnSave.Enabled = True
    End Sub

    Private Sub bntValidate_Click(sender As Object, e As EventArgs) Handles bntValidate.Click
        ' Check mapping against sample data file
        ' gdtmap is the mapping data table displayed in the mapping grid
        '        columns of n ote: TargetColumn, SourceColumnLabel, DataType
        ' gdtTargetColumns is the columns in the target SQL table
        ' gdtAvailableColumns is the columns in the sample file
        ' Additional available columns:
        '            %User%, %FileName%, %ImportNum%, %TranNum%, %TranLine%, %Time%

        'Verify all SourceColumnLabel values in gdtmap exist in gdtavailablecolumns where datatype = 'L' or 'K'
        Dim MapIssuesText As String = ""
        If btnSave.Enabled Then
            Dim msgresp As MsgBoxResult
            msgresp = MsgBox("Must save changes to validate.  Do you want to save now?", MsgBoxStyle.YesNoCancel, "Pending changes")
            If msgresp = MsgBoxResult.Yes Then
                btnSave_Click(sender, e)
            End If
            If msgresp = MsgBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim ColumnFound As Boolean = False

        Dim drMapSource() As DataRow = gdtMap.Select("DataType = 'R' or DataType = 'L'")
        For Each row As DataRow In drMapSource
            ColumnFound = False
            Debug.Print(row("SourceColumnLabel"))
            For Each col In UploadCRFile.gdsSelectedCRSourceFile.Tables(0).Columns
                Debug.Print(col.ToString & ":" & row("SourceColumnLabel"))
                If col.ToString.ToUpper = row("SourceColumnLabel").toupper Then
                    ColumnFound = True
                    Exit For
                End If
                Select Case row("SourceColumnLabel").toupper
                    Case "%User%".ToUpper
                        ColumnFound = True
                    Case "%FileName%".ToUpper
                        ColumnFound = True
                    Case "%ImportNum%".ToUpper
                        ColumnFound = True
                    Case "%TranNum%".ToUpper
                        ColumnFound = True
                    Case "%TranLine%".ToUpper
                        ColumnFound = True
                    Case "%Time%".ToUpper
                        ColumnFound = True
                End Select

            Next
            If Not ColumnFound Then
                MapIssuesText &= "Source column:" + row("SourceColumnLabel").ToString + " not found in file" + Environment.NewLine
            End If
        Next

        Dim drMapTarget() As DataRow = gdtMap.Select("TargetColumn <> ''")
        For Each row As DataRow In drMapTarget
            ColumnFound = False
            Debug.Print(row("TargetColumn"))
            For Each Targetrow In gdtTargetColumns.Rows
                Debug.Print(Targetrow("Name") & ":" & row("TargetColumn"))
                If Targetrow("Name").toupper = row("TargetColumn").toupper Then
                    ColumnFound = True
                    Exit For
                End If
            Next
            If Not ColumnFound Then
                MapIssuesText &= "Target column:" + row("TargetColumn").ToString + " not found SQL Table" + Environment.NewLine
            End If
        Next

        If MapIssuesText = "" Then
            MapIssuesText = "No Issues Identified"
        End If
        MsgBox(MapIssuesText, vbOKOnly, "Mapping Issues")
    End Sub

    Private Sub btnExportmap_Click(sender As Object, e As EventArgs) Handles btnExportmap.Click
        Dim ExportFileName As String
        Dim ExportStreamWriter As StreamWriter
        ExportFileName = Path.GetDirectoryName(UploadCRFile.OpenFileDialog1.FileName) & "\" & cboMap.SelectedValue & ".map"
        If File.Exists(ExportFileName) Then
            Dim mans As MsgBoxResult
            mans = MsgBox("Map file " & ExportFileName & " exists.  Do you want to overwrite it?", vbYesNoCancel, "Export Map")
            If mans = vbYes Then
                File.Delete(ExportFileName)
            Else
                Exit Sub
            End If
        End If
        ExportStreamWriter = New StreamWriter(ExportFileName, True)
        Dim lGetMapJsonCmd As String = "select * from _AAC_CRMAPz 
                                        Join _AAC_CRMAP  on _AAC_CRMAP.MapCode = _aac_crmapz.mapcode 
                                        where _AAC_CRMAPZ.mapcode = '" & cboMap.SelectedValue & "'  for json auto"
        Debug.Print(lGetMapJsonCmd.Length)
        SQLCtl.ExecQuery(lGetMapJsonCmd, gSQLConnection)
        Dim lRow As DataRow
        For Each lRow In SQLCtl.sqlds.Tables(0).Rows
            ExportStreamWriter.Write(lRow(0).ToString)
        Next
        ExportStreamWriter.Flush()
        ExportStreamWriter.Close()
        ExportStreamWriter.Dispose()
    End Sub

    Private Sub btnImportMap_Click(sender As Object, e As EventArgs) Handles btnImportMap.Click
        With OpenFileDialog1
            .InitialDirectory = Path.GetDirectoryName(UploadCRFile.OpenFileDialog1.FileName)
            .Filter = "CR Import Maps|*.map"
            .ShowDialog()

            If .FileName = "" Then
                Exit Sub
            End If
        End With
        Dim ImportFileName As String = OpenFileDialog1.FileName
        Dim ImportStreamReader As StreamReader
        Dim JSON As String
        ImportStreamReader = New StreamReader(ImportFileName)
        JSON = ImportStreamReader.ReadLine
        ImportStreamReader.Close()
        ImportStreamReader.Dispose()

        Dim ImportCmd As String = "exec _spaac_CR_Import_Map @JSON = '" & JSON & "'"
        SQLCtl.ExecCmd(ImportCmd, gSQLConnection)
    End Sub
End Class