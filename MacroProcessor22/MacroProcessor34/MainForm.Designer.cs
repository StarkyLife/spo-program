namespace MacroProcessor34
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tb_error = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_next_step = new System.Windows.Forms.Button();
            this.btn_refresh_all = new System.Windows.Forms.Button();
            this.btn_first_run = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.tb_output_file = new System.Windows.Forms.TextBox();
            this.tb_result = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_global = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.dgv_tmo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_load_file = new System.Windows.Forms.Button();
            this.tb_input_file = new System.Windows.Forms.TextBox();
            this.tb_source_code = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_global)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tmo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_error
            // 
            this.tb_error.Enabled = false;
            this.tb_error.Location = new System.Drawing.Point(325, 467);
            this.tb_error.Multiline = true;
            this.tb_error.Name = "tb_error";
            this.tb_error.Size = new System.Drawing.Size(307, 76);
            this.tb_error.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(453, 451);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Ошибка";
            // 
            // btn_next_step
            // 
            this.btn_next_step.Enabled = false;
            this.btn_next_step.Location = new System.Drawing.Point(120, 520);
            this.btn_next_step.Name = "btn_next_step";
            this.btn_next_step.Size = new System.Drawing.Size(104, 23);
            this.btn_next_step.TabIndex = 24;
            this.btn_next_step.Text = "Второй проход";
            this.btn_next_step.UseVisualStyleBackColor = true;
            this.btn_next_step.Click += new System.EventHandler(this.btn_next_step_Click);
            // 
            // btn_refresh_all
            // 
            this.btn_refresh_all.Enabled = false;
            this.btn_refresh_all.Location = new System.Drawing.Point(230, 520);
            this.btn_refresh_all.Name = "btn_refresh_all";
            this.btn_refresh_all.Size = new System.Drawing.Size(89, 23);
            this.btn_refresh_all.TabIndex = 23;
            this.btn_refresh_all.Text = "Заново";
            this.btn_refresh_all.UseVisualStyleBackColor = true;
            this.btn_refresh_all.Click += new System.EventHandler(this.btn_refresh_all_Click);
            // 
            // btn_first_run
            // 
            this.btn_first_run.Enabled = false;
            this.btn_first_run.Location = new System.Drawing.Point(12, 520);
            this.btn_first_run.Name = "btn_first_run";
            this.btn_first_run.Size = new System.Drawing.Size(102, 23);
            this.btn_first_run.TabIndex = 22;
            this.btn_first_run.Text = "Первый проход";
            this.btn_first_run.UseVisualStyleBackColor = true;
            this.btn_first_run.Click += new System.EventHandler(this.btn_first_run_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_save);
            this.groupBox3.Controls.Add(this.tb_output_file);
            this.groupBox3.Controls.Add(this.tb_result);
            this.groupBox3.Location = new System.Drawing.Point(638, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(307, 531);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ассемблерный код";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(101, 505);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(106, 20);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "В файл";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // tb_output_file
            // 
            this.tb_output_file.Enabled = false;
            this.tb_output_file.Location = new System.Drawing.Point(262, 18);
            this.tb_output_file.Name = "tb_output_file";
            this.tb_output_file.Size = new System.Drawing.Size(13, 20);
            this.tb_output_file.TabIndex = 7;
            this.tb_output_file.Visible = false;
            // 
            // tb_result
            // 
            this.tb_result.Enabled = false;
            this.tb_result.Location = new System.Drawing.Point(6, 18);
            this.tb_result.Multiline = true;
            this.tb_result.Name = "tb_result";
            this.tb_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_result.Size = new System.Drawing.Size(295, 481);
            this.tb_result.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dgv_global);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dgv_tmo);
            this.groupBox2.Location = new System.Drawing.Point(325, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 436);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Таблицы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Глобальные переменные";
            // 
            // dgv_global
            // 
            this.dgv_global.AllowUserToAddRows = false;
            this.dgv_global.AllowUserToDeleteRows = false;
            this.dgv_global.AllowUserToResizeColumns = false;
            this.dgv_global.AllowUserToResizeRows = false;
            this.dgv_global.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_global.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgv_global.Location = new System.Drawing.Point(6, 332);
            this.dgv_global.Name = "dgv_global";
            this.dgv_global.RowHeadersVisible = false;
            this.dgv_global.Size = new System.Drawing.Size(295, 98);
            this.dgv_global.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Имя";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Значение";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(121, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Макросы";
            // 
            // dgv_tmo
            // 
            this.dgv_tmo.AllowUserToAddRows = false;
            this.dgv_tmo.AllowUserToDeleteRows = false;
            this.dgv_tmo.AllowUserToResizeColumns = false;
            this.dgv_tmo.AllowUserToResizeRows = false;
            this.dgv_tmo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_tmo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgv_tmo.Location = new System.Drawing.Point(6, 32);
            this.dgv_tmo.Name = "dgv_tmo";
            this.dgv_tmo.RowHeadersVisible = false;
            this.dgv_tmo.Size = new System.Drawing.Size(295, 282);
            this.dgv_tmo.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Имя макроса";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Тело макроса";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_load_file);
            this.groupBox1.Controls.Add(this.tb_input_file);
            this.groupBox1.Controls.Add(this.tb_source_code);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 502);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Исходные данные";
            // 
            // btn_load_file
            // 
            this.btn_load_file.Location = new System.Drawing.Point(109, 472);
            this.btn_load_file.Name = "btn_load_file";
            this.btn_load_file.Size = new System.Drawing.Size(93, 20);
            this.btn_load_file.TabIndex = 7;
            this.btn_load_file.Text = "Из файла";
            this.btn_load_file.UseVisualStyleBackColor = true;
            this.btn_load_file.Click += new System.EventHandler(this.btn_load_file_Click);
            // 
            // tb_input_file
            // 
            this.tb_input_file.Enabled = false;
            this.tb_input_file.Location = new System.Drawing.Point(263, 20);
            this.tb_input_file.Name = "tb_input_file";
            this.tb_input_file.Size = new System.Drawing.Size(38, 20);
            this.tb_input_file.TabIndex = 6;
            this.tb_input_file.Visible = false;
            // 
            // tb_source_code
            // 
            this.tb_source_code.Location = new System.Drawing.Point(6, 20);
            this.tb_source_code.Multiline = true;
            this.tb_source_code.Name = "tb_source_code";
            this.tb_source_code.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_source_code.Size = new System.Drawing.Size(295, 446);
            this.tb_source_code.TabIndex = 1;
            this.tb_source_code.TextChanged += new System.EventHandler(this.tb_source_code_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 553);
            this.Controls.Add(this.tb_error);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_next_step);
            this.Controls.Add(this.btn_refresh_all);
            this.Controls.Add(this.btn_first_run);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Программа";
            this.Load += new System.EventHandler(this.fm_main_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_global)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_tmo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_error;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_next_step;
        private System.Windows.Forms.Button btn_refresh_all;
        private System.Windows.Forms.Button btn_first_run;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox tb_output_file;
        private System.Windows.Forms.TextBox tb_result;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv_global;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgv_tmo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_load_file;
        private System.Windows.Forms.TextBox tb_input_file;
        private System.Windows.Forms.TextBox tb_source_code;
    }
}