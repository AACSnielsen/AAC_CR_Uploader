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

        btnSave.Enabled = False

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
            lCmdText = "Select * from _aac_CRMAPZ where mapcode = '" & cboMap.SelectedValue & "'"
            SQLCtl.ExecQuery(lCmdText)
            txtReceiptID.Text = SQLCtl.sqlds.Tables(0).Rows(0)("ReceiptID").ToString
            txtFileMask.Text = SQLCtl.sqlds.Tables(0).Rows(0)("FileMask").ToString





            lCmdText = "Select RowUno, Mapcode, ApplicationType, TargetColumn, SourceColumnLabel, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;"
            '"Select MapCode, MapDesc, FileMask, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
            '"Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
            ldaMap = New SqlDataAdapter(lCmdText, lSqlConnection)
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


            End With

            ldaMap.UpdateCommand = New SqlCommandBuilder(ldaMap).GetUpdateCommand
            btnSave.Enabled = False


        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ldaMap.Update(ldsMap)
        Dim lCmd As String = "Update _aac_CRMAPZ set receiptid = '" & txtReceiptID.Text & "', FileMask = '" & txtFileMask.Text & "' where mapcode = '" &
            cboMap.SelectedValue & "'"
        SQLCtl.ExecCmd(lCmd)
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub dbMap_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dbMap.CellValueChanged
        btnSave.Enabled = True
    End Sub
End Class