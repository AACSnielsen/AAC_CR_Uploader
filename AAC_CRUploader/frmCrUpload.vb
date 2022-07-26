Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Xml


Public Class UploadCRFile
    Protected Friend FormLoad As Boolean
    Protected Friend ReceiptKeyColumn As String
    Protected Friend gdtMap As DataTable
    Protected Friend gImportNum As Integer = -1
    Protected Friend gAutoRun As Boolean = False
    Protected Friend gAutoFile As String = ""
    Protected Friend gAutoMap As String = ""
    Protected Friend gSQLConnection As SqlConnection
    Protected Friend gdtReceipts As DataTable
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
        Dim lCmdText As String = "Select Mapcode, MapDesc, FileMask, TargetTable, FileType from _aac_CRMAPZ where inactive <> 'Y'"

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
        cboMap_SelectedIndexChanged(sender, e)
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
                'TODO REMAP TO CUSTOM TABLE FOR NEXT BATCH NUMBER  !!!!
                lCmdText = "Select Mapcode, ApplicationType, SourceColumnLabel, TargetColumn, DataType from _aac_CRMAP where mapcode = '" & cboMap.SelectedValue & "' order by applicationType DESC;" &
                       "Select MapCode, MapDesc, FileMask, TargetTable, FileType, ReceiptID from _aac_crmapz where mapcode = '" & cboMap.SelectedValue & "';" &
                       "Select next value for sequence.import_num as Import_Num from _aac_crmap where mapcode = '" & cboMap.SelectedValue & "' and sourcecolumnlabel = '%ImportNum%';"
                Dim ldaMap As New SqlDataAdapter(lCmdText, gSQLConnection)
                Dim ldsMap = New DataSet
                ldaMap.Fill(ldsMap)

                gdtMap = ldsMap.Tables(0)
                dbMap.DataSource = gdtMap

                dbMapz.DataSource = ldsMap.Tables(1)
                ReceiptKeyColumn = ldsMap.Tables(1).Rows(0)("ReceiptID") 'Get identifier for receipt object
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

        Dim lcmd As New SqlCommand
        lcmd.Connection = gSQLConnection

        ' // Test if file has already been processed by checking if it is in the load file //
        Dim lCmdText As String = "Select count(*) As 'Count', max(_StagingDate) 'StagingDate' from _aac_CR_load where _SourceFile = '" & OpenFileDialog1.FileName & "'"
        Dim ldaHist As New SqlDataAdapter(lCmdText, gSQLConnection)
        Dim ldtHist As New DataTable
        ldaHist.Fill(ldtHist)
        If ldtHist.Rows(0)("Count") <> 0 Then
            Dim mresp As MsgBoxResult
            mresp = MsgBox("File " & Path.GetFileName(OpenFileDialog1.FileName) & " has already been processed." + vbCrLf + "Do you want to re-upload it?" & vbCrLf + "Last upload was on " & ldtHist.Rows(0)("StagingDate").ToString, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Duplicate Upload Name")
            If mresp <> MsgBoxResult.Yes Then
                Exit Sub '  Do not want to re-process - get out of this routine
            End If
        End If


        'Open source data table                                                                                      dtCRData
        Dim dtCRData As New DataTable ' This is the full contents
        Dim strCRSelect As String = "select * from [" & Path.GetFileName(OpenFileDialog1.FileName) & "]"
        Dim csvConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(OpenFileDialog1.FileName) & ";Extended Properties=""text;HDR=Yes;FMT=Delimited"";"
        Using csvDataAdapter As New OleDbDataAdapter(strCRSelect, csvConnection)
            csvDataAdapter.Fill(dtCRData)
        End Using

        ' For each type, get distinct values for mapped columns                                                         distinctRcptHeaders.
        Dim distinctRcptHeaders As DataTable = GetDistinctRows("R", dtCRData)
        'distinctRcptHeaders = GetDistinctRows("R", dtCRData)
        Dim dtRcptHeaderMap As DataTable = GetTypeMap("R")

        ' For each type, get distinct values for mapped columns and get mapping                                                         distinctRcptRowsThisType  I (Bills)
        Dim ThisType As String = "I"
        Dim distinctRcptRowsThisType As DataTable
        distinctRcptRowsThisType = GetDistinctRows(ThisType, dtCRData)
        Dim dtThistypeMap As DataTable = GetTypeMap(ThisType)
        'DisplayDT(dtThistypeMap, 100)
        '' Get distinct values of all mapped columns for thistype 

        'Dim SourceColumnsThisType As DataRow() '= gdtMap.Select(RTypeColumnFilter)
        'Dim dtThisType As DataTable = gdtMap.Clone()

        ' then loop through for each subset matching the ReceiptKeyColumn                                               Process all R Receipts
        Dim ThisTypeThisReceipt As DataTable
        Dim distinctRcptHeader As DataRow
        Dim TypeFilter As String = ""
        For Each distinctRcptHeader In distinctRcptHeaders.Rows   '                                                      Receipt Header Loop
            'Write R row followed by all details rows mapped
            '  distinctRcptHeader is the current receipt header data

            Debug.Print("Start Processing:" & distinctRcptHeader(ReceiptKeyColumn).ToString)
            'Write HeaderRow to SQL
            WriteDataViaMap(distinctRcptHeader, dtRcptHeaderMap)

            'Filter I rows and write to SQL
            TypeFilter = "[" & ReceiptKeyColumn & "] = '" & distinctRcptHeader(ReceiptKeyColumn).ToString & "'"
            Dim SourceRowsMatchingType As DataRow() = distinctRcptRowsThisType.Select(TypeFilter)
            'ThisTypeThisReceipt = SourceRowsMatchingType.CopyToDataTable
            ThisTypeThisReceipt = GetDistinctRows("I", dtCRData, distinctRcptHeader(ReceiptKeyColumn).ToString)
            'DisplayDT(ThisTypeThisReceipt, 100) ' DEBUG   DEBUG   DEBUG   DEBUG   DEBUG
            Dim ItemDatarow As DataRow
            For Each ItemDatarow In ThisTypeThisReceipt.Rows
                WriteDataViaMap(ItemDatarow, dtThistypeMap)
            Next

            'ThisTypeThisReceipt is the set of rows for ThisType designated for this receipt key

            ' Process the map for this type against the ThisTypeThisReceipt datatable

        Next



        ' OLD CODE BELOW
        Dim dtThisType As DataTable
        Dim ColumnList As New List(Of String)
        Dim distinctTypes As DataTable = gdtMap.DefaultView.ToTable(True, "ApplicationType") 'TRUE implies distinct here
        '''' distinctTypes e.g. "R" and "B"
        For TypeIX = 0 To distinctTypes.Rows.Count - 1
            ''    Debug.Print("Processing Type = " & distinctTypes.Rows(TypeIX)(0).ToString)
            ''    ' ??If type = R build table of distinct receipts,  if not R, retrieve distinct receipt num


            ''    'Get distinct mapping rows for select map and type
            ''    ''Dim TypeFilter As String = "ApplicationType = '" & distinctTypes.Rows(TypeIX)(0).ToString & "' and DataType = 'L'"
            Dim SourceRowsMatchingType As DataRow() = gdtMap.Select(TypeFilter)
            ''    '' ''Dim dtThisType As DataTable = gdtMap.Clone()
            ''    dtThisType = SourceRowsMatchingType.CopyToDataTable
            ''    ' At this point dtThisType should have the mapping for the iterative data type (R, B, ...)
            ''    ' Process each row in the dtThistype mapping against all (distinct) input rows to write to database table
            ''    '  First set column list to select:
            ''    ColumnList = New List(Of String)
            ''    For ColIX = 0 To dtThisType.Rows.Count - 1
            ''        ColumnList.Add(dtThisType.Rows(ColIX)("SourceColumnLabel"))
            ''    Next ColIX

            ''    'Update the map to include all columns
            ''    TypeFilter = "ApplicationType = '" & distinctTypes.Rows(TypeIX)(0).ToString & "' and DataType <> ''"
            ''    SourceRowsMatchingType = gdtMap.Select(TypeFilter)
            dtThisType = SourceRowsMatchingType.CopyToDataTable


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
                If CurrReceiptID <> distinctRcpt.Rows(rcptix)(ReceiptKeyColumn) Then
                    ReceiptLineSeq = 0
                    CurrReceiptID = distinctRcpt.Rows(rcptix)(ReceiptKeyColumn)
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
    Sub WriteDataViaMap(pDataRow As DataRow, pMapTable As DataTable)
        Dim txtPreamble As String = "insert into _aac_CR_LOAD ("
        Dim txtValues As String = ""
        For ColIX = 0 To pMapTable.Rows.Count - 1
            txtPreamble &= (pMapTable.Rows(ColIX)("TargetColumn")) + ", "
        Next ColIX
        txtPreamble = txtPreamble.Substring(0, txtPreamble.Length - 2) & ")" & vbCrLf
        txtValues = "Values ("
        For colix = 0 To pMapTable.Rows.Count - 1
            Select Case pMapTable.Rows(colix)("DataType")
                Case "K" ' C(K)onstant
                    txtValues &= "'" & (pMapTable.Rows(colix)("SourceColumnLabel")) & "', "
                Case "R" 'Replacement Variable
                    Select Case (pMapTable.Rows(colix)("SourceColumnLabel"))
                        Case "%FileName%"
                            txtValues &= "'" & OpenFileDialog1.FileName & "', "
                        Case "%ImportNum%"
                            txtValues &= gImportNum.ToString & ", "
                        Case "%Time%"
                            txtValues &= "getdate(), "
                        Case "%User%"
                            txtValues &= "Suser_Sname(), "
                        Case "%LineNum%"
                            txtValues &= "0, " 'ReceiptLineSeq.ToString + ", "
                        Case "%TranLine%"
                            'txtValues &= ReceiptLineSeq.ToString + ", "
                            txtValues &= 0.ToString + ", "
                        Case Else
                            txtValues &= "'" & pMapTable.Rows(colix)("SourceColumnLabel") & "', "
                    End Select
                Case Else
                    txtValues &= "'" & pDataRow(pMapTable.Rows(colix)("SourceColumnLabel")) & "', "
            End Select

        Next colix
        txtValues = txtValues.Substring(0, txtValues.Length - 2) & ")"
        Debug.Print(txtPreamble & txtValues)
    End Sub

    Function GetDistinctRows(pType As String, pDataSource As DataTable, Optional pReceiptID As String = "") As DataTable
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
        ' DisplayDT(distinctRcptRowsThisType, 100) ' DEBUG   DEBUG   DEBUG   DEBUG   DEBUG
        If pReceiptID <> "" Then
            RTypeColumnFilter = "[" & ReceiptKeyColumn & "] = '" & pReceiptID & "'"
            GetDistinctRows = distinctRcptRowsThisType.Select(RTypeColumnFilter).CopyToDataTable
        Else
            GetDistinctRows = distinctRcptRowsThisType
        End If
    End Function

    Function GetTypeMap(pType As String) As DataTable
        'Update the map to include all columns
        Dim TypeFilter As String
        Dim SourceRowsMatchingType As DataRow()
        TypeFilter = "ApplicationType = '" & pType & "' and DataType <> ''"
        SourceRowsMatchingType = gdtMap.Select(TypeFilter)
        GetTypeMap = SourceRowsMatchingType.CopyToDataTable
    End Function



    Sub DisplayDT(Pdt As DataTable, pMaxRows As Integer)
        If pMaxRows > Pdt.Rows.Count Then
            pMaxRows = Pdt.Rows.Count
        End If

        Dim DebugLine As String = ""
        For c = 0 To Pdt.Columns.Count - 1
            DebugLine &= Pdt.Columns(c).ColumnName & "|"
        Next c
        Debug.Print(DebugLine)
        DebugLine = ""
        For r = 0 To pMaxRows - 1
            For c = 0 To Pdt.Columns.Count - 1
                DebugLine &= Pdt.Rows(r)(c) & "|"
            Next c
            Debug.Print(DebugLine)
            DebugLine = ""
        Next r
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
