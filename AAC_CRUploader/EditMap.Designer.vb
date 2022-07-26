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
        CType(Me.dbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dbMap
        '
        Me.dbMap.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dbMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dbMap.Location = New System.Drawing.Point(12, 146)
        Me.dbMap.MultiSelect = False
        Me.dbMap.Name = "dbMap"
        Me.dbMap.RowHeadersVisible = False
        Me.dbMap.Size = New System.Drawing.Size(476, 323)
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
        Me.Label1.Location = New System.Drawing.Point(9, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "File Layout Template"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(332, 488)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "File Mask"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Unique Receipt Key"
        '
        'lbAvailableFields
        '
        Me.lbAvailableFields.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbAvailableFields.FormattingEnabled = True
        Me.lbAvailableFields.Location = New System.Drawing.Point(521, 146)
        Me.lbAvailableFields.Name = "lbAvailableFields"
        Me.lbAvailableFields.Size = New System.Drawing.Size(127, 316)
        Me.lbAvailableFields.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 130)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Field Map"
        '
        'lblSampleFields
        '
        Me.lblSampleFields.AutoSize = True
        Me.lblSampleFields.Location = New System.Drawing.Point(518, 130)
        Me.lblSampleFields.Name = "lblSampleFields"
        Me.lblSampleFields.Size = New System.Drawing.Size(99, 13)
        Me.lblSampleFields.TabIndex = 10
        Me.lblSampleFields.Text = "Fields inSample File"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(413, 488)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txtFileMask
        '
        Me.txtFileMask.Location = New System.Drawing.Point(120, 45)
        Me.txtFileMask.Name = "txtFileMask"
        Me.txtFileMask.Size = New System.Drawing.Size(368, 20)
        Me.txtFileMask.TabIndex = 12
        '
        'txtReceiptID
        '
        Me.txtReceiptID.Location = New System.Drawing.Point(120, 71)
        Me.txtReceiptID.Name = "txtReceiptID"
        Me.txtReceiptID.Size = New System.Drawing.Size(368, 20)
        Me.txtReceiptID.TabIndex = 13
        '
        'txtTargetTable
        '
        Me.txtTargetTable.Location = New System.Drawing.Point(120, 97)
        Me.txtTargetTable.Name = "txtTargetTable"
        Me.txtTargetTable.Size = New System.Drawing.Size(368, 20)
        Me.txtTargetTable.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Target Table"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(651, 130)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Columns in Target"
        '
        'lbTargetColumns
        '
        Me.lbTargetColumns.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbTargetColumns.FormattingEnabled = True
        Me.lbTargetColumns.Location = New System.Drawing.Point(654, 146)
        Me.lbTargetColumns.Name = "lbTargetColumns"
        Me.lbTargetColumns.Size = New System.Drawing.Size(127, 316)
        Me.lbTargetColumns.TabIndex = 16
        '
        'frmEditMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(815, 537)
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
End Class
