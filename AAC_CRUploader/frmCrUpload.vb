Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Xml


Public Class UploadCRFile
    Private Protected FormLoad As Boolean
    Private Protected ReceiptKeyColumn As String
    Private Protected TargetTableName As String
    Protected Friend CurrSelectedMap As String

    Private Protected SourceFileType As String
    Private Protected SourceFileName As String
    Private Protected SourceXLSheetName As String
    Private Protected SourceFileOpenMask As String

    Private Protected gdtMap As DataTable
    Private Protected gAutoRun As Boolean = False
    Private Protected gAutoFile As String = ""
    Private Protected gAutoMap As String = ""
    Private Protected gSQLConnection As SqlConnection
    Private Protected gdtReceipts As DataTable
    Protected Friend gCRDataFile As New DataSet
    Private Protected gdtMaps As New DataTable
    Private Protected SQLCtl As New SQLControl
    Private Protected gLineNumber As Integer = 0 'Input Line Number
    Private Protected gImportNum As Integer = 0 ' Batch number for file
    Private Protected gTranLineNumber As Integer = 0 'sequence within trannumber
    Private Protected gTranNumber As Integer = 0 'Counter for unique receipt
    Private Protected gSourceDataPath As String = ""
    Private Protected gImportLogName As String = ""
    Private Protected gswLogFile As StreamWriter


    Private Sub UploadCRFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'chkLogDebug.Checked = True
        gImportLogName = Application.StartupPath() & My.Application.Info.AssemblyName & ".log"
        gswLogFile = New StreamWriter(gImportLogName, True)
        StatusStrip1.Items(0).Text = "Load"
        Dim args As String()
        args = Environment.GetCommandLineArgs
        For argIx = 1 To args.Count - 1
            If args(argIx) = "AUTORUN" Then
                gAutoRun = True
            End If
            'Map should load before file
            If args(argIx).Length < 5 Then
                gAutoMap = args(argIx)
            End If
            If args(argIx).Contains(".") Then
                gAutoFile = args(argIx)
            End If
        Next argIx

        FormLoad = True

        gSQLConnection = New SqlClient.SqlConnection
        Dim lConnectionString As String
        btnOpen.Enabled = False
        btnUpload.Enabled = False
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
        Dim lCmdText As String = "Select Mapcode, MapDesc, FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'"

        SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
        gdtMaps = SQLCtl.sqlds.Tables(0)
        With cboMap
            .DataSource = SQLCtl.sqlds.Tables(0)
            .DisplayMember = "MapDesc"
            .ValueMember = "MapCode"
        End With

        FormLoad = False
        If gAutoMap <> "" Then
            'cboMap.SelectedIndex = 0
            cboMap.SelectedValue = gAutoMap
            cboMap_SelectedIndexChanged(sender, e)
            Application.DoEvents()
        End If
        If gAutoFile <> "" Then
            txtCRFile.Text = gAutoFile
            OpenFileDialog1.FileName = gAutoFile
            txtCRFile_TextChanged(sender, e)
        End If
        If gAutoRun Then
            btnUpload_Click(sender, e)
            btnClose_Click(sender, e)
        End If
        'If cboMap.SelectedText <> "" Then cboMap_SelectedIndexChanged(sender, e)
        'If txtCRFile.Text <> "" Then txtCRFile_TextChanged(sender, e)
    End Sub



    Private Sub cboMap_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedIndexChanged
        '================================================================================================================
        '                           MAP SELECTED    MAP SELECTED    MAP SELECTED    MAP SELECTED    MAP SELECTED        '
        '================================================================================================================
        ' cboMap has all defined mappings.  When selected, load the details for that mapping to the gdtMap Data table
        CurrSelectedMap = cboMap.SelectedValue.ToString
        If Not FormLoad And CurrSelectedMap <> "" Then
            Try

                Dim lCmdText As String = ""
                'Ensure SQL sequenece exists
                lCmdText = "if not exists (select * from sys.sequences where schema_name(schema_id) = 'sequence' and name = 'import_num') " &
                        "CREATE SEQUENCE [Sequence].[Import_Num]  START WITH 1"

                ''Dim lcmd As New SqlCommand(lCmdText, gSQLConnection)
                ''lcmd.ExecuteNonQuery()
                SQLCtl.ExecQuery(lCmdText, gSQLConnection)



                lCmdText = "Select Mapcode, ApplicationType, SourceColumnLabel, TargetColumn, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;" &
                       "Select * from _aac_crmapz where mapcode = '" & CurrSelectedMap & "';"

                ''Dim ldaMap As New SqlDataAdapter(lCmdText, gSQLConnection)
                Dim ldsMap = New DataSet
                ''ldaMap.Fill(ldsMap)

                SQLCtl.ExecQuery(lCmdText, gSQLConnection)
                ldsMap = SQLCtl.sqlds



                gdtMap = ldsMap.Tables(0)
                dbMap.DataSource = gdtMap

                dbMapz.DataSource = ldsMap.Tables(1)
                ReceiptKeyColumn = ldsMap.Tables(1).Rows(0)("ReceiptID") 'Get identifier for receipt object
                TargetTableName = ldsMap.Tables(1).Rows(0)("TargetTable") 'Get Name of target table
                SourceFileType = ldsMap.Tables(1).Rows(0)("FileType") 'Get file type of source table
                SourceFileOpenMask = ldsMap.Tables(1).Rows(0)("FileMask") 'Get file mask for open dialog
                SourceXLSheetName = ldsMap.Tables(1).Rows(0)("XLSheetName").ToString 'Get Sheet Name
                OpenFileDialog1.Filter = "CR Excel Import Files|" & SourceFileOpenMask 'Get identifier for receipt object

                'If ldsMap.Tables(2).Rows.Count > 0 Then
                '    gImportNum = ldsMap.Tables(2).Rows(0)(0)
                'End If

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


    Sub WriteDataViaMap(pDataRow As DataRow, pMapTable As DataTable)
        '================================================================================================================
        '                           WRITE CURRENT DATAROW TO SQL STAGING TABLE USING MAP                                         '
        '================================================================================================================

        ' The MAPPING datatable, pMapTable, has instructions on how to populate the target with data from the provided datarow, pDataRow
        ' THis logic will read each item in the map and use it to construct an insert statement into the target table
        Dim txtPreamble As String = "insert into " & TargetTableName & " ("
        Dim txtValues As String = ""
        For ColIX = 0 To pMapTable.Rows.Count - 1
            txtPreamble &= (pMapTable.Rows(ColIX)("TargetColumn")) + ", "
        Next ColIX
        'txtPreamble = txtPreamble.Substring(0, txtPreamble.Length - 2) & ")" & vbCrLf
        txtPreamble = txtPreamble & "_ImportStatus)" & vbCrLf

        txtValues = "Values ("
        For colix = 0 To pMapTable.Rows.Count - 1
            Select Case pMapTable.Rows(colix)("DataType")
                Case "K" ' C(K)onstant
                    txtValues &= "'" & (pMapTable.Rows(colix)("SourceColumnLabel")) & "', "
                Case "R" 'Replacement Variable
                    Select Case pMapTable.Rows(colix)("SourceColumnLabel").ToString.ToUpper
                        Case "%FileName%".ToUpper
                            txtValues &= "'" & OpenFileDialog1.FileName & "', "
                        Case "%ImportNum%".ToUpper
                            txtValues &= gImportNum.ToString & ", "
                        Case "%Time%".ToUpper
                            txtValues &= "getdate(), "
                        Case "%User%".ToUpper
                            txtValues &= "Suser_Sname(), "
                            ' Case "%LineNum%"
                       '     txtValues &= pDataRow("__ROWNUM").ToString & ", " '"0, " 'ReceiptLineSeq.ToString + ", "
                        Case "%TranLine%".ToUpper
                            txtValues &= gTranLineNumber.ToString + ", "
                        Case "%TranNum%".ToUpper
                            txtValues &= gTranNumber.ToString + ", "

                        Case Else
                            txtValues &= "'" & pMapTable.Rows(colix)("SourceColumnLabel") & "', "
                    End Select
                Case Else
                    txtValues &= "'" & pDataRow(pMapTable.Rows(colix)("SourceColumnLabel")) & "', "

            End Select

        Next colix
        'txtValues = txtValues.Substring(0, txtValues.Length - 2) & ")"
        txtValues = txtValues & "'N')"
        Debug.Print(txtPreamble & txtValues)
        Dim UpdateCommand As String
        'Dim lcmd As New SqlCommand
        'lcmd.Connection = gSQLConnection
        'lcmd.CommandText = txtPreamble & txtValues
        'lcmd.ExecuteNonQuery()

        UpdateCommand = txtPreamble & txtValues
        SQLCtl.ExecCmd(UpdateCommand, gSQLConnection)

    End Sub

    Function GetDistinctRows(pType As String, pDataSource As DataTable, Optional pReceiptID As String = "") As DataTable
        '================================================================================================================
        '                           GET SOURCE COLUMNS MAPPED FOR THIS TYPE ('L's)                                      '
        '================================================================================================================
        Try
            ' For each type, get distinct values for mapped columns                                                         TWO
            Dim ThisType As String = pType
            Dim RTypeColumnFilter As String
            Dim ColumnList As New List(Of String)
            ' Get distinct values of all mapped columns for thistype 
            RTypeColumnFilter = "ApplicationType = '" & ThisType & "' and DataType = 'L'"

            Dim SourceColumnsThisType As DataRow() = gdtMap.Select(RTypeColumnFilter)
            Dim dtThisType As DataTable = gdtMap.Clone()
            dtThisType = SourceColumnsThisType.CopyToDataTable

            ColumnList = New List(Of String)
            For ColIX = 0 To dtThisType.Rows.Count - 1
                ColumnList.Add(dtThisType.Rows(ColIX)("SourceColumnLabel"))
            Next ColIX

            Dim distinctRcptRowsThisType As DataTable = pDataSource.DefaultView.ToTable(True, ColumnList.ToArray())

            If pReceiptID <> "" Then
                RTypeColumnFilter = "[" & ReceiptKeyColumn & "] = '" & pReceiptID & "'"
                GetDistinctRows = distinctRcptRowsThisType.Select(RTypeColumnFilter).CopyToDataTable
            Else
                RTypeColumnFilter = "[" & ReceiptKeyColumn & "] <> ''"
                GetDistinctRows = distinctRcptRowsThisType.Select(RTypeColumnFilter).CopyToDataTable
            End If
        Catch ex As Exception
            Dim mresponse As MsgBoxResult
            mresponse = MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in GetDistinctRows")
            GetDistinctRows = pDataSource.Clone 'Return empty copy
        End Try
        If chkLogDebug.Checked Then DisplayDT(GetDistinctRows, 100)
    End Function

    Function GetTypeMap(pType As String) As DataTable
        '================================================================================================================
        '                           GET ALL MAPPINGS THIS TYPE                                                          '
        '================================================================================================================
        Dim TypeFilter As String
        Dim SourceRowsMatchingType As DataRow()
        TypeFilter = "ApplicationType = '" & pType & "' and DataType <> ''"
        SourceRowsMatchingType = gdtMap.Select(TypeFilter)
        GetTypeMap = SourceRowsMatchingType.CopyToDataTable
    End Function

    Private Sub txtCRFile_TextChanged(sender As Object, e As EventArgs) Handles txtCRFile.TextChanged
        '================================================================================================================
        '                           FILL gCRDataFile FROM SOURCE (XLS ot CSV)                                           '
        '================================================================================================================
        btnUpload.Enabled = False
        Dim CnStr As String '= ""
        If FormLoad <> True And txtCRFile.Text <> "" Then
            Dim fileobject As FileInfo
            fileobject = New FileInfo(txtCRFile.Text)

            If fileobject.Exists Then
                ' If ACE driver does not exist, use JET driver 
                If Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Microsoft.ACE.OLEDB.12.0\CLSID") Is Nothing Then
                    CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(txtCRFile.Text) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
                Else
                    CnStr = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" & Path.GetDirectoryName(txtCRFile.Text) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
                End If
                gCRDataFile = New DataSet

                If SourceFileType = "CSV" Then
                    Try
                        Dim CSVSelect As String = "select * from [" & Path.GetFileName(txtCRFile.Text) & "]"
                        Using Adp As New OleDbDataAdapter(CSVSelect, CnStr)
                            Adp.Fill(gCRDataFile)
                        End Using
                        'gvData.DataSource = gCRDataFile.Tables(0)
                        'btnUpload.Enabled = True
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Critical, "Error in CRFile.TextChanged event.")
                    End Try
                End If
                If SourceFileType = "XLS" Then
                    Try
                        Dim CSVSelect As String = "select * from [" & SourceXLSheetName & "]"
                        Using Adp As New OleDbDataAdapter(CSVSelect, CnStr)
                            Adp.Fill(gCRDataFile)
                        End Using
                        'gvData.DataSource = gCRDataFile.Tables(0)
                        'btnUpload.Enabled = True
                    Catch ex As Exception
                        MsgBox("Verify range " + SourceXLSheetName + " exists In file " + txtCRFile.Text + vbCrLf + ex.Message, MsgBoxStyle.Critical, "CRFile.TextChanged Event. ")
                    End Try
                End If
                gvData.DataSource = gCRDataFile.Tables(0)
                btnUpload.Enabled = True

            End If

            gSourceDataPath = Path.GetDirectoryName(txtCRFile.Text)
            If gswLogFile IsNot Nothing Then
                gswLogFile.Close()
            End If
            gImportLogName = Path.GetDirectoryName(txtCRFile.Text) & "\" & Path.GetFileNameWithoutExtension(txtCRFile.Text) & ".log"
            gswLogFile = New StreamWriter(gImportLogName, True)
        End If
    End Sub

    Private Sub cboMap_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedValueChanged
        If Not FormLoad Then
            txtCRFile_TextChanged(sender, e)
        End If

    End Sub
    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        '================================================================================================================
        '                           PROCESS IMPORT FILE AND LOAD TO DATABASE STAGING TABLE                              '
        '================================================================================================================


        ' ldtMap hold the current mapping spec.  This includes multiple ApplicationType mappings.
        ' Application types include, among others, 'R'eceipts Headers, 'A'pplications to 'B'Ills
        '  Get distinct types from mapping,  then for each type, get disctinct values

        ' 7/16 - will assume each file should be it's own session.
        ' R types will create an R row in the staging table. THere should be one R row per check - correspondes to CRT_CASH row
        ' B' types will be an bill invoice payment and correspond to BLT_BILL_AMT CR trans - Can be multiple per R type.  Shares receipt ID linkint B to R
        '
        '' --Define datatable to hold receipt data
        'gdtReceipts = New DataTable()
        'gdtReceipts.Columns.Add("ReceiptId", System.Type.GetType("System.String"))
        'gdtReceipts.Columns.Add("ReceiptNum", System.Type.GetType("System.Int32"))


        StatusStrip1.Items(0).Text = "Start"
        WriteLog("Start Upload Of " & txtCRFile.Text)

        Dim lcmd As New SqlCommand
        lcmd.Connection = gSQLConnection

        ' // Test if file has already been processed by checking if it is in the load file //
        Dim lCmdText As String = "Select count(*) As 'Count', max(_StagingDate) 'StagingDate' from " + TargetTableName + " where _SourceFile = '" & OpenFileDialog1.FileName & "'"
        ''Dim ldaHist As New SqlDataAdapter(lCmdText, gSQLConnection)
        Dim ldtHist As DataTable  '' was new
        SQLCtl.ExecQuery(lCmdText, gSQLConnection)
        ldtHist = SQLCtl.sqlds.Tables(0)
        ''ldaHist.Fill(ldtHist)
        If ldtHist.Rows(0)("Count") <> 0 Then
            Dim mresp As MsgBoxResult
            mresp = MsgBox("File " & Path.GetFileName(OpenFileDialog1.FileName) & " has already been processed." + vbCrLf + "Do you want to re-upload it?" & vbCrLf + "Last upload was on " & ldtHist.Rows(0)("StagingDate").ToString, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Duplicate Upload Name")
            If mresp <> MsgBoxResult.Yes Then
                Exit Sub '  Do not want to re-process - get out of this routine
            End If
        End If


        'Open source data table                                                                                      dtCRData
        Dim dtCRData As New DataTable ' This is the full contents
        dtCRData = gCRDataFile.Tables(0).Copy
        If chkLogDebug.Checked Then WriteLog("Copied gCRDataFile to dtCRData")
        DisplayDT(dtCRData, 100)
        'Dim strCRSelect As String = "select * from [" & Path.GetFileName(OpenFileDialog1.FileName) & "]"
        'Dim csvConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(OpenFileDialog1.FileName) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
        'Using csvDataAdapter As New OleDbDataAdapter(strCRSelect, csvConnection)
        '    csvDataAdapter.Fill(dtCRData)
        'End Using


        ' For each type, get distinct values for mapped columns                                                         distinctRcptHeaders.
        If chkLogDebug.Checked Then WriteLog("Get Distinct R rows from input file")
        Dim distinctRcptHeaders As DataTable = GetDistinctRows("R", dtCRData)

        If chkLogDebug.Checked Then WriteLog("Get TypeMap dtRcptHeaderMap for R")
        Dim dtRcptHeaderMap As DataTable = GetTypeMap("R")
        DisplayDT(dtRcptHeaderMap, 100)

        ' For each type, get distinct values for mapped columns and get mapping                                                         distinctRcptRowsThisType  I (Bills)
        Dim ThisType As String = "I"
        Dim distinctRcptRowsThisType As DataTable
        If chkLogDebug.Checked Then WriteLog("Get Distinct Rows for type " & ThisType)
        distinctRcptRowsThisType = GetDistinctRows(ThisType, dtCRData)

        If chkLogDebug.Checked Then WriteLog("Get map for " & ThisType)
        Dim dtThistypeMap As DataTable = GetTypeMap(ThisType)
        DisplayDT(dtThistypeMap, 100)

        SQLCtl.ExecQuery("Select next value for sequence.import_num as Import_Num", gSQLConnection)
        gImportNum = SQLCtl.sqlds.Tables(0).Rows(0)(0)

        ' then loop through for each subset matching the ReceiptKeyColumn                                               Process all R Receipts
        Dim ThisTypeThisReceipt As DataTable
        Dim distinctRcptHeader As DataRow
        Dim TypeFilter As String = ""
        gTranNumber = 0
        For Each distinctRcptHeader In distinctRcptHeaders.Rows   '                                                      Receipt Header Loop
            gTranNumber += 1
            gTranLineNumber = 1
            'Write R row followed by all details rows mapped
            '  distinctRcptHeader is the current receipt header data

            Debug.Print("Start Processing: " & distinctRcptHeader(ReceiptKeyColumn).ToString)
            'Write HeaderRow to SQL
            WriteDataViaMap(distinctRcptHeader, dtRcptHeaderMap)

            'Filter I rows and write to SQL
            TypeFilter = "[" & ReceiptKeyColumn & "] = '" & distinctRcptHeader(ReceiptKeyColumn).ToString & "'"
            Dim SourceRowsMatchingType As DataRow() = distinctRcptRowsThisType.Select(TypeFilter)

            'ThisTypeThisReceipt = SourceRowsMatchingType.CopyToDataTable
            If chkLogDebug.Checked Then WriteLog("Get GetDistinctRows for I, " & distinctRcptHeader(ReceiptKeyColumn).ToString)
            ThisTypeThisReceipt = GetDistinctRows("I", dtCRData, distinctRcptHeader(ReceiptKeyColumn).ToString)

            Dim ItemDatarow As DataRow
            For Each ItemDatarow In ThisTypeThisReceipt.Rows
                gTranLineNumber += 1
                WriteDataViaMap(ItemDatarow, dtThistypeMap)
            Next

            'ThisTypeThisReceipt is the set of rows for ThisType designated for this receipt key

            ' Process the map for this type against the ThisTypeThisReceipt datatable

        Next

        MsgBox("Upload Complete:" & vbCrLf & OpenFileDialog1.FileName, MsgBoxStyle.OkOnly, "Complete")
        StatusStrip1.Items(0).Text = "Uploaded to Batch " & gImportNum.ToString
        If chkValidate.Checked Then
            WriteLog("Batch:" & gImportNum.ToString & "|Begin Validation" & Now().ToString)
            Dim cdtError As New DataTable()
            SQLCtl.ExecCmd("Exec SpAacCRPreValidateData @BatchId=" & gImportNum.ToString, gSQLConnection)
            SQLCtl.ExecQuery("select Severity, TrackingNumber, errorvalue from [_AacCRImpErrorLog] where BatchId=" & gImportNum.ToString & " Order by trackingnumber", gSQLConnection)
            cdtError = SQLCtl.sqlds.Tables(0)
            Dim lRow As DataRow
            For Each lRow In cdtError.Rows
                WriteLog("Batch:" & gImportNum.ToString & "|TrackingNumber:" & lRow(0) & "|" & lRow(1) & "|" & lRow(2))
            Next
            cdtError.Dispose()
            cdtError = New DataTable()
            SQLCtl.ExecQuery("select isnull(TrackingNumber,-1), Severity, errorvalue from [_AacCRImpErrorLog] where BatchId=" & gImportNum.ToString & " and severity <> 0 Order by trackingnumber", gSQLConnection)
            cdtError = SQLCtl.sqlds.Tables(0)
            If cdtError.Rows.Count <> 0 Then
                Dim ErrLogName As String = Path.GetDirectoryName(txtCRFile.Text) & "\" & Path.GetFileNameWithoutExtension(txtCRFile.Text) & ".err"
                Dim gswLogFileErr As New StreamWriter(ErrLogName, True)
                gswLogFile.AutoFlush = True
                gswLogFileErr.WriteLine("TimeStamp|BatchID|TrackingNum|Severity|Error")
                For Each lRow In cdtError.Rows
                    gswLogFileErr.WriteLine(Now.ToString & "|" & "Batch:" & gImportNum.ToString & "|TrackingNumber:" & lRow(0) & "|" & lRow(1) & "|" & lRow(2))
                Next
                gswLogFileErr.Flush()

                MsgBox("Could not import due to validation errors. Please review log:" & vbCrLf & ErrLogName, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Validation Errors")
            Else
                ' Call procedure to create Expert sessions (_spaac_importcr)
                SQLCtl.ExecCmd("Exec _spaac_ImportCR @NoCommit = 0, @BatchId=" & gImportNum.ToString, gSQLConnection)
                cdtError.Dispose()
                cdtError = New DataTable()
                SQLCtl.ExecQuery("select max(_Session) from _aac_CR_load where BatchId=" & gImportNum.ToString, gSQLConnection)
                cdtError = SQLCtl.sqlds.Tables(0)
                StatusStrip1.Items(0).Text &= "; Created Session " & cdtError.Rows(0)(0).ToString
            End If
            cdtError.Dispose()
        End If
    End Sub

    Private Sub btnEditMap_Click(sender As Object, e As EventArgs) Handles btnEditMap.Click
        frmEditMap.ShowDialog(Me)

        FormLoad = True
        SQLCtl.ExecQuery("Select Mapcode, MapDesc, FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
        gdtMaps = SQLCtl.sqlds.Tables(0)
        With cboMap
            .DataSource = gdtMaps
            .DisplayMember = "MapDesc"
            .ValueMember = "MapCode"
            .Text = CurrSelectedMap
            .Refresh()
        End With
        FormLoad = False
        cboMap_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub btnMapRefresh_Click_1(sender As Object, e As EventArgs) Handles btnMapRefresh.Click
        cboMap_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub btnViewLog_Click(sender As Object, e As EventArgs) Handles btnViewLog.Click
        If System.IO.File.Exists(gImportLogName) = True Then
            Process.Start(gImportLogName)
        Else
            MsgBox("Error opening log file. " & gImportLogName & " Not found.")
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
        If gswLogFile IsNot Nothing Then
            gswLogFile.Close()
        End If
        Me.Close()
        End
    End Sub



    Sub DisplayDT(Pdt As DataTable, pMaxRows As Integer)
        ' this routine can be used for debugging to display the contents of a datatable
        If pMaxRows > Pdt.Rows.Count Then
            pMaxRows = Pdt.Rows.Count
        End If

        Dim DebugLine As String = ""
        For c = 0 To Pdt.Columns.Count - 1
            DebugLine &= Pdt.Columns(c).ColumnName & "|"
        Next c
        Debug.Print(DebugLine)
        If chkLogDebug.Checked Then
            WriteLog(DebugLine)
        End If
        DebugLine = ""
        For r = 0 To pMaxRows - 1
            For c = 0 To Pdt.Columns.Count - 1
                DebugLine &= Pdt.Rows(r)(c) & "|"
            Next c
            Debug.Print(DebugLine)
            If chkLogDebug.Checked Then
                WriteLog(DebugLine)
            End If
            DebugLine = ""

        Next r
        If chkLogDebug.Checked Then WriteLog("--------------------------")
    End Sub
    Sub WriteLog(pString As String)
        If gswLogFile IsNot Nothing Then
            gswLogFile.WriteLine(Now.ToString & "|" & pString)
            gswLogFile.Flush()
        End If
        StatusStrip1.Items(0).Text = pString
    End Sub

    Private Sub ProgramAbend()
        If gSQLConnection.State = ConnectionState.Open Then
            gSQLConnection.Close()
        End If
        If gswLogFile IsNot Nothing Then
            gswLogFile.Close()
        End If
        End
    End Sub

    Class
