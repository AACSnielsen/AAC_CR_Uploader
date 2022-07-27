<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UploadCRFile
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtCRFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboMap = New System.Windows.Forms.ComboBox()
        Me.dbMap = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dbMapz = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.gvData = New System.Windows.Forms.DataGridView()
        Me.chkNoUpdate = New System.Windows.Forms.CheckBox()
        Me.btnEditMap = New System.Windows.Forms.Button()
        Me.btnMapRefresh = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkValidate = New System.Windows.Forms.CheckBox()
        Me.btnViewLog = New System.Windows.Forms.Button()
        CType(Me.dbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dbMapz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtCRFile
        '
        Me.txtCRFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCRFile.Location = New System.Drawing.Point(107, 297)
        Me.txtCRFile.Name = "txtCRFile"
        Me.txtCRFile.Size = New System.Drawing.Size(777, 20)
        Me.txtCRFile.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 300)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Cash Receipt File"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnOpen
        '
        Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpen.Location = New System.Drawing.Point(890, 298)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(21, 20)
        Me.btnOpen.TabIndex = 2
        Me.btnOpen.Text = "..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnUpload
        '
        Me.btnUpload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpload.Location = New System.Drawing.Point(754, 545)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(75, 23)
        Me.btnUpload.TabIndex = 3
        Me.btnUpload.Text = "Upload"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(835, 545)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "File Layout Template"
        '
        'cboMap
        '
        Me.cboMap.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMap.FormattingEnabled = True
        Me.cboMap.Location = New System.Drawing.Point(120, 5)
        Me.cboMap.Name = "cboMap"
        Me.cboMap.Size = New System.Drawing.Size(766, 21)
        Me.cboMap.TabIndex = 7
        '
        'dbMap
        '
        Me.dbMap.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dbMap.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dbMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dbMap.Location = New System.Drawing.Point(12, 133)
        Me.dbMap.MultiSelect = False
        Me.dbMap.Name = "dbMap"
        Me.dbMap.ReadOnly = True
        Me.dbMap.RowHeadersVisible = False
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dbMap.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dbMap.RowTemplate.Height = 18
        Me.dbMap.RowTemplate.ReadOnly = True
        Me.dbMap.Size = New System.Drawing.Size(902, 147)
        Me.dbMap.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 117)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Data Mappings"
        '
        'dbMapz
        '
        Me.dbMapz.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbMapz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dbMapz.Location = New System.Drawing.Point(12, 68)
        Me.dbMapz.MultiSelect = False
        Me.dbMapz.Name = "dbMapz"
        Me.dbMapz.ReadOnly = True
        Me.dbMapz.RowHeadersVisible = False
        Me.dbMapz.Size = New System.Drawing.Size(902, 46)
        Me.dbMapz.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Map Header"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 333)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Data Preview"
        '
        'gvData
        '
        Me.gvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gvData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvData.Location = New System.Drawing.Point(11, 348)
        Me.gvData.MultiSelect = False
        Me.gvData.Name = "gvData"
        Me.gvData.ReadOnly = True
        Me.gvData.RowHeadersVisible = False
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gvData.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.gvData.RowTemplate.ReadOnly = True
        Me.gvData.Size = New System.Drawing.Size(902, 191)
        Me.gvData.TabIndex = 13
        '
        'chkNoUpdate
        '
        Me.chkNoUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkNoUpdate.AutoSize = True
        Me.chkNoUpdate.Location = New System.Drawing.Point(6, 545)
        Me.chkNoUpdate.Name = "chkNoUpdate"
        Me.chkNoUpdate.Size = New System.Drawing.Size(107, 17)
        Me.chkNoUpdate.TabIndex = 14
        Me.chkNoUpdate.Text = "No SQL Updates"
        Me.chkNoUpdate.UseVisualStyleBackColor = True
        Me.chkNoUpdate.Visible = False
        '
        'btnEditMap
        '
        Me.btnEditMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditMap.Location = New System.Drawing.Point(302, 545)
        Me.btnEditMap.Name = "btnEditMap"
        Me.btnEditMap.Size = New System.Drawing.Size(113, 23)
        Me.btnEditMap.TabIndex = 15
        Me.btnEditMap.Text = "Edit Mapping"
        Me.btnEditMap.UseVisualStyleBackColor = True
        '
        'btnMapRefresh
        '
        Me.btnMapRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMapRefresh.Image = Global.AAC_CRUploader.My.Resources.Resources.Refresh
        Me.btnMapRefresh.Location = New System.Drawing.Point(892, 4)
        Me.btnMapRefresh.Name = "btnMapRefresh"
        Me.btnMapRefresh.Size = New System.Drawing.Size(23, 23)
        Me.btnMapRefresh.TabIndex = 16
        Me.btnMapRefresh.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 586)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(954, 22)
        Me.StatusStrip1.TabIndex = 18
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(60, 17)
        Me.ToolStripStatusLabel1.Text = "StatusText"
        '
        'chkValidate
        '
        Me.chkValidate.AutoSize = True
        Me.chkValidate.Checked = True
        Me.chkValidate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkValidate.Location = New System.Drawing.Point(623, 551)
        Me.chkValidate.Name = "chkValidate"
        Me.chkValidate.Size = New System.Drawing.Size(125, 17)
        Me.chkValidate.TabIndex = 19
        Me.chkValidate.Text = "Validate after Upload"
        Me.chkValidate.UseVisualStyleBackColor = True
        '
        'btnViewLog
        '
        Me.btnViewLog.Location = New System.Drawing.Point(421, 545)
        Me.btnViewLog.Name = "btnViewLog"
        Me.btnViewLog.Size = New System.Drawing.Size(113, 23)
        Me.btnViewLog.TabIndex = 20
        Me.btnViewLog.Text = "View Validation Log"
        Me.btnViewLog.UseVisualStyleBackColor = True
        '
        'UploadCRFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(954, 608)
        Me.Controls.Add(Me.btnViewLog)
        Me.Controls.Add(Me.chkValidate)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnMapRefresh)
        Me.Controls.Add(Me.btnEditMap)
        Me.Controls.Add(Me.chkNoUpdate)
        Me.Controls.Add(Me.gvData)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dbMapz)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dbMap)
        Me.Controls.Add(Me.cboMap)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpload)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCRFile)
        Me.Name = "UploadCRFile"
        Me.Text = "Upload Cash Receipt File"
        CType(Me.dbMap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dbMapz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gvData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtCRFile As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btnOpen As Button
    Friend WithEvents btnUpload As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents cboMap As ComboBox
    Friend WithEvents dbMap As DataGridView
    Friend WithEvents Label3 As Label
    Friend WithEvents dbMapz As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents gvData As DataGridView
    Friend WithEvents chkNoUpdate As CheckBox
    Friend WithEvents btnEditMap As Button
    Friend WithEvents btnMapRefresh As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents chkValidate As CheckBox
    Friend WithEvents btnViewLog As Button
End Class
