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
    Private Protected UseSchemaName As String
    Private Protected FirstDataRow As Long



    Private Protected gdtMap As DataTable
    Private Protected gAutoRun As Boolean = False
    Private Protected gAutoFile As String = ""
    Private Protected gAutoMap As String = ""
    Private Protected gSQLConnection As SqlConnection
    Private Protected gdtReceipts As DataTable
    Protected Friend gdsSelectedCRSourceFile As New DataSet
    Private Protected gdtMaps As New DataTable
    Private Protected SQLCtl As New SQLControl
    Private Protected gLineNumber As Integer = 0 'Input Line Number
    Private Protected gImportNum As Integer = 0 ' Batch number for file
    Private Protected gTranLineNumber As Integer = 0 'sequence within trannumber
    Private Protected gTranNumber As Integer = 0 'Counter for unique receipt
    Private Protected gSourceDataPath As String = ""
    Private Protected gImportLogName As String = ""
    Private Protected gswLogFile As StreamWriter
    Private Protected gCRValidationProc As String = "SpAacCRPreValidateData"


    Private Sub UploadCRFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'chkLogDebug.Checked = True
            gImportLogName = Application.StartupPath() & "\" & My.Application.Info.AssemblyName & ".log"
            'gswLogFile = New StreamWriter(gImportLogName, True)
            ActivityText.Text = "Load"
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
            btnOpen.Enabled = False
            btnUpload.Enabled = False
            txtCRFile.Enabled = False

            ' Get Server name and data base for this environment from the expertshare instance.config file
            gSQLConnection = New SqlClient.SqlConnection
            Dim lConnectionString As String
            Dim lServer As String = ""
            Dim lDatabase As String = ""
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

            ' Check that user is authorized:
#If Not DEBUG Then
                Dim lCmdText As String = "exec security.SetContextByName '" & Environment.UserName &
                "'; execute [SECURITY].[HasResourceRight] @nodepath = 'AccountsReceivable.Custom.CRUploader', @RightName = 'Execute', @Hierarchytypeid = 3"
                SQLCtl.ExecQuery(lCmdText, gSQLConnection)
                If SQLCtl.sqlds.Tables(0).Rows.Count <> 0 Then 'Resource is defined - Check access (Else continue)
                    If SQLCtl.sqlds.Tables(0).Rows(0)(0) <> 1 Then '(0)("PermissionType") <> 1 Then
                        MsgBox("User " & Environment.UserName & " not authorized for resource <AccountsReceivable.Custom.CRUploader>", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "No valid connection information")
                        End
                    End If
                End If
                If SQLCtl.sqlds.Tables(0).Rows.Count = 0 Then 'Resource not granted
                    MsgBox("User " & Environment.UserName & " not authorized for resource <AccountsReceivable.Custom.CRUploader>", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "No valid connection information")
                    End
                End If
#End If 'Debug


            'Check config for override CR Validation Procedure.  IF not found will use defaule defiend in globals
            lCmdText = "exec  [Configuration].[GetSingleConfigurationValue] @path = 'Preference.Applications.FirmCustom.CRUploader' ,@name = 'CRValidationProcedure'"
            SQLCtl.ExecQuery(lCmdText, gSQLConnection)
            If SQLCtl.sqlds.Tables(0).Rows.Count <> 0 Then 'Resource not defined - do not overrid
                gCRValidationProc = SQLCtl.sqlds.Tables(0).Rows(0)("Content")
            End If

            lCmdText = "Select Mapcode, MapDesc + ' (' + mapcode + ')' as MapDesc, FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'"
            SQLCtl.ExecQuery(lCmdText, gSQLConnection)
            gdtMaps = SQLCtl.sqlds.Tables(0)
            With cboMap
                .DataSource = gdtMaps 'SQLCtl.sqlds.Tables(0)
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
        Catch ex As Exception
            MsgBox("Unhandled error:" & ex.Message & vbCrLf, vbOK, "Form Load")
        End Try
        If cboMap.SelectedValue <> "" Then cboMap_SelectedIndexChanged(sender, e)
        'If txtCRFile.Text <> "" Then txtCRFile_TextChanged(sender, e)
    End Sub



    Private Sub cboMap_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMap.SelectedIndexChanged
        '================================================================================================================
        '                           MAP SELECTED    MAP SELECTED    MAP SELECTED    MAP SELECTED    MAP SELECTED        '
        '================================================================================================================
        ' cboMap has all defined mappings.  When selected, load the details for that mapping to the gdtMap Data table
        'CurrSelectedMap = cboMap.SelectedValue.ToString

        If Not FormLoad And cboMap.SelectedValue.ToString <> "" Then
            Try
                CurrSelectedMap = cboMap.SelectedValue.ToString
                Dim lCmdText As String = ""
                'Ensure SQL sequenece exists
                lCmdText = "if not exists (select * from sys.sequences where schema_name(schema_id) = 'sequence' and name = 'import_num') " &
                        "CREATE SEQUENCE [Sequence].[Import_Num]  START WITH 1"
                SQLCtl.ExecQuery(lCmdText, gSQLConnection)

                lCmdText = "Select Mapcode, ApplicationType, SourceColumnLabel, TargetColumn, DataType 
                            from _aac_CRMAP where isnull(applicationTYpe,'') <> '' and mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;
                           SELECT MapCode ,MapDesc ,FileMask ,FileType ,Inactive ,ReceiptID ,TargetTable ,XLSheetName ,isnull(FirstDataRow,1) 'FirstDataRow',SchemaName 
                            FROM dbo._aac_CRMAPZ where mapcode = '" & CurrSelectedMap & "';"
                Dim ldsMap = New DataSet
                SQLCtl.ExecQuery(lCmdText, gSQLConnection)
                ldsMap = SQLCtl.sqlds
                gdtMap = ldsMap.Tables(0)
                ' dbMap.DataSource = gdtMap

                dbMapz.DataSource = ldsMap.Tables(1)
                ReceiptKeyColumn = ldsMap.Tables(1).Rows(0)("ReceiptID") 'Get identifier for receipt object
                TargetTableName = ldsMap.Tables(1).Rows(0)("TargetTable") 'Get Name of target table
                SourceFileType = ldsMap.Tables(1).Rows(0)("FileType") 'Get file type of source table
                SourceFileOpenMask = ldsMap.Tables(1).Rows(0)("FileMask") 'Get file mask for open dialog
                SourceXLSheetName = ldsMap.Tables(1).Rows(0)("XLSheetName").ToString 'Get Sheet Name
                FirstDataRow = ldsMap.Tables(1).Rows(0)("FirstDataRow") 'Get Sheet Name
                UseSchemaName = ldsMap.Tables(1).Rows(0)("SchemaName").ToString 'Get Sheet Name
                OpenFileDialog1.Filter = "CR Import Files|" & SourceFileOpenMask 'Get identifier for receipt object

                'If ldsMap.Tables(2).Rows.Count > 0 Then
                '    gImportNum = ldsMap.Tables(2).Rows(0)(0)
                'End If

                btnOpen.Enabled = True
                txtCRFile.Enabled = True

            Catch ex As Exception
                Dim mresp As MsgBoxResult
                mresp = MsgBox("Unexpected Error:" & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Error in cboMap.SelectedIndexChanged")
                WriteLog("Error in cboMap_SelectedIndexChanged" & ex.Message & ex.StackTrace)
                If mresp = MsgBoxResult.Cancel Then
                    ProgramAbend()
                End If
            Finally
                btnEditMap.Enabled = (cboMap.SelectedValue <> "" And txtCRFile.Text <> "")
                'txtCRFile.Enabled = True
            End Try


        End If
    End Sub

    Private Sub txtCRFile_TextChanged(sender As Object, e As EventArgs) Handles txtCRFile.TextChanged
        '================================================================================================================
        '             FILE SELECTED     FILE SELECTED     FILE SELECTED     FILE SELECTED     FILE SELECTED             '
        '==============================================================================================================='
        '                           FILL gdsSelectedCRSourceFile FROM SOURCE (XLS ot CSV)                               '
        '================================================================================================================
        btnUpload.Enabled = False
        Dim CnStr As String '= ""
        If FormLoad <> True And txtCRFile.Text <> "" Then
            SourceFileName = txtCRFile.Text
            Dim fileobject As FileInfo
            fileobject = New FileInfo(SourceFileName)

            If fileobject.Exists Then
                ' If ACE driver does not exist, use JET driver 
                'If Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Microsoft.ACE.OLEDB.16.0\CLSID") Is Nothing Then
                'CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(txtCRFile.Text) &
                '                   ";Extended Properties=""text;HDR=Yes;FMT=Delimited(|)"";"
                CnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""" & Path.GetDirectoryName(SourceFileName) &
                    """;Extended Properties='text;HDR=Yes;FMT=Delimited;';"

                '           Else
                'CnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Path.GetDirectoryName(txtCRFile.Text) &
                '              ";Extended Properties=""text;HDR=Yes;FMT=Delimited(|)"";"
                '               End If
                Debug.Print(CnStr)
                gdsSelectedCRSourceFile = New DataSet

                If SourceFileType = "CSV" Then
                    If UseSchemaName <> "" Then 'Copy file to schemna name and se that as input
                        If (My.Computer.FileSystem.FileExists(Path.GetDirectoryName(txtCRFile.Text) & "\" & UseSchemaName)) Then
                            My.Computer.FileSystem.DeleteFile(Path.GetDirectoryName(txtCRFile.Text) & "\" & UseSchemaName)
                        End If

                        My.Computer.FileSystem.CopyFile(SourceFileName, Path.GetDirectoryName(SourceFileName) & "\" & UseSchemaName)
                        OrigFileName = SourceFileName
                        SourceFileName = Path.GetDirectoryName(SourceFileName) & "\" & UseSchemaName


                    End If



                    Try
                        Dim CSVSelect As String = "select * from [" & Path.GetFileName(SourceFileName) & "]"
                        Using Adp As New OleDbDataAdapter(CSVSelect, CnStr)
                            Adp.Fill(gdsSelectedCRSourceFile)
                        End Using

                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Critical, "Error in CRFile.TextChanged event.")
                    End Try
                End If
                If SourceFileType = "XLS" Then
                    Try
                        Dim CSVSelect As String = "select * from [" & SourceXLSheetName & "]"
                        Using Adp As New OleDbDataAdapter(CSVSelect, CnStr)
                            Adp.Fill(gdsSelectedCRSourceFile)
                        End Using

                    Catch ex As Exception
                        MsgBox("Verify range " + SourceXLSheetName + " exists In file " + txtCRFile.Text + vbCrLf + ex.Message, MsgBoxStyle.Critical, "CRFile.TextChanged Event. ")
                    End Try
                End If
                gvData.DataSource = gdsSelectedCRSourceFile.Tables(0) 'Bind preview grid to imput data table
                btnUpload.Enabled = True
            Else
                MsgBox("Input File Not Found", MsgBoxStyle.Information, "CRFile.TextChanged Event. ")
            End If


            gSourceDataPath = Path.GetDirectoryName(SourceFileName)
            If gswLogFile IsNot Nothing Then
                gswLogFile.Close()
            End If
            ' Log File name should carry the original inptu file name regardles sof if a schema is being used.
            gImportLogName = Path.GetDirectoryName(SourceFileName) & "\" & Path.GetFileNameWithoutExtension(txtCRFile.Text) & ".log"
            gswLogFile = New StreamWriter(gImportLogName, True)
        End If

        btnEditMap.Enabled = (cboMap.SelectedValue <> "" And SourceFileName <> "")

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
        txtPreamble &= "_ImportStatus)" & vbCrLf
        Dim CMSplit() As String
        txtValues = "Values ("
        For colix = 0 To pMapTable.Rows.Count - 1
            Select Case pMapTable.Rows(colix)("DataType")
                Case "K" ' C(K)onstant
                    txtValues &= "'" & (pMapTable.Rows(colix)("SourceColumnLabel")) & "', "
                Case "R" 'Replacement Variable
                    Select Case pMapTable.Rows(colix)("SourceColumnLabel").ToString.ToUpper
                        Case "%FileName%".ToUpper
                            txtValues &= "'" & txtCRFile.Text & "', "
                        Case "%ImportNum%".ToUpper
                            txtValues &= gImportNum.ToString & ", "
                        Case "%Time%".ToUpper
                            txtValues &= "getdate(), "
                        Case "%User%".ToUpper
                            txtValues &= "Suser_Sname(), "
                        Case "%TranLine%".ToUpper
                            txtValues &= gTranLineNumber.ToString + ", "
                        Case "%TranNum%".ToUpper
                            txtValues &= gTranNumber.ToString + ", "

                        Case Else
                            txtValues &= "'" & pMapTable.Rows(colix)("SourceColumnLabel") & "', "
                    End Select
                Case "L" ' get value based on column heading label
                    txtValues &= "'" & pDataRow(pMapTable.Rows(colix)("SourceColumnLabel")) & "', "
                Case "C" ' get value based on column heading label
                    If Not IsDBNull(pDataRow(pMapTable.Rows(colix)("SourceColumnLabel"))) Then
                        CMSplit = pDataRow(pMapTable.Rows(colix)("SourceColumnLabel")).split(".")
                        txtValues &= "'" & CMSplit(0) & "', "
                    Else
                        txtValues &= "'', "
                    End If

                Case "M" ' get value based on column heading label
                    If Not IsDBNull(pDataRow(pMapTable.Rows(colix)("SourceColumnLabel"))) Then
                        CMSplit = pDataRow(pMapTable.Rows(colix)("SourceColumnLabel")).split(".")
                        txtValues &= "'" & CMSplit(1) & "', "
                    Else
                        txtValues &= "'', "
                    End If

                Case Else
                    txtValues &= "'InvalidMap', "
            End Select

        Next colix
        txtValues &= "'N')"  ' Trailing 'N' goes into Import Status column
        Debug.Print(txtPreamble & txtValues)
        Dim UpdateCommand As String
        UpdateCommand = txtPreamble & txtValues
        SQLCtl.ExecCmd(UpdateCommand, gSQLConnection)

    End Sub

    Function GetDistinctRows(pType As String, pDataSource As DataTable, Optional pReceiptID As String = "") As DataTable
        '================================================================================================================
        '                           GET SOURCE COLUMNS MAPPED FOR THIS TYPE ('L's)    
        '                           RETURN DataTable of all rows from inupt for indicated pType, i.e. Headers, Details...
        '================================================================================================================
        ' ReceiptKeyColumn is global variable holding the column name from the source data that groups all details for a given receipt

        Try
            ' For each type, get distinct values for mapped columns                                                         TWO
            Dim ThisType As String = pType
            Dim RTypeColumnFilter As String
            Dim ColumnList As New List(Of String)


            ' Get distinct values of all mapped columns for thistype 
            RTypeColumnFilter = "ApplicationType = '" & ThisType & "' and DataType in ('L','C','M')"
            If chkLogDebug.Checked Then WriteLog("GetDistinctRows (SourceColumns) for " & RTypeColumnFilter)
            Dim SourceColumnsThisType As DataRow() = gdtMap.Select(RTypeColumnFilter)
            Dim dtThisType As DataTable = gdtMap.Clone()
            dtThisType = SourceColumnsThisType.CopyToDataTable
            DisplayDT(dtThisType, 20)
            'Get list of colums from source table mapped for indicated type.
            ' Data Type = 'L' indicates data that is to be retrieved from source data file
            ColumnList = New List(Of String)
            For ColIX = 0 To dtThisType.Rows.Count - 1
                Debug.Print(dtThisType.Rows(ColIX)("SourceColumnLabel"))
                ColumnList.Add(dtThisType.Rows(ColIX)("SourceColumnLabel"))
            Next ColIX
            ColumnList = ColumnList.Distinct().ToList '  Remove duplicates from column list (in case a source column is mapped to multiple target columns.)


            '-- dtInputRowsTHisType is distinct subset of indicated columns (True means distinct)
            Dim dtInputRowsThisType As DataTable = pDataSource.DefaultView.ToTable(True, ColumnList.ToArray())

            If pReceiptID <> "" Then 'Special test to only process indicated receiptjey = receiptID (for debugging)
                RTypeColumnFilter = "[" & ReceiptKeyColumn & "] = '" & pReceiptID & "'"
            Else                     ' Filter to only rows that have receiptkey data
                RTypeColumnFilter = "[" & ReceiptKeyColumn & "] <> ''"
            End If
            If chkLogDebug.Checked Then WriteLog("GetDistinctRows (DataColumns) for " & RTypeColumnFilter)
            GetDistinctRows = dtInputRowsThisType.Select(RTypeColumnFilter).CopyToDataTable
        Catch ex As Exception
            Dim mresponse As MsgBoxResult
            mresponse = MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error in GetDistinctRows")
            WriteLog("Error in GetDistinctRows" & ex.Message & ex.StackTrace)
            GetDistinctRows = pDataSource.Clone 'Return empty copy
        End Try
        If chkLogDebug.Checked Then DisplayDT(GetDistinctRows, 10)
    End Function

    Function GetTypeMap(pType As String) As DataTable
        '================================================================================================================
        '                           GET ALL MAPPINGS THIS TYPE                                                          '
        '================================================================================================================
        Dim TypeFilter As String
        Dim SourceRowsMatchingType As DataRow()
        TypeFilter = "ApplicationType = '" & pType & "' and DataType <> ''"
        If chkLogDebug.Checked Then WriteLog("GetTypeMap for " & TypeFilter)
        SourceRowsMatchingType = gdtMap.Select(TypeFilter)
        GetTypeMap = SourceRowsMatchingType.CopyToDataTable
        If chkLogDebug.Checked Then DisplayDT(GetTypeMap, 10)  '--??
    End Function

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

        Try
            ActivityText.Text = "Start"
            WriteLog("Start Upload Of " & txtCRFile.Text)

            ' Dim lcmd As New SqlCommand
            ' lcmd.Connection = gSQLConnection

            ' // Test if file has already been processed by checking if it is in the load file //
            Dim lCmdText As String = "Select count(*) As 'Count', max(_StagingDate) 'StagingDate' from " + TargetTableName + " where _SourceFile = '" & txtCRFile.Text & "'"
            ''Dim ldtHist As DataTable  '' was new
            SQLCtl.ExecQuery(lCmdText, gSQLConnection)
            ''ldtHist = SQLCtl.sqlds.Tables(0)
            ''ldaHist.Fill(ldtHist)
            If SQLCtl.sqlds.Tables(0).Rows(0)("Count") <> 0 Then
                Dim mresp As MsgBoxResult
                mresp = MsgBox("File " & Path.GetFileName(txtCRFile.Text) & " has already been processed." + vbCrLf +
                               "Do you want to re-upload it?" & vbCrLf +
                               "Last upload was on " & SQLCtl.sqlds.Tables(0).Rows(0)("StagingDate").ToString, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Duplicate Upload Name")
                If mresp <> MsgBoxResult.Yes Then
                    Exit Sub '  Do not want to re-process - get out of this routine
                End If
            End If


            'Open source data table                                                                                      dtCRInputDataFile
            Dim dtCRInputDataFile As New DataTable ' This is the full contents
            dtCRInputDataFile = gdsSelectedCRSourceFile.Tables(0).Copy
            If chkLogDebug.Checked Then DisplayDT(dtCRInputDataFile, 10)


            ' GET HEADER DATA and HEADER MAPPING
            Dim ThisType As String = "R"
            Dim dtHeaderData As DataTable = GetDistinctRows(ThisType, dtCRInputDataFile)
            Dim dtHeaderMapping As DataTable = GetTypeMap(ThisType)

            ' For each type, get distinct values for mapped columns and get mapping                                         dtInputRowsThisType  I (Bills)

            ' GET TYPE I DATA and TYPE I MAPPING
            ThisType = "I"
            Dim dtType_I_Data As DataTable
            dtType_I_Data = GetDistinctRows(ThisType, dtCRInputDataFile)
            Dim dtType_I_Map As DataTable = GetTypeMap(ThisType)


            SQLCtl.ExecQuery("Select next value for sequence.import_num as Import_Num", gSQLConnection)
            gImportNum = SQLCtl.sqlds.Tables(0).Rows(0)(0)

            ' For Each distinct R row in dtHeaderData:
            '   Write the R row
            '   Write the I rows matching the receipt header (as indicated by the columnname  in ReceiptKeyColumn)
            '   ADD CODE to also write additional mapping types, e.g. C, M, T, G...

            ' then loop through for each subset matching the ReceiptKeyColumn                                               Process all R Receipts
            Dim ThisTypeThisReceipt As DataTable
            Dim RcptHeaderRow As DataRow
            Dim TypeFilter As String = ""
            gTranNumber = 0
            For Each RcptHeaderRow In dtHeaderData.Rows   '                                                      Receipt Header Loop
                gTranNumber += 1                'Receipt Sequence within file
                gTranLineNumber = 1             'Detail number within receipt
                ' RcptHeaderRow is the current receipt header data
                WriteLog("Start Processing: " & RcptHeaderRow(ReceiptKeyColumn).ToString)

                'Write HeaderRow to SQL
                WriteDataViaMap(RcptHeaderRow, dtHeaderMapping)

                '' NOT USED?  TypeFilter = "[" & ReceiptKeyColumn & "] = '" & RcptHeaderRow(ReceiptKeyColumn).ToString & "'"
                '' NOT USED?  Dim I_RowsThisReceipt As DataRow() = dtType_I_Data.Select(TypeFilter)
                '' NOT USED?  'ThisTypeThisReceipt = I_RowsThisReceipt.CopyToDataTable

                'Filter I rows and write to SQL
                ThisType = "I"
                ThisTypeThisReceipt = GetDistinctRows(ThisType, dtCRInputDataFile, RcptHeaderRow(ReceiptKeyColumn).ToString)
                Dim ItemDatarow As DataRow
                For Each ItemDatarow In ThisTypeThisReceipt.Rows
                    gTranLineNumber += 1
                    WriteDataViaMap(ItemDatarow, dtType_I_Map)
                Next ' I Detail Item

            Next ' R Receipt Header

            ' ------------------------------------------------------------
            ' UPLOAD COMPLETE 
            ' ------------------------------------------------------------
            MsgBox("Upload Complete:" & vbCrLf & OpenFileDialog1.FileName, MsgBoxStyle.OkOnly, "Complete")
            StatusText.Text = "Uploaded to Batch " & gImportNum.ToString
            ActivityText.Text = "Done"
            ' ------------------------------------------------------------
            ' IF VALIDATE IS CHECKED, CALL THE VALIDATION STORED PROCEDURE
            ' ------------------------------------------------------------
            If chkValidate.Checked Then
                WriteLog("Batch:" & gImportNum.ToString & "|Begin Validation" & Now().ToString)
                Dim cdtError As New DataTable()
                ' Validation Routine
                SQLCtl.ExecCmd("Exec " & gCRValidationProc & " @BatchId=" & gImportNum.ToString, gSQLConnection)
                ' Results of Validation
                SQLCtl.ExecQuery("select Severity, TrackingNumber, errorvalue from [_AacCRImpErrorLog] where BatchId=" & gImportNum.ToString & " Order by trackingnumber", gSQLConnection)
                cdtError = SQLCtl.sqlds.Tables(0)
                Dim lRow As DataRow
                For Each lRow In cdtError.Rows
                    WriteLog("Batch:" & gImportNum.ToString & "|TrackingNumber:" & lRow(0) & "|" & lRow(1) & "|" & lRow(2))
                Next
                cdtError.Dispose()

                ' Results with severity > 0 (Errors)
                cdtError = New DataTable()
                SQLCtl.ExecQuery("select isnull(TrackingNumber,-1), Severity, errorvalue from [_AacCRImpErrorLog] where BatchId=" & gImportNum.ToString & " and severity <> 0 Order by trackingnumber", gSQLConnection)
                cdtError = SQLCtl.sqlds.Tables(0)
                If cdtError.Rows.Count <> 0 Then  ' Errors will prevent upload
                    Dim ErrLogName As String = Path.GetDirectoryName(SourceFileName) & "\" & Path.GetFileNameWithoutExtension(SourceFileName) & ".err"
                    Dim gswLogFileErr As New StreamWriter(ErrLogName, True)
                    gswLogFile.AutoFlush = True
                    gswLogFileErr.WriteLine("TimeStamp|BatchID|TrackingNum|Severity|Error")
                    For Each lRow In cdtError.Rows
                        gswLogFileErr.WriteLine(Now.ToString & "|" & "Batch:" & gImportNum.ToString & "|TrackingNumber:" & lRow(0) & "|" & lRow(1) & "|" & lRow(2))
                    Next
                    gswLogFileErr.Flush()

                    MsgBox("Could not import due to validation errors. Please review log:" & vbCrLf & ErrLogName, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Validation Errors")

                Else ' No Errors > severity 0 found
                    '-------------------------------------------------------
                    ' IF NO ERRORS, CALL UPLOAD ROUTINE TO CREATE CR SESSION
                    '-------------------------------------------------------
                    SQLCtl.ExecCmd("Exec _spaac_ImportCR @NoCommit = 0, @BatchId=" & gImportNum.ToString, gSQLConnection)
                    cdtError.Dispose()
                    cdtError = New DataTable()
                    ' Get session created to report back 
                    SQLCtl.ExecQuery("select max(_Session) from " & TargetTableName & " where BatchId=" & gImportNum.ToString, gSQLConnection)
                    cdtError = SQLCtl.sqlds.Tables(0)
                    StatusText.Text &= "; Created Session " & cdtError.Rows(0)(0).ToString
                    ' If sucessful upload, archive the import file
                    Dim fso As New FileInfo(txtCRFile.Text)
                    Dim ArchPath As String = Path.GetDirectoryName(txtCRFile.Text) & "\archive\"
                    If Not Directory.Exists(ArchPath) Then
                        Directory.CreateDirectory(ArchPath)
                    End If
                    ArchPath = ArchPath & Path.GetFileName(txtCRFile.Text)
                    fso.CopyTo(ArchPath)
                    fso.Delete()




                End If
                    cdtError.Dispose()
            End If
        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Unhandled Exception In btnUpload")
        End Try
    End Sub

    Private Sub btnEditMap_Click(sender As Object, e As EventArgs) Handles btnEditMap.Click
        If (cboMap.SelectedValue <> "" And txtCRFile.Text <> "") Then
            frmEditMap.ShowDialog(Me)

            FormLoad = True
            SQLCtl.ExecQuery("Select Mapcode, MapDesc + ' (' + MAPCODE + ')' AS 'MapDesc', FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'", gSQLConnection)
            gdtMaps = SQLCtl.sqlds.Tables(0)
            With cboMap
                .DataSource = gdtMaps
                .DisplayMember = "MapDesc"
                .ValueMember = "MapCode"
                .SelectedValue = CurrSelectedMap
                '.Refresh()
            End With
            FormLoad = False
            cboMap_SelectedIndexChanged(sender, e)
        Else
            MsgBox("Must select mapping and source file.", vbOKOnly, "Missing Information'")
        End If

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
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName <> "" Then
            txtCRFile.Text = OpenFileDialog1.FileName
        End If
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

        'ActivityText.Text = pString
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

    Private Sub cboMap_TextChanged(sender As Object, e As EventArgs) Handles cboMap.TextChanged
        If cboMap.Text.ToLower = "initialize" Then
            frmEditMap.ShowDialog(Me)
            Return

        End If
    End Sub
End Class
