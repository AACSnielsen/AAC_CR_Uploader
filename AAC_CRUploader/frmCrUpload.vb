﻿Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Xml


Public Class UploadCRFile
    Protected Friend FormLoad As Boolean
    Protected Friend ReceiptId As String
    Protected Friend gdtMap As DataTable
    Protected Friend gImportNum As Integer = -1
    Protected Friend gAutoRun As Boolean = False
    Protected Friend gAutoFile As String = ""
    Protected Friend gAutoMap As String = ""
    Protected Friend gSQLConnection As SqlConnection

    Protected Friend gCRDataFile As New DataSet


    Private Sub UploadCRFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim args As String()
        args = Environment.GetCommandLineArgs
        For argIx = 1 To args.Count - 1
            If args(argIx) = "AUTORUN" Then
                gAutoRun = True
            End If
            If args(argIx).Contains(".") Then
                gAutoFile = args(argIx)
            End If
            If args(argIx).Length < 5 Then
                gAutoMap = args(argIx)
            End If

        Next argIx

        FormLoad = True

        gSQLConnection = New SqlClient.SqlConnection
        Dim lConnectionString As String
        btnOpen.Enabled = False
        txtCRFile.Enabled = False

        Dim lServer As String = "" '= "SDN-ENVY-2020\SDN_HPENVY"
        Dim lDatabase As String = "" '= "TestDB"
        Dim AppPath As String = System.AppDomain.CurrentDomain.BaseDirectory()
        If File.Exists(AppPath & "Instance.config") Then 'Get connection info
            Dim ConfigXML As New XmlDocument() 'XDocument = XDocument.Load(AppPath & "Instance.config")
            ConfigXML.Load(AppPath & "Instance.config")
            Dim RepositoryNode As XmlNode = ConfigXML.GetElementsByTagName("repository")(0)
            lServer = (RepositoryNode.Attributes("server").Value)
            lDatabase = (RepositoryNode.Attributes("name").Value)
        End If
        If lServer = "" Then ' Error - no config found
            MsgBox("No connection info found in Instance.config or file is missing." & vbCrLf & "(" + AppPath & ")" & vbCrLf & "<instanceMetadataConfigurationSection><repository name={database} server={server}...>", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "No valid connection information")
            End
        End If

        lConnectionString = "Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;"
        gSQLConnection.ConnectionString = lConnectionString
        gSQLConnection.Open()
        Dim lCmdText As String = "Select Mapcode, MapDesc, FileMask, FileType from _aac_CRMAPZ where inactive <> 'Y'"

        Dim ldaMaps As New SqlDataAdapter(lCmdText, gSQLConnection)
        Dim ldtMaps As New DataTable
        ldaMaps.Fill(ldtMaps)

        With cboMap
            .DataSource = ldtMaps
            .DisplayMember = "MapDesc"
            .ValueMember = "MapCode"

        End With

        'gSQLConnection.Close()
        FormLoad = False
        If gAutoMap <> "" Then
            cboMap.SelectedIndex = 0
            cboMap.SelectedValue = gAutoMap
            Application.DoEvents()
        End If
        If gAutoFile <> "" Then
            txtCRFile.Text = gAutoFile
            OpenFileDialog1.FileName = gAutoFile
        End If
        If gAutoRun Then
            btnUpload_Click(sender, e)
            btnClose_Click(sender, e)


        End If
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        OpenFileDialog1.ShowDialog()
        txtCRFile.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If gSQLConnection.State = ConnectionState.Open Then
            gSQLConnection.Close()
        End If
        Me.Close()
        End
    End Sub

    Private Sub cboMap_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedIndexChanged
        ' cboMap has all defined mappings.  When selected, load the details for that mapping to the gdtMap Data table
        If Not FormLoad Then
            Try
                Dim lServer As String = "SDN-ENVY-2020\SDN_HPENVY"
                Dim lDatabase As String = "TestDB"
                'Dim gSQLConnection As New SqlClient.SqlConnection
                'Dim lConnectionString As String
                'lConnectionString = "Server=" & lServer & "; Database=" & lDatabase & ";Integrated Security=SSPI;"
                'gSQLConnection.ConnectionString = lConnectionString
                'gSQLConnection.Open()
                Dim lCmdText As String = ""
                'Endsure SQL sequenece exists
                lCmdText = "if not exists (select * from sys.sequences where schema_name(schema_id) = 'sequence' and name = 'import_num') " &
                        "CREATE SEQUENCE [Sequence].[Import_Num]  START WITH 1"
                Dim lcmd As New SqlCommand(lCmdText, gSQLConnection)
                lcmd.ExecuteNonQuery()

                lCmdText = "Select Mapcode, ApplicationType, SourceColumnLabel, TargetColumn, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;" &
                       "Select MapCode, MapDesc, FileMask, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
                       "Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
                Dim ldaMap As New SqlDataAdapter(lCmdText, gSQLConnection)
                Dim ldsMap = New DataSet
                ldaMap.Fill(ldsMap)

                gdtMap = ldsMap.Tables(0)
                dbMap.DataSource = gdtMap

                dbMapz.DataSource = ldsMap.Tables(1)
                ReceiptId = ldsMap.Tables(1).Rows(0)("ReceiptID") 'Get identifier for receipt object
                OpenFileDialog1.Filter = "CR IMport Files|" & ldsMap.Tables(1).Rows(0)("FileMask") 'Get identifier for receipt object

                If ldsMap.Tables(2).Rows.Count > 0 Then
                    gImportNum = ldsMap.Tables(2).Rows(0)(0)
                End If

                btnOpen.Enabled = True
                txtCRFile.Enabled = True
            Catch err As Exception
                Dim mresp As MsgBoxResult
                mresp = MsgBox("Unexpected Error:" & err.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Error in cboMap.SelectedIndexChanged")
                If mresp = MsgBoxResult.Cancel Then
                    ProgramAbend()

                End If
                Debug.Print(e.ToString)
            End Try

        End If
    End Sub
    Private Sub ProgramAbend()
        If gSQLConnection.State = ConnectionState.Open Then
            gSQLConnection.Close()
        End If
        End
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        ' ldtMap hold the current mapping spec.  This includes multiple ApplicationType mappings.
        ' Application types include, among others, 'R'eceipts Headers, 'A'pplications to BIlls
        '  Get distinct types from mapping,  then for each type, get disctinct values
        Dim lcmd As New SqlCommand
        lcmd.Connection = gSQLConnection

        Dim lCmdText As String = "select count(*) as 'Count', max(_StagingDate) 'StagingDate' from _aac_CR_load where _SourceFile = '" & OpenFileDialog1.FileName & "'"
        Dim ldaHist As New SqlDataAdapter(lCmdText, gSQLConnection)
        Dim ldtHist As New DataTable
        ldaHist.Fill(ldtHist)
        If ldtHist.Rows(0)("Count") <> 0 Then
            Dim mresp As MsgBoxResult
            mresp = MsgBox("File " & Path.GetFileName(OpenFileDialog1.FileName) & " has already been processed." + vbCrLf + "Do you want to re-upload it?" & vbCrLf + "Last upload was on " & ldtHist.Rows(0)("StagingDate").ToString, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Duplicate Upload Name")
            If mresp <> MsgBoxResult.Yes Then
                Exit Sub

            End If
        End If

        'Open source data table
        Dim dtCRData As New DataTable
        Dim strCRSelect As String = "select * from [" & Path.GetFileName(OpenFileDialog1.FileName) & "]"
        Dim csvConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(OpenFileDialog1.FileName) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
        Using csvDataAdapter As New OleDbDataAdapter(strCRSelect, csvConnection)
            csvDataAdapter.Fill(dtCRData)
        End Using


        Dim distinctTypes As DataTable = gdtMap.DefaultView.ToTable(True, "ApplicationType")
        For TypeIX = 0 To distinctTypes.Rows.Count - 1
            Debug.Print("Processing Type = " & distinctTypes.Rows(TypeIX)(0).ToString)
            'Get distinct mapping rows for select map and type
            Dim TypeFilter As String = "ApplicationType = '" & distinctTypes.Rows(TypeIX)(0).ToString & "' and DataType = 'L'"
            Dim ThisTypeRows As DataRow() = gdtMap.Select(TypeFilter)
            Dim dtThisType As DataTable = gdtMap.Clone()
            dtThisType = ThisTypeRows.CopyToDataTable
            ' At this point dtThisType should have the mapping for the iterative data type (R, B, ...)
            ' Process each row in the dtThistype mapping against all (distinct) input rows to write to database table
            '  First set column list to select:
            Dim ColumnList As New List(Of String)
            For ColIX = 0 To dtThisType.Rows.Count - 1
                ColumnList.Add(dtThisType.Rows(ColIX)("SourceColumnLabel"))
            Next ColIX

            'Update the map to include all columns
            TypeFilter = "ApplicationType = '" & distinctTypes.Rows(TypeIX)(0).ToString & "' and DataType <> ''"
            ThisTypeRows = gdtMap.Select(TypeFilter)
            dtThisType = ThisTypeRows.CopyToDataTable
            Dim txtPreamble As String = "insert into _aac_CR_LOAD ("
            Dim txtValues As String = ""
            For ColIX = 0 To dtThisType.Rows.Count - 1
                txtPreamble &= (dtThisType.Rows(ColIX)("TargetColumn")) + ", "
            Next ColIX
            txtPreamble = txtPreamble.Substring(0, txtPreamble.Length - 2) & ")" & vbCrLf

            'Now select the distinct values for the indiacted columns
            Dim ReceiptLineSeq As Integer = 0
            Dim CurrReceiptID As Integer = -1
            Dim distinctRcpt As DataTable = dtCRData.DefaultView.ToTable(True, ColumnList.ToArray())

            For rcptix = 0 To distinctRcpt.Rows.Count - 1
                If CurrReceiptID <> distinctRcpt.Rows(rcptix)(ReceiptId) Then
                    ReceiptLineSeq = 0
                    CurrReceiptID = distinctRcpt.Rows(rcptix)(ReceiptId)
                End If
                ReceiptLineSeq += 1
                txtValues = "Values ("
                For colix = 0 To dtThisType.Rows.Count - 1
                    Select Case dtThisType.Rows(colix)("DataType")
                        Case "K" ' C(K)onstant
                            txtValues &= "'" & (dtThisType.Rows(colix)("SourceColumnLabel")) & "', "
                        Case "R" 'Replacement Variable
                            Select Case (dtThisType.Rows(colix)("SourceColumnLabel"))
                                Case "%FileName%"
                                    txtValues &= "'" & OpenFileDialog1.FileName & "', "
                                Case "%ImportNum%"
                                    txtValues &= gImportNum.ToString & ", "
                                Case "%Time%"
                                    txtValues &= "getdate(), "
                                Case "%User%"
                                    txtValues &= "Suser_Sname(), "
                                Case "%TranLine%"
                                    txtValues &= ReceiptLineSeq.ToString + ", "
                                Case Else
                                    txtValues &= "'" & dtThisType.Rows(colix)("SourceColumnLabel") & "', "
                            End Select
                        Case Else
                            txtValues &= "'" & distinctRcpt.Rows(rcptix)(dtThisType.Rows(colix)("SourceColumnLabel")) & "', "
                    End Select

                Next colix
                txtValues = txtValues.Substring(0, txtValues.Length - 2) & ")"
                Debug.Print(txtPreamble & txtValues)
                If chkNoUpdate.Checked = CheckState.Unchecked Then
                    lcmd.CommandText = txtPreamble & txtValues
                    lcmd.ExecuteNonQuery()
                End If
            Next rcptix

        Next TypeIX
        MsgBox("Upload Complete:" & vbCrLf & OpenFileDialog1.FileName, MsgBoxStyle.OkOnly, "Complete")
        'Dim DistinctReceipts = From row In dssample.Tables(0).AsEnumerable()
        ' Select Case row.Field(Of String)(dssample.Tables(0))

    End Sub

    Private Sub txtCRFile_TextChanged(sender As Object, e As EventArgs) Handles txtCRFile.TextChanged
        If txtCRFile.Text <> "" Then
            Dim fileobject As FileInfo
            fileobject = New FileInfo(txtCRFile.Text)
            If fileobject.Exists Then
                Try
                    gCRDataFile = New DataSet
                    Dim CSVSelect As String = "select * from [" & Path.GetFileName(txtCRFile.Text) & "]"
                    Dim CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(txtCRFile.Text) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"

                    Using Adp As New OleDbDataAdapter(CSVSelect, CnStr)
                        Adp.Fill(gCRDataFile)
                    End Using

                    gvData.DataSource = gCRDataFile.Tables(0)
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Error in CRFile.change event.")
                End Try
            End If
        End If


    End Sub

    Private Sub cboMap_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedValueChanged
        If Not FormLoad Then
            txtCRFile_TextChanged(sender, e)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnEditMap.Click
        frmEditMap.Show()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        cboMap_SelectedIndexChanged(sender, e)
    End Sub
End Class
