<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditMap
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dbMap = New System.Windows.Forms.DataGridView()
        Me.cboMap = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbAvailableFields = New System.Windows.Forms.ListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblSampleFields = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtFileMask = New System.Windows.Forms.TextBox()
        Me.txtReceiptID = New System.Windows.Forms.TextBox()
        Me.txtTargetTable = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lbTargetColumns = New System.Windows.Forms.ListBox()
        Me.btnAddPair = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtNewCode = New System.Windows.Forms.TextBox()
        Me.txtNewDesc = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cboFileType = New System.Windows.Forms.ComboBox()
        Me.CBOSheetName = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.bntValidate = New System.Windows.Forms.Button()
        Me.btnExportmap = New System.Windows.Forms.Button()
        Me.btnImportMap = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.dbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dbMap
        '
        Me.dbMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dbMap.Location = New System.Drawing.Point(12, 172)
        Me.dbMap.MultiSelect = False
        Me.dbMap.Name = "dbMap"
        Me.dbMap.RowHeadersVisible = False
        Me.dbMap.Size = New System.Drawing.Size(477, 426)
        Me.dbMap.TabIndex = 0
        '
        'cboMap
        '
        Me.cboMap.FormattingEnabled = True
        Me.cboMap.Location = New System.Drawing.Point(120, 18)
        Me.cboMap.Name = "cboMap"
        Me.cboMap.Size = New System.Drawing.Size(368, 21)
        Me.cboMap.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "File Layout Template"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(333, 604)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(62, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "File Mask"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Unique Receipt Key"
        '
        'lbAvailableFields
        '
        Me.lbAvailableFields.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbAvailableFields.FormattingEnabled = True
        Me.lbAvailableFields.Location = New System.Drawing.Point(646, 172)
        Me.lbAvailableFields.Name = "lbAvailableFields"
        Me.lbAvailableFields.Size = New System.Drawing.Size(127, 420)
        Me.lbAvailableFields.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 156)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Field Map"
        '
        'lblSampleFields
        '
        Me.lblSampleFields.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSampleFields.AutoSize = True
        Me.lblSampleFields.Location = New System.Drawing.Point(644, 156)
        Me.lblSampleFields.Name = "lblSampleFields"
        Me.lblSampleFields.Size = New System.Drawing.Size(99, 13)
        Me.lblSampleFields.TabIndex = 10
        Me.lblSampleFields.Text = "Fields inSample File"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(414, 604)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txtFileMask
        '
        Me.txtFileMask.Location = New System.Drawing.Point(120, 39)
        Me.txtFileMask.Name = "txtFileMask"
        Me.txtFileMask.Size = New System.Drawing.Size(368, 20)
        Me.txtFileMask.TabIndex = 12
        '
        'txtReceiptID
        '
        Me.txtReceiptID.Location = New System.Drawing.Point(120, 101)
        Me.txtReceiptID.Name = "txtReceiptID"
        Me.txtReceiptID.Size = New System.Drawing.Size(368, 20)
        Me.txtReceiptID.TabIndex = 13
        '
        'txtTargetTable
        '
        Me.txtTargetTable.Location = New System.Drawing.Point(120, 121)
        Me.txtTargetTable.Name = "txtTargetTable"
        Me.txtTargetTable.Size = New System.Drawing.Size(368, 20)
        Me.txtTargetTable.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(46, 125)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Target Table"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(502, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Columns in Target"
        '
        'lbTargetColumns
        '
        Me.lbTargetColumns.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbTargetColumns.FormattingEnabled = True
        Me.lbTargetColumns.Location = New System.Drawing.Point(504, 172)
        Me.lbTargetColumns.Name = "lbTargetColumns"
        Me.lbTargetColumns.Size = New System.Drawing.Size(127, 420)
        Me.lbTargetColumns.TabIndex = 16
        '
        'btnAddPair
        '
        Me.btnAddPair.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddPair.Enabled = False
        Me.btnAddPair.Location = New System.Drawing.Point(505, 603)
        Me.btnAddPair.Name = "btnAddPair"
        Me.btnAddPair.Size = New System.Drawing.Size(268, 24)
        Me.btnAddPair.TabIndex = 18
        Me.btnAddPair.Text = "Add Highlighted Pair to Map"
        Me.btnAddPair.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.txtNewCode)
        Me.GroupBox1.Controls.Add(Me.txtNewDesc)
        Me.GroupBox1.Location = New System.Drawing.Point(516, 18)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(275, 85)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Clone to New Template"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(85, 42)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "New Desc."
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(85, 27)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "New Code"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(6, 23)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(73, 39)
        Me.Button1.TabIndex = 24
        Me.Button1.Text = "Save As:  >"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtNewCode
        '
        Me.txtNewCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewCode.BackColor = System.Drawing.SystemColors.Info
        Me.txtNewCode.Location = New System.Drawing.Point(147, 23)
        Me.txtNewCode.Name = "txtNewCode"
        Me.txtNewCode.Size = New System.Drawing.Size(122, 20)
        Me.txtNewCode.TabIndex = 23
        '
        'txtNewDesc
        '
        Me.txtNewDesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewDesc.BackColor = System.Drawing.SystemColors.Info
        Me.txtNewDesc.Location = New System.Drawing.Point(147, 42)
        Me.txtNewDesc.Name = "txtNewDesc"
        Me.txtNewDesc.Size = New System.Drawing.Size(122, 20)
        Me.txtNewDesc.TabIndex = 22
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(13, 604)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 25
        Me.Button2.Text = "Delete Map"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(64, 63)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(50, 13)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "File Type"
        '
        'cboFileType
        '
        Me.cboFileType.FormattingEnabled = True
        Me.cboFileType.Items.AddRange(New Object() {"CSV", "XLS"})
        Me.cboFileType.Location = New System.Drawing.Point(120, 59)
        Me.cboFileType.Name = "cboFileType"
        Me.cboFileType.Size = New System.Drawing.Size(368, 21)
        Me.cboFileType.TabIndex = 27
        '
        'CBOSheetName
        '
        Me.CBOSheetName.FormattingEnabled = True
        Me.CBOSheetName.Items.AddRange(New Object() {"CSV", "XLS"})
        Me.CBOSheetName.Location = New System.Drawing.Point(120, 80)
        Me.CBOSheetName.Name = "CBOSheetName"
        Me.CBOSheetName.Size = New System.Drawing.Size(368, 21)
        Me.CBOSheetName.TabIndex = 29
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(48, 84)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(66, 13)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Sheet Name"
        '
        'bntValidate
        '
        Me.bntValidate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bntValidate.Location = New System.Drawing.Point(94, 604)
        Me.bntValidate.Name = "bntValidate"
        Me.bntValidate.Size = New System.Drawing.Size(75, 23)
        Me.bntValidate.TabIndex = 30
        Me.bntValidate.Text = "Validate Map"
        Me.bntValidate.UseVisualStyleBackColor = True
        '
        'btnExportmap
        '
        Me.btnExportmap.Location = New System.Drawing.Point(516, 110)
        Me.btnExportmap.Name = "btnExportmap"
        Me.btnExportmap.Size = New System.Drawing.Size(75, 23)
        Me.btnExportmap.TabIndex = 31
        Me.btnExportmap.Text = "Export Map"
        Me.btnExportmap.UseVisualStyleBackColor = True
        '
        'btnImportMap
        '
        Me.btnImportMap.Location = New System.Drawing.Point(710, 110)
        Me.btnImportMap.Name = "btnImportMap"
        Me.btnImportMap.Size = New System.Drawing.Size(75, 23)
        Me.btnImportMap.TabIndex = 32
        Me.btnImportMap.Text = "Import Map"
        Me.btnImportMap.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmEditMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(803, 639)
        Me.Controls.Add(Me.btnImportMap)
        Me.Controls.Add(Me.btnExportmap)
        Me.Controls.Add(Me.bntValidate)
        Me.Controls.Add(Me.CBOSheetName)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cboFileType)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnAddPair)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lbTargetColumns)
        Me.Controls.Add(Me.txtTargetTable)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtReceiptID)
        Me.Controls.Add(Me.txtFileMask)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblSampleFields)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lbAvailableFields)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboMap)
        Me.Controls.Add(Me.dbMap)
        Me.Name = "frmEditMap"
        Me.Text = "Edit Map"
        CType(Me.dbMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dbMap As DataGridView
    Friend WithEvents cboMap As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lbAvailableFields As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblSampleFields As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents txtFileMask As TextBox
    Friend WithEvents txtReceiptID As TextBox
    Friend WithEvents txtTargetTable As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lbTargetColumns As ListBox
    Friend WithEvents btnAddPair As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents txtNewCode As TextBox
    Friend WithEvents txtNewDesc As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents cboFileType As ComboBox
    Friend WithEvents CBOSheetName As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents bntValidate As Button
    Friend WithEvents btnExportmap As Button
    Friend WithEvents btnImportMap As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class
