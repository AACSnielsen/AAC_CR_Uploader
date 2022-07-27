Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Xml
'Imports System.Data.OleDb
'Imports System.IO


Public Class frmEditMap
    Protected Friend gdtMap As DataTable
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
            lCmdText = "Select * from _aac_CRMAPZ where mapcode = '" & cboMap.SelectedValue & "'"
            SQLCtl.ExecQuery(lCmdText, gSQLConnection)
            txtReceiptID.Text = SQLCtl.sqlds.Tables(0).Rows(0)("ReceiptID").ToString
            txtFileMask.Text = SQLCtl.sqlds.Tables(0).Rows(0)("FileMask").ToString
            txtTargetTable.Text = SQLCtl.sqlds.Tables(0).Rows(0)("TargetTable").ToString

            lCmdText = "Select RowUno, Mapcode, ApplicationType, TargetColumn, SourceColumnLabel, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;"
            '"Select MapCode, MapDesc, FileMask, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
            '"Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
            ldaMap = New SqlDataAdapter(lCmdText, gSQLConnection)
            ldsMap.clear

            ldaMap.Fill(ldsMap)

            With lbAvailableFields
                If UploadCRFile.txtCRFile.Text <> "" Then
                    For Each col In UploadCRFile.gCRDataFile.Tables(0).Columns
                        .Items.Add(col.ToString)
                    Next
                Else
                    .Items.Add("{Select file on previous screen}")
                End If
                .Items.Add("%User%")
                .Items.Add("%FileName%")
                .Items.Add("%ImportNum%")
                .Items.Add("%TranlINE%")
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
        ldaMap.Update(ldsMap)
        Dim lCmd As String = "Update _aac_CRMAPZ set receiptid = '" & txtReceiptID.Text & "', FileMask = '" & txtFileMask.Text &
                            "', TargetTable = '" & txtTargetTable.Text &
                            "' where mapcode = '" &
            cboMap.SelectedValue & "'"
        SQLCtl.ExecCmd(lCmd, gSQLConnection)
        btnSave.Enabled = False
        cboMap_SelectedIndexChanged(sender, e)
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
            lcmd = "insert into _AAC_CRMAP" &
                " select '" & txtNewCode.Text & "',	ApplicationType	,StageTable	,SourceColumnLabel	,TargetColumn	,DataType from _AAC_CRMAP where mapcode = '" & cboMap.SelectedValue & "'" &
                " insert into _AAC_CRMAPZ" &
                " select '" & txtNewCode.Text & "','" & txtNewDesc.Text & "' ,FileMask	,FileType	,Inactive	,ReceiptID	,TargetTable from _AAC_CRMAPZ where mapcode = '" & cboMap.SelectedValue & "'"
            SQLCtl.ExecCmd(lcmd, gSQLConnection)

            SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
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
End Class