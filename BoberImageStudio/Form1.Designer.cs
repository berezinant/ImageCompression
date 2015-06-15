namespace BoberImageStudio
{
    partial class BoberForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.OriginalImageBox = new System.Windows.Forms.PictureBox();
            this.ModifiedImageBox = new System.Windows.Forms.PictureBox();
            this.OpenButton1 = new System.Windows.Forms.Button();
            this.YUVButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.Original = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PSNRButton = new System.Windows.Forms.Button();
            this.PSNRBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.YTrack = new System.Windows.Forms.TrackBar();
            this.UTrack = new System.Windows.Forms.TrackBar();
            this.VTrack = new System.Windows.Forms.TrackBar();
            this.SaveButton1 = new System.Windows.Forms.Button();
            this.SaveButton2 = new System.Windows.Forms.Button();
            this.OpenButton2 = new System.Windows.Forms.Button();
            this.GrayscaleButton = new System.Windows.Forms.Button();
            this.YButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.UButton = new System.Windows.Forms.Button();
            this.VButton = new System.Windows.Forms.Button();
            this.CbButton = new System.Windows.Forms.Button();
            this.CrButton = new System.Windows.Forms.Button();
            this.YButton1 = new System.Windows.Forms.Button();
            this.CbTrack = new System.Windows.Forms.TrackBar();
            this.CrTrack = new System.Windows.Forms.TrackBar();
            this.YTrack1 = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.YCrCbButton = new System.Windows.Forms.Button();
            this.DecimateButton = new System.Windows.Forms.Button();
            this.DecimateOptionBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModifiedImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrack1)).BeginInit();
            this.SuspendLayout();
            // 
            // OriginalImageBox
            // 
            this.OriginalImageBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.OriginalImageBox.Location = new System.Drawing.Point(12, 28);
            this.OriginalImageBox.Name = "OriginalImageBox";
            this.OriginalImageBox.Size = new System.Drawing.Size(256, 256);
            this.OriginalImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OriginalImageBox.TabIndex = 0;
            this.OriginalImageBox.TabStop = false;
            this.OriginalImageBox.Tag = "";
            // 
            // ModifiedImageBox
            // 
            this.ModifiedImageBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ModifiedImageBox.Location = new System.Drawing.Point(312, 28);
            this.ModifiedImageBox.Name = "ModifiedImageBox";
            this.ModifiedImageBox.Size = new System.Drawing.Size(256, 256);
            this.ModifiedImageBox.TabIndex = 1;
            this.ModifiedImageBox.TabStop = false;
            // 
            // OpenButton1
            // 
            this.OpenButton1.Location = new System.Drawing.Point(12, 290);
            this.OpenButton1.Name = "OpenButton1";
            this.OpenButton1.Size = new System.Drawing.Size(75, 23);
            this.OpenButton1.TabIndex = 2;
            this.OpenButton1.Text = "Open";
            this.OpenButton1.UseVisualStyleBackColor = true;
            this.OpenButton1.Click += new System.EventHandler(this.OpenButton1_Click);
            // 
            // YUVButton
            // 
            this.YUVButton.Location = new System.Drawing.Point(595, 28);
            this.YUVButton.Name = "YUVButton";
            this.YUVButton.Size = new System.Drawing.Size(99, 23);
            this.YUVButton.TabIndex = 3;
            this.YUVButton.Text = "YUV";
            this.YUVButton.UseVisualStyleBackColor = true;
            this.YUVButton.Click += new System.EventHandler(this.YUV_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(12, 375);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 4;
            this.ClearButton.Text = "Clear all";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // Original
            // 
            this.Original.AutoSize = true;
            this.Original.Location = new System.Drawing.Point(119, 9);
            this.Original.Name = "Original";
            this.Original.Size = new System.Drawing.Size(41, 13);
            this.Original.TabIndex = 6;
            this.Original.Text = "Source";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(424, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Modified";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(596, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(596, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "U";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(595, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "V";
            // 
            // PSNRButton
            // 
            this.PSNRButton.Location = new System.Drawing.Point(710, 28);
            this.PSNRButton.Name = "PSNRButton";
            this.PSNRButton.Size = new System.Drawing.Size(46, 23);
            this.PSNRButton.TabIndex = 19;
            this.PSNRButton.Text = "PSNR";
            this.PSNRButton.UseVisualStyleBackColor = true;
            this.PSNRButton.Click += new System.EventHandler(this.PSNRButton_Click);
            // 
            // PSNRBox
            // 
            this.PSNRBox.Location = new System.Drawing.Point(762, 30);
            this.PSNRBox.Name = "PSNRBox";
            this.PSNRBox.Size = new System.Drawing.Size(63, 20);
            this.PSNRBox.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(831, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "dB";
            // 
            // YTrack
            // 
            this.YTrack.Location = new System.Drawing.Point(617, 56);
            this.YTrack.Maximum = 8;
            this.YTrack.Name = "YTrack";
            this.YTrack.Size = new System.Drawing.Size(150, 45);
            this.YTrack.TabIndex = 22;
            this.YTrack.Value = 8;
            // 
            // UTrack
            // 
            this.UTrack.Location = new System.Drawing.Point(617, 83);
            this.UTrack.Maximum = 8;
            this.UTrack.Name = "UTrack";
            this.UTrack.Size = new System.Drawing.Size(150, 45);
            this.UTrack.TabIndex = 23;
            this.UTrack.Value = 8;
            // 
            // VTrack
            // 
            this.VTrack.Location = new System.Drawing.Point(617, 109);
            this.VTrack.Maximum = 8;
            this.VTrack.Name = "VTrack";
            this.VTrack.Size = new System.Drawing.Size(150, 45);
            this.VTrack.TabIndex = 24;
            this.VTrack.Value = 8;
            // 
            // SaveButton1
            // 
            this.SaveButton1.Location = new System.Drawing.Point(12, 319);
            this.SaveButton1.Name = "SaveButton1";
            this.SaveButton1.Size = new System.Drawing.Size(75, 23);
            this.SaveButton1.TabIndex = 25;
            this.SaveButton1.Text = "Save";
            this.SaveButton1.UseVisualStyleBackColor = true;
            this.SaveButton1.Click += new System.EventHandler(this.SaveButton1_Click);
            // 
            // SaveButton2
            // 
            this.SaveButton2.Location = new System.Drawing.Point(312, 319);
            this.SaveButton2.Name = "SaveButton2";
            this.SaveButton2.Size = new System.Drawing.Size(75, 23);
            this.SaveButton2.TabIndex = 27;
            this.SaveButton2.Text = "Save";
            this.SaveButton2.UseVisualStyleBackColor = true;
            this.SaveButton2.Click += new System.EventHandler(this.SaveButton2_Click);
            // 
            // OpenButton2
            // 
            this.OpenButton2.Location = new System.Drawing.Point(312, 290);
            this.OpenButton2.Name = "OpenButton2";
            this.OpenButton2.Size = new System.Drawing.Size(75, 23);
            this.OpenButton2.TabIndex = 26;
            this.OpenButton2.Text = "Open";
            this.OpenButton2.UseVisualStyleBackColor = true;
            this.OpenButton2.Click += new System.EventHandler(this.OpenButton2_Click);
            // 
            // GrayscaleButton
            // 
            this.GrayscaleButton.Location = new System.Drawing.Point(166, 290);
            this.GrayscaleButton.Name = "GrayscaleButton";
            this.GrayscaleButton.Size = new System.Drawing.Size(102, 23);
            this.GrayscaleButton.TabIndex = 28;
            this.GrayscaleButton.Text = "Show Grayscales";
            this.GrayscaleButton.UseVisualStyleBackColor = true;
            this.GrayscaleButton.Click += new System.EventHandler(this.GrayscaleButton_Click);
            // 
            // YButton
            // 
            this.YButton.Location = new System.Drawing.Point(762, 57);
            this.YButton.Name = "YButton";
            this.YButton.Size = new System.Drawing.Size(102, 23);
            this.YButton.TabIndex = 29;
            this.YButton.Text = "Y";
            this.YButton.UseVisualStyleBackColor = true;
            this.YButton.Click += new System.EventHandler(this.YButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "(changes source)";
            // 
            // UButton
            // 
            this.UButton.Location = new System.Drawing.Point(762, 80);
            this.UButton.Name = "UButton";
            this.UButton.Size = new System.Drawing.Size(102, 23);
            this.UButton.TabIndex = 31;
            this.UButton.Text = "U";
            this.UButton.UseVisualStyleBackColor = true;
            this.UButton.Click += new System.EventHandler(this.UButton_Click);
            // 
            // VButton
            // 
            this.VButton.Location = new System.Drawing.Point(762, 105);
            this.VButton.Name = "VButton";
            this.VButton.Size = new System.Drawing.Size(102, 23);
            this.VButton.TabIndex = 32;
            this.VButton.Text = "V";
            this.VButton.UseVisualStyleBackColor = true;
            this.VButton.Click += new System.EventHandler(this.VButton_Click);
            // 
            // CbButton
            // 
            this.CbButton.Location = new System.Drawing.Point(762, 237);
            this.CbButton.Name = "CbButton";
            this.CbButton.Size = new System.Drawing.Size(102, 23);
            this.CbButton.TabIndex = 45;
            this.CbButton.Text = "Cb";
            this.CbButton.UseVisualStyleBackColor = true;
            this.CbButton.Click += new System.EventHandler(this.CbButton_Click);
            // 
            // CrButton
            // 
            this.CrButton.Location = new System.Drawing.Point(762, 212);
            this.CrButton.Name = "CrButton";
            this.CrButton.Size = new System.Drawing.Size(102, 23);
            this.CrButton.TabIndex = 44;
            this.CrButton.Text = "Cr";
            this.CrButton.UseVisualStyleBackColor = true;
            this.CrButton.Click += new System.EventHandler(this.CrButton_Click);
            // 
            // YButton1
            // 
            this.YButton1.Location = new System.Drawing.Point(762, 189);
            this.YButton1.Name = "YButton1";
            this.YButton1.Size = new System.Drawing.Size(102, 23);
            this.YButton1.TabIndex = 43;
            this.YButton1.Text = "Y";
            this.YButton1.UseVisualStyleBackColor = true;
            this.YButton1.Click += new System.EventHandler(this.YButton1_Click);
            // 
            // CbTrack
            // 
            this.CbTrack.Location = new System.Drawing.Point(617, 241);
            this.CbTrack.Maximum = 8;
            this.CbTrack.Name = "CbTrack";
            this.CbTrack.Size = new System.Drawing.Size(150, 45);
            this.CbTrack.TabIndex = 42;
            this.CbTrack.Value = 8;
            // 
            // CrTrack
            // 
            this.CrTrack.Location = new System.Drawing.Point(617, 215);
            this.CrTrack.Maximum = 8;
            this.CrTrack.Name = "CrTrack";
            this.CrTrack.Size = new System.Drawing.Size(150, 45);
            this.CrTrack.TabIndex = 41;
            this.CrTrack.Value = 8;
            // 
            // YTrack1
            // 
            this.YTrack1.Location = new System.Drawing.Point(617, 188);
            this.YTrack1.Maximum = 8;
            this.YTrack1.Name = "YTrack1";
            this.YTrack1.Size = new System.Drawing.Size(150, 45);
            this.YTrack1.TabIndex = 40;
            this.YTrack1.Value = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(595, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 17);
            this.label8.TabIndex = 36;
            this.label8.Text = "Cb";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(596, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 17);
            this.label9.TabIndex = 35;
            this.label9.Text = "Cr";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(596, 192);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 17);
            this.label10.TabIndex = 34;
            this.label10.Text = "Y";
            // 
            // YCrCbButton
            // 
            this.YCrCbButton.Location = new System.Drawing.Point(595, 160);
            this.YCrCbButton.Name = "YCrCbButton";
            this.YCrCbButton.Size = new System.Drawing.Size(99, 23);
            this.YCrCbButton.TabIndex = 33;
            this.YCrCbButton.Text = "YCrCb";
            this.YCrCbButton.UseVisualStyleBackColor = true;
            this.YCrCbButton.Click += new System.EventHandler(this.YCrCbButton_Click);
            // 
            // DecimateButton
            // 
            this.DecimateButton.Location = new System.Drawing.Point(599, 292);
            this.DecimateButton.Name = "DecimateButton";
            this.DecimateButton.Size = new System.Drawing.Size(75, 23);
            this.DecimateButton.TabIndex = 46;
            this.DecimateButton.Text = "Decimate";
            this.DecimateButton.UseVisualStyleBackColor = true;
            this.DecimateButton.Click += new System.EventHandler(this.DecimateButton_Click);
            // 
            // DecimateOptionBox
            // 
            this.DecimateOptionBox.FormattingEnabled = true;
            this.DecimateOptionBox.Items.AddRange(new object[] {
            "4:4:4 (no decimation)",
            "4:2:2 (2h1v)",
            "4:1:1 (2h2v)",
            "4:2:2 (1h2v)"});
            this.DecimateOptionBox.Location = new System.Drawing.Point(694, 292);
            this.DecimateOptionBox.Name = "DecimateOptionBox";
            this.DecimateOptionBox.Size = new System.Drawing.Size(121, 21);
            this.DecimateOptionBox.TabIndex = 47;
            // 
            // BoberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 408);
            this.Controls.Add(this.DecimateOptionBox);
            this.Controls.Add(this.DecimateButton);
            this.Controls.Add(this.CbButton);
            this.Controls.Add(this.CrButton);
            this.Controls.Add(this.YButton1);
            this.Controls.Add(this.CbTrack);
            this.Controls.Add(this.CrTrack);
            this.Controls.Add(this.YTrack1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.YCrCbButton);
            this.Controls.Add(this.VButton);
            this.Controls.Add(this.UButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YButton);
            this.Controls.Add(this.GrayscaleButton);
            this.Controls.Add(this.SaveButton2);
            this.Controls.Add(this.OpenButton2);
            this.Controls.Add(this.SaveButton1);
            this.Controls.Add(this.VTrack);
            this.Controls.Add(this.UTrack);
            this.Controls.Add(this.YTrack);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PSNRBox);
            this.Controls.Add(this.PSNRButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Original);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.YUVButton);
            this.Controls.Add(this.OpenButton1);
            this.Controls.Add(this.ModifiedImageBox);
            this.Controls.Add(this.OriginalImageBox);
            this.Name = "BoberForm";
            this.Text = "ImageCompressionStudio";
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModifiedImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrack1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OriginalImageBox;
        private System.Windows.Forms.PictureBox ModifiedImageBox;
        private System.Windows.Forms.Button OpenButton1;
        private System.Windows.Forms.Button YUVButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Label Original;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button PSNRButton;
        private System.Windows.Forms.TextBox PSNRBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar YTrack;
        private System.Windows.Forms.TrackBar UTrack;
        private System.Windows.Forms.TrackBar VTrack;
        private System.Windows.Forms.Button SaveButton1;
        private System.Windows.Forms.Button SaveButton2;
        private System.Windows.Forms.Button OpenButton2;
        private System.Windows.Forms.Button GrayscaleButton;
        private System.Windows.Forms.Button YButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button UButton;
        private System.Windows.Forms.Button VButton;
        private System.Windows.Forms.Button CbButton;
        private System.Windows.Forms.Button CrButton;
        private System.Windows.Forms.Button YButton1;
        private System.Windows.Forms.TrackBar CbTrack;
        private System.Windows.Forms.TrackBar CrTrack;
        private System.Windows.Forms.TrackBar YTrack1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button YCrCbButton;
        private System.Windows.Forms.Button DecimateButton;
        private System.Windows.Forms.ComboBox DecimateOptionBox;
    }
}

