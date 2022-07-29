
namespace SelfRegi_V2
{
    partial class Front
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

       
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteManual = new System.Windows.Forms.RadioButton();
            this.btnDeleteSingle2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnDeleteAuto = new System.Windows.Forms.RadioButton();
            this.btnInsert = new System.Windows.Forms.RadioButton();
            this.lJan = new System.Windows.Forms.Label();
            this.txtJan = new System.Windows.Forms.TextBox();
            this.lRfid = new System.Windows.Forms.Label();
            this.txtRfid = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lNew = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMakerName = new System.Windows.Forms.TextBox();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lProductType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lRCode = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lJanCode = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.Image = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Peru;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(984, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(37, 36);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnDeleteManual);
            this.panel1.Controls.Add(this.btnDeleteSingle2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.btnDeleteAuto);
            this.panel1.Controls.Add(this.btnInsert);
            this.panel1.Controls.Add(this.lJan);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.txtJan);
            this.panel1.Controls.Add(this.lRfid);
            this.panel1.Controls.Add(this.txtRfid);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1021, 161);
            this.panel1.TabIndex = 25;
            // 
            // btnDeleteManual
            // 
            this.btnDeleteManual.AutoSize = true;
            this.btnDeleteManual.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDeleteManual.ForeColor = System.Drawing.Color.Red;
            this.btnDeleteManual.Location = new System.Drawing.Point(560, 11);
            this.btnDeleteManual.Name = "btnDeleteManual";
            this.btnDeleteManual.Size = new System.Drawing.Size(212, 31);
            this.btnDeleteManual.TabIndex = 10;
            this.btnDeleteManual.Text = "Manual Delete";
            this.btnDeleteManual.UseVisualStyleBackColor = true;
            this.btnDeleteManual.CheckedChanged += new System.EventHandler(this.btnDeleteManual_CheckedChanged);
            // 
            // btnDeleteSingle2
            // 
            this.btnDeleteSingle2.BackColor = System.Drawing.Color.Teal;
            this.btnDeleteSingle2.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnDeleteSingle2.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSingle2.Location = new System.Drawing.Point(862, 11);
            this.btnDeleteSingle2.Name = "btnDeleteSingle2";
            this.btnDeleteSingle2.Size = new System.Drawing.Size(99, 39);
            this.btnDeleteSingle2.TabIndex = 11;
            this.btnDeleteSingle2.Text = "Delete";
            this.btnDeleteSingle2.UseVisualStyleBackColor = false;
            this.btnDeleteSingle2.Visible = false;
            this.btnDeleteSingle2.Click += new System.EventHandler(this.btnDeleteSingle_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(22, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 33);
            this.label9.TabIndex = 32;
            this.label9.Text = "Edit Mode";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // btnDeleteAuto
            // 
            this.btnDeleteAuto.AutoSize = true;
            this.btnDeleteAuto.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDeleteAuto.ForeColor = System.Drawing.Color.Red;
            this.btnDeleteAuto.Location = new System.Drawing.Point(368, 11);
            this.btnDeleteAuto.Name = "btnDeleteAuto";
            this.btnDeleteAuto.Size = new System.Drawing.Size(184, 31);
            this.btnDeleteAuto.TabIndex = 9;
            this.btnDeleteAuto.Text = "Auto Delete";
            this.btnDeleteAuto.UseVisualStyleBackColor = true;
            this.btnDeleteAuto.CheckedChanged += new System.EventHandler(this.btnDelete_CheckedChanged);
            // 
            // btnInsert
            // 
            this.btnInsert.AutoSize = true;
            this.btnInsert.Checked = true;
            this.btnInsert.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnInsert.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnInsert.Location = new System.Drawing.Point(235, 11);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(114, 31);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.TabStop = true;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.CheckedChanged += new System.EventHandler(this.btnInsert_CheckedChanged);
            // 
            // lJan
            // 
            this.lJan.AutoSize = true;
            this.lJan.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lJan.Location = new System.Drawing.Point(22, 112);
            this.lJan.Name = "lJan";
            this.lJan.Size = new System.Drawing.Size(134, 33);
            this.lJan.TabIndex = 7;
            this.lJan.Text = "Jancode";
            // 
            // txtJan
            // 
            this.txtJan.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtJan.Enabled = false;
            this.txtJan.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtJan.Location = new System.Drawing.Point(235, 106);
            this.txtJan.Name = "txtJan";
            this.txtJan.ReadOnly = true;
            this.txtJan.Size = new System.Drawing.Size(725, 39);
            this.txtJan.TabIndex = 6;
            this.txtJan.TextChanged += new System.EventHandler(this.txtJan_TextChanged);
            // 
            // lRfid
            // 
            this.lRfid.AutoSize = true;
            this.lRfid.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lRfid.Location = new System.Drawing.Point(22, 62);
            this.lRfid.Name = "lRfid";
            this.lRfid.Size = new System.Drawing.Size(83, 33);
            this.lRfid.TabIndex = 5;
            this.lRfid.Text = "RFID";
            // 
            // txtRfid
            // 
            this.txtRfid.AcceptsTab = true;
            this.txtRfid.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtRfid.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtRfid.Location = new System.Drawing.Point(235, 56);
            this.txtRfid.Name = "txtRfid";
            this.txtRfid.ReadOnly = true;
            this.txtRfid.Size = new System.Drawing.Size(725, 39);
            this.txtRfid.TabIndex = 4;
            this.txtRfid.TextChanged += new System.EventHandler(this.txtRfid_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.lNew);
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Location = new System.Drawing.Point(3, 169);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 460);
            this.panel2.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1, 420);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 24);
            this.label7.TabIndex = 28;
            this.label7.Visible = false;
            // 
            // lNew
            // 
            this.lNew.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.lNew.AutoSize = true;
            this.lNew.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lNew.ForeColor = System.Drawing.Color.Red;
            this.lNew.Location = new System.Drawing.Point(7, 24);
            this.lNew.Name = "lNew";
            this.lNew.Size = new System.Drawing.Size(0, 33);
            this.lNew.TabIndex = 26;
            this.lNew.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.09678F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.90323F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtMakerName, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtProductID, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtPrice, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lProductType, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lRCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lJanCode, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-1, -1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(698, 460);
            this.tableLayoutPanel1.TabIndex = 25;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(4, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "Product Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(4, 413);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 24);
            this.label5.TabIndex = 12;
            this.label5.Text = "Product ID";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(4, 216);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(217, 24);
            this.label8.TabIndex = 22;
            this.label8.Text = "Vendor";
            // 
            // txtPName
            // 
            this.txtPName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPName.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPName.Location = new System.Drawing.Point(228, 151);
            this.txtPName.MaxLength = 64;
            this.txtPName.Name = "txtPName";
            this.txtPName.ReadOnly = true;
            this.txtPName.Size = new System.Drawing.Size(466, 24);
            this.txtPName.TabIndex = 1;
            this.txtPName.TextChanged += new System.EventHandler(this.txtPName_TextChanged);
            this.txtPName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPName_KeyDown);
            this.txtPName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPName_KeyPress);
            this.txtPName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPName_KeyUp);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(4, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "Product Type";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(4, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(217, 24);
            this.label6.TabIndex = 13;
            this.label6.Text = "Price";
            // 
            // txtMakerName
            // 
            this.txtMakerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMakerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMakerName.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMakerName.Location = new System.Drawing.Point(228, 216);
            this.txtMakerName.MaxLength = 4;
            this.txtMakerName.Name = "txtMakerName";
            this.txtMakerName.ReadOnly = true;
            this.txtMakerName.Size = new System.Drawing.Size(466, 24);
            this.txtMakerName.TabIndex = 2;
            this.txtMakerName.TextChanged += new System.EventHandler(this.txtMakerName_TextChanged);
            this.txtMakerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMakerName_KeyDown);
            this.txtMakerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMakerName_KeyPress);
            // 
            // txtProductID
            // 
            this.txtProductID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProductID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProductID.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtProductID.Location = new System.Drawing.Point(228, 413);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.ReadOnly = true;
            this.txtProductID.Size = new System.Drawing.Size(466, 24);
            this.txtProductID.TabIndex = 4;
            this.txtProductID.TextChanged += new System.EventHandler(this.txtProductID_TextChanged);
            this.txtProductID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductID_KeyDown);
            // 
            // txtPrice
            // 
            this.txtPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrice.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPrice.Location = new System.Drawing.Point(228, 281);
            this.txtPrice.MaxLength = 5;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(466, 24);
            this.txtPrice.TabIndex = 3;
            this.txtPrice.TextChanged += new System.EventHandler(this.txtPrice_TextChanged);
            this.txtPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrice_KeyDown);
            this.txtPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrice_KeyPress);
            // 
            // lProductType
            // 
            this.lProductType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lProductType.AutoSize = true;
            this.lProductType.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lProductType.Location = new System.Drawing.Point(228, 346);
            this.lProductType.Name = "lProductType";
            this.lProductType.Size = new System.Drawing.Size(466, 24);
            this.lProductType.TabIndex = 17;
            this.lProductType.Click += new System.EventHandler(this.lProductType_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(4, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "RFID";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lRCode
            // 
            this.lRCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lRCode.AutoSize = true;
            this.lRCode.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lRCode.Location = new System.Drawing.Point(228, 21);
            this.lRCode.Name = "lRCode";
            this.lRCode.Size = new System.Drawing.Size(466, 24);
            this.lRCode.TabIndex = 15;
            this.lRCode.Click += new System.EventHandler(this.lRCode_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(4, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "Jancode";
            // 
            // lJanCode
            // 
            this.lJanCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lJanCode.AutoSize = true;
            this.lJanCode.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lJanCode.Location = new System.Drawing.Point(228, 86);
            this.lJanCode.Name = "lJanCode";
            this.lJanCode.Size = new System.Drawing.Size(466, 24);
            this.lJanCode.TabIndex = 16;
            this.lJanCode.Click += new System.EventHandler(this.lJanCode_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Teal;
            this.btnCancel.Font = new System.Drawing.Font("MS Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(564, 654);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(213, 91);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClear.Location = new System.Drawing.Point(720, 596);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(287, 33);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(720, 479);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(287, 103);
            this.richTextBox1.TabIndex = 28;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Teal;
            this.btnRegister.Font = new System.Drawing.Font("MS Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(783, 654);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(213, 91);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Visible = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Teal;
            this.btnConnect.Font = new System.Drawing.Font("MS Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(17, 665);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(216, 91);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Image
            // 
            this.Image.Location = new System.Drawing.Point(721, 169);
            this.Image.Name = "Image";
            this.Image.Size = new System.Drawing.Size(286, 304);
            this.Image.TabIndex = 29;
            this.Image.TabStop = false;
            // 
            // Front
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.Image);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Front";
            this.Text = "Self Register App";
            this.Load += new System.EventHandler(this.Front_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Front_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Front_KeyPress);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lJan;
        private System.Windows.Forms.TextBox txtJan;
        private System.Windows.Forms.Label lRfid;
        private System.Windows.Forms.TextBox txtRfid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lNew;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lProductType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lJanCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lRCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPName;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMakerName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton btnDeleteAuto;
        private System.Windows.Forms.RadioButton btnInsert;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton btnDeleteManual;
        private System.Windows.Forms.Button btnDeleteSingle2;
        private System.Windows.Forms.PictureBox Image;
    }
}

