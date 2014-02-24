namespace RenderArrangeMap
{
    partial class FormRenderMap
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
            this.components = new System.ComponentModel.Container();
            this.pbMap = new System.Windows.Forms.PictureBox();
            this.ilPictures = new System.Windows.Forms.ImageList(this.components);
            this.cbLayer1 = new System.Windows.Forms.CheckBox();
            this.cbLayer2 = new System.Windows.Forms.CheckBox();
            this.cbLayer3 = new System.Windows.Forms.CheckBox();
            this.btExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMap
            // 
            this.pbMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMap.Location = new System.Drawing.Point(0, 0);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new System.Drawing.Size(512, 512);
            this.pbMap.TabIndex = 0;
            this.pbMap.TabStop = false;
            this.pbMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMap_Paint);
            // 
            // ilPictures
            // 
            this.ilPictures.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilPictures.ImageSize = new System.Drawing.Size(16, 8);
            this.ilPictures.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cbLayer1
            // 
            this.cbLayer1.AutoSize = true;
            this.cbLayer1.Checked = true;
            this.cbLayer1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLayer1.Location = new System.Drawing.Point(22, 12);
            this.cbLayer1.Name = "cbLayer1";
            this.cbLayer1.Size = new System.Drawing.Size(88, 17);
            this.cbLayer1.TabIndex = 2;
            this.cbLayer1.Text = "Show Layer1";
            this.cbLayer1.UseVisualStyleBackColor = true;
            this.cbLayer1.CheckedChanged += new System.EventHandler(this.cbLayer1_CheckedChanged);
            // 
            // cbLayer2
            // 
            this.cbLayer2.AutoSize = true;
            this.cbLayer2.Checked = true;
            this.cbLayer2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLayer2.Location = new System.Drawing.Point(22, 35);
            this.cbLayer2.Name = "cbLayer2";
            this.cbLayer2.Size = new System.Drawing.Size(88, 17);
            this.cbLayer2.TabIndex = 3;
            this.cbLayer2.Text = "Show Layer2";
            this.cbLayer2.UseVisualStyleBackColor = true;
            this.cbLayer2.CheckedChanged += new System.EventHandler(this.cbLayer1_CheckedChanged);
            // 
            // cbLayer3
            // 
            this.cbLayer3.AutoSize = true;
            this.cbLayer3.Location = new System.Drawing.Point(22, 58);
            this.cbLayer3.Name = "cbLayer3";
            this.cbLayer3.Size = new System.Drawing.Size(88, 17);
            this.cbLayer3.TabIndex = 4;
            this.cbLayer3.Text = "Show Layer3";
            this.cbLayer3.UseVisualStyleBackColor = true;
            this.cbLayer3.CheckedChanged += new System.EventHandler(this.cbLayer1_CheckedChanged);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(22, 81);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(75, 23);
            this.btExport.TabIndex = 5;
            this.btExport.Text = "Export";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // FormRenderMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 517);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.cbLayer3);
            this.Controls.Add(this.cbLayer1);
            this.Controls.Add(this.cbLayer2);
            this.Controls.Add(this.pbMap);
            this.Name = "FormRenderMap";
            this.Text = "Render Map";
            this.Load += new System.EventHandler(this.FormRenderMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbMap;
        private System.Windows.Forms.ImageList ilPictures;
        private System.Windows.Forms.CheckBox cbLayer1;
        private System.Windows.Forms.CheckBox cbLayer2;
        private System.Windows.Forms.CheckBox cbLayer3;
        private System.Windows.Forms.Button btExport;
    }
}

