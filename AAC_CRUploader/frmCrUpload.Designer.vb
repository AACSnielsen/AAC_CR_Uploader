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
        Me.txtCRFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboMap = New System.Windows.Forms.ComboBox()
        Me.dbMapz = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.gvData = New System.Windows.Forms.DataGridView()
        Me.btnEditMap = New System.Windows.Forms.Button()
        Me.btnMapRefresh = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ActivityText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkValidate = New System.Windows.Forms.CheckBox()
        Me.btnViewLog = New System.Windows.Forms.Button()
        Me.chkLogDebug = New System.Windows.Forms.CheckBox()
        CType(Me.dbMapz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gvData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtCRFile
        '
        Me.txtCRFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCRFile.Location = New System.Drawing.Point(109, 135)
        Me.txtCRFile.Name = "txtCRFile"
        Me.txtCRFile.Size = New System.Drawing.Size(1039, 20)
        Me.txtCRFile.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 138)
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
        Me.btnOpen.Location = New System.Drawing.Point(1154, 136)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(21, 20)
        Me.btnOpen.TabIndex = 2
        Me.btnOpen.Text = "..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnUpload
        '
        Me.btnUpload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpload.Location = New System.Drawing.Point(1016, 439)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(75, 23)
        Me.btnUpload.TabIndex = 3
        Me.btnUpload.Text = "Upload"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(1097, 439)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 30)
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
        Me.cboMap.Location = New System.Drawing.Point(120, 26)
        Me.cboMap.Name = "cboMap"
        Me.cboMap.Size = New System.Drawing.Size(1028, 21)
        Me.cboMap.TabIndex = 7
        '
        'dbMapz
        '
        Me.dbMapz.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbMapz.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dbMapz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dbMapz.Location = New System.Drawing.Point(11, 70)
        Me.dbMapz.MultiSelect = False
        Me.dbMapz.Name = "dbMapz"
        Me.dbMapz.ReadOnly = True
        Me.dbMapz.RowHeadersVisible = False
        Me.dbMapz.Size = New System.Drawing.Size(1164, 46)
        Me.dbMapz.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Map Header"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 165)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(98, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Input Data Preview"
        '
        'gvData
        '
        Me.gvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gvData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gvData.Location = New System.Drawing.Point(11, 185)
        Me.gvData.MultiSelect = False
        Me.gvData.Name = "gvData"
        Me.gvData.ReadOnly = True
        Me.gvData.RowHeadersVisible = False
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gvData.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gvData.RowTemplate.ReadOnly = True
        Me.gvData.Size = New System.Drawing.Size(1164, 248)
        Me.gvData.TabIndex = 13
        '
        'btnEditMap
        '
        Me.btnEditMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditMap.Enabled = False
        Me.btnEditMap.Location = New System.Drawing.Point(238, 439)
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
        Me.btnMapRefresh.Location = New System.Drawing.Point(1154, 25)
        Me.btnMapRefresh.Name = "btnMapRefresh"
        Me.btnMapRefresh.Size = New System.Drawing.Size(23, 23)
        Me.btnMapRefresh.TabIndex = 16
        Me.btnMapRefresh.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusText, Me.ActivityText})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 471)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1216, 22)
        Me.StatusStrip1.TabIndex = 18
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusText
        '
        Me.StatusText.Name = "StatusText"
        Me.StatusText.Size = New System.Drawing.Size(60, 17)
        Me.StatusText.Text = "StatusText"
        '
        'ActivityText
        '
        Me.ActivityText.Name = "ActivityText"
        Me.ActivityText.Size = New System.Drawing.Size(68, 17)
        Me.ActivityText.Text = "ActivityText"
        '
        'chkValidate
        '
        Me.chkValidate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkValidate.AutoSize = True
        Me.chkValidate.Checked = True
        Me.chkValidate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkValidate.Location = New System.Drawing.Point(592, 442)
        Me.chkValidate.Name = "chkValidate"
        Me.chkValidate.Size = New System.Drawing.Size(156, 17)
        Me.chkValidate.TabIndex = 19
        Me.chkValidate.Text = "Create Session after upload"
        Me.chkValidate.UseVisualStyleBackColor = True
        '
        'btnViewLog
        '
        Me.btnViewLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewLog.Location = New System.Drawing.Point(119, 439)
        Me.btnViewLog.Name = "btnViewLog"
        Me.btnViewLog.Size = New System.Drawing.Size(113, 23)
        Me.btnViewLog.TabIndex = 20
        Me.btnViewLog.Text = "View Log"
        Me.btnViewLog.UseVisualStyleBackColor = True
        '
        'chkLogDebug
        '
        Me.chkLogDebug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkLogDebug.AutoSize = True
        Me.chkLogDebug.Location = New System.Drawing.Point(14, 442)
        Me.chkLogDebug.Name = "chkLogDebug"
        Me.chkLogDebug.Size = New System.Drawing.Size(99, 17)
        Me.chkLogDebug.TabIndex = 21
        Me.chkLogDebug.Text = "Log Debugging"
        Me.chkLogDebug.UseVisualStyleBackColor = True
        '
        'UploadCRFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1216, 493)
        Me.Controls.Add(Me.chkLogDebug)
        Me.Controls.Add(Me.btnViewLog)
        Me.Controls.Add(Me.chkValidate)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnMapRefresh)
        Me.Controls.Add(Me.btnEditMap)
        Me.Controls.Add(Me.gvData)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dbMapz)
        Me.Controls.Add(Me.cboMap)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpload)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCRFile)
        Me.Name = "UploadCRFile"
        Me.Text = "Upload Cash Receipt File"
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
    Friend WithEvents dbMapz As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents gvData As DataGridView
    Friend WithEvents btnEditMap As Button
    Friend WithEvents btnMapRefresh As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusText As ToolStripStatusLabel
    Friend WithEvents chkValidate As CheckBox
    Friend WithEvents btnViewLog As Button
    Friend WithEvents chkLogDebug As CheckBox
    Friend WithEvents ActivityText As ToolStripStatusLabel
End Class
