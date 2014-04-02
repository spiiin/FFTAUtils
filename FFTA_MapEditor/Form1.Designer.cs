namespace FFTA_MapEditor
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.cboMapIndex = new System.Windows.Forms.ComboBox();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.chkHeightCompressed = new System.Windows.Forms.CheckBox();
            this.chkHeightPacked = new System.Windows.Forms.CheckBox();
            this.picPalette = new System.Windows.Forms.PictureBox();
            this.chkPalCompressed = new System.Windows.Forms.CheckBox();
            this.cboTileCompression = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picTiles = new System.Windows.Forms.PictureBox();
            this.chkShowBottom = new System.Windows.Forms.CheckBox();
            this.chkShowTop = new System.Windows.Forms.CheckBox();
            this.chkShowHeight = new System.Windows.Forms.CheckBox();
            this.tmrAnimate = new System.Windows.Forms.Timer(this.components);
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.picHmapEdit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPalette)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHmapEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // cboMapIndex
            // 
            this.cboMapIndex.FormattingEnabled = true;
            this.cboMapIndex.Items.AddRange(new object[] {
            "School Yard (Day)",
            "School Yard (Night)",
            "Real World (Day)",
            "Real World (Night)",
            "Players Room (Day)",
            "Players Room (Night)",
            "Jagd Dorsa (Day)",
            "Jagd Dorsa (Night)",
            "Jagd Helje (Day)",
            "Jagd Helje (Night)",
            "Carrot\'s Lair",
            "000B",
            "000C",
            "Magewyrm\'s lair",
            "Dread Lord\'s tomb",
            "Jagd Ahli (Shadow Clan)",
            "0010",
            "0011",
            "Jagd Ahli",
            "Totema Map (5-2)",
            "Totema Map (3)",
            "Totema Map (1)",
            "Totema Map (4)",
            "Totema Map (2)",
            "Totema Map (5-1)",
            "0019",
            "Bervenia Palace (waiting room)",
            "Bervenia Palace (throne room)",
            "Bervenia arena",
            "001D",
            "001E",
            "001F",
            "Deti Plains (To Ambervale)",
            "Siena Gorge (Over the Hill)",
            "0022",
            "0023",
            "Royal Valley (exterior)",
            "Royal Valley (interior)",
            "Cyril",
            "Bervenia (night)",
            "Cyril (Staring Eyes)",
            "Cyril (night)",
            "Baguba (Free Baguba)",
            "Baguba Port (night)",
            "Baguba (Hero Blade)",
            "Baguba Port 2 (night)",
            "Sprohm",
            "Sprohm (Free Sprohm)",
            "0030",
            "0031",
            "0032",
            "Cadoan (night)",
            "0034",
            "Cadoan (night 2)",
            "Muscadet",
            "Muscadet (night)",
            "No Answers",
            "Muscadet (Free Muscadet)",
            "Tubola Cave (Missing Prof)",
            "Tubola Cave path to Helje",
            "003C",
            "Tubola Cave (Hidden Vein)",
            "Pub",
            "003F",
            "Sprohm Prison",
            "0041",
            "0042",
            "Card Shop",
            "Giza Plains",
            "Field at night (Fowl Thief)",
            "Giza Plains (Herb Picking)",
            "Aisenfield (night)",
            "Ozmonfield",
            "Deti Plains (night)",
            "Aisenfield (Diamond Rain)",
            "Ozmonfield (night)",
            "The Bounty",
            "Deti Plains w/ gap (Diaghilev)",
            "Koringwood (Day)",
            "Koringwood (Night)",
            "Siena Gorge (Day)",
            "Siena Gorge (Night)",
            "Deti Plains (Day)",
            "Deti Plains (Night) 2",
            "Lutia Pass",
            "No Jumping",
            "Lutia Pass (Thesis Hunt)",
            "0057",
            "Koringwood (Magic Wood)",
            "Salikawood (night)",
            "Nubswood (The Cheetahs)",
            "Nubswood w/ river (night)",
            "Nubswood",
            "Nubswood (night)",
            "Salikawood",
            "Salikawood (night) 2",
            "Materi Wood (Day)",
            "Materi Wood (Night)",
            "Eluut Sands w/ high plateaus",
            "Eluut (night)",
            "Eluut Sands",
            "Delia Dunes (Challengers)",
            "Jeraw Sands (Golden Clock)",
            "Jeraw Sands (Gabanna)",
            "Gotor Sands (Desert Patrol)",
            "Gotor Sands (Desert Rose)",
            "Delia Dunes",
            "Delia Dunes path to Ozmonfield",
            "Uladon Bog",
            "Uladon Bog (Kanan)",
            "Ulei River",
            "Baguba wetlands",
            "Uladon Bog 2",
            "Uladon Bog w/ bridges (Foreign Fiend 3)",
            "Ulei (A Lost Ring)",
            "0073",
            "Ulei River w/ bridges (Foreign Fiend 1)",
            "No Literacy",
            "Kudik Peaks",
            "Lutia (snowy pond)",
            "Lutia (snowy-2)",
            "Kudik Peaks flatland",
            "Deti Plains w/ snow (Sorry, Friend)",
            "007B",
            "Deti Plains (snow)",
            "Lutia (snowy area)",
            "Lutia (snowy)",
            "Mortal Snow",
            "0080",
            "Nargai Cave",
            "Tubola Cave",
            "Nargai Cave pyramid-shaped (Water Sigil)",
            "Nargai ruins",
            "Salikawood (Emerald Keep)",
            "0086",
            "Aisenfield ruins",
            "0088",
            "Siena Gorge (Fey Blade)",
            "Cursed Bride map",
            "Cadoan outskirts (Cadoan Watch)",
            "008C",
            "Gotor Sands path to Ahli",
            "Salika Keep ruins",
            "Bervenia coast",
            "0090",
            "0091",
            "0092",
            "Gotor Sands",
            "Wasteland",
            "Jeraw Sands",
            "Aisenfield",
            "Roda Volcano (Hot Recipe)",
            "Roda Volcano",
            "Roda Volcano (Fire Sigil)",
            "Roda Volcano lavafalls",
            "Hospital (End Game)",
            "Materiwood (Moogle Bride)",
            "Materiwood (Materite Now)",
            "Immunity Pass",
            "Favoritism",
            "00A0",
            "With Babus"});
            this.cboMapIndex.Location = new System.Drawing.Point(12, 12);
            this.cboMapIndex.Name = "cboMapIndex";
            this.cboMapIndex.Size = new System.Drawing.Size(137, 21);
            this.cboMapIndex.TabIndex = 0;
            this.cboMapIndex.SelectedIndexChanged += new System.EventHandler(this.cboMapIndex_SelectedIndexChanged);
            // 
            // picMain
            // 
            this.picMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picMain.Location = new System.Drawing.Point(12, 86);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(556, 493);
            this.picMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMain.TabIndex = 1;
            this.picMain.TabStop = false;
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            // 
            // chkHeightCompressed
            // 
            this.chkHeightCompressed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHeightCompressed.AutoSize = true;
            this.chkHeightCompressed.Enabled = false;
            this.chkHeightCompressed.Location = new System.Drawing.Point(12, 585);
            this.chkHeightCompressed.Name = "chkHeightCompressed";
            this.chkHeightCompressed.Size = new System.Drawing.Size(118, 17);
            this.chkHeightCompressed.TabIndex = 2;
            this.chkHeightCompressed.Text = "Height Compressed";
            this.chkHeightCompressed.UseVisualStyleBackColor = true;
            // 
            // chkHeightPacked
            // 
            this.chkHeightPacked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHeightPacked.AutoSize = true;
            this.chkHeightPacked.Enabled = false;
            this.chkHeightPacked.Location = new System.Drawing.Point(136, 585);
            this.chkHeightPacked.Name = "chkHeightPacked";
            this.chkHeightPacked.Size = new System.Drawing.Size(97, 17);
            this.chkHeightPacked.TabIndex = 3;
            this.chkHeightPacked.Text = "Height Packed";
            this.chkHeightPacked.UseVisualStyleBackColor = true;
            // 
            // picPalette
            // 
            this.picPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPalette.Location = new System.Drawing.Point(961, 39);
            this.picPalette.Name = "picPalette";
            this.picPalette.Size = new System.Drawing.Size(130, 80);
            this.picPalette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picPalette.TabIndex = 4;
            this.picPalette.TabStop = false;
            // 
            // chkPalCompressed
            // 
            this.chkPalCompressed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPalCompressed.AutoSize = true;
            this.chkPalCompressed.Enabled = false;
            this.chkPalCompressed.Location = new System.Drawing.Point(239, 585);
            this.chkPalCompressed.Name = "chkPalCompressed";
            this.chkPalCompressed.Size = new System.Drawing.Size(120, 17);
            this.chkPalCompressed.TabIndex = 5;
            this.chkPalCompressed.Text = "Palette Compressed";
            this.chkPalCompressed.UseVisualStyleBackColor = true;
            // 
            // cboTileCompression
            // 
            this.cboTileCompression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboTileCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTileCompression.Enabled = false;
            this.cboTileCompression.FormattingEnabled = true;
            this.cboTileCompression.Items.AddRange(new object[] {
            "Uncompressed Tiles ",
            "LZ77 Compressed Tiles",
            "LZSS Compressed Tiles"});
            this.cboTileCompression.Location = new System.Drawing.Point(365, 583);
            this.cboTileCompression.Name = "cboTileCompression";
            this.cboTileCompression.Size = new System.Drawing.Size(147, 21);
            this.cboTileCompression.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picTiles);
            this.panel1.Location = new System.Drawing.Point(12, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 41);
            this.panel1.TabIndex = 7;
            // 
            // picTiles
            // 
            this.picTiles.Location = new System.Drawing.Point(0, 0);
            this.picTiles.Name = "picTiles";
            this.picTiles.Size = new System.Drawing.Size(100, 24);
            this.picTiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTiles.TabIndex = 0;
            this.picTiles.TabStop = false;
            this.picTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.picTiles_Paint);
            this.picTiles.MouseLeave += new System.EventHandler(this.picTiles_MouseLeave);
            this.picTiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTiles_MouseMove);
            this.picTiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTiles_MouseUp);
            // 
            // chkShowBottom
            // 
            this.chkShowBottom.AutoSize = true;
            this.chkShowBottom.Checked = true;
            this.chkShowBottom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowBottom.Location = new System.Drawing.Point(155, 14);
            this.chkShowBottom.Name = "chkShowBottom";
            this.chkShowBottom.Size = new System.Drawing.Size(88, 17);
            this.chkShowBottom.TabIndex = 9;
            this.chkShowBottom.Text = "Bottom Layer";
            this.chkShowBottom.UseVisualStyleBackColor = true;
            this.chkShowBottom.CheckedChanged += new System.EventHandler(this.chkShowLayer_CheckedChanged);
            // 
            // chkShowTop
            // 
            this.chkShowTop.AutoSize = true;
            this.chkShowTop.Checked = true;
            this.chkShowTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowTop.Location = new System.Drawing.Point(249, 14);
            this.chkShowTop.Name = "chkShowTop";
            this.chkShowTop.Size = new System.Drawing.Size(74, 17);
            this.chkShowTop.TabIndex = 10;
            this.chkShowTop.Text = "Top Layer";
            this.chkShowTop.UseVisualStyleBackColor = true;
            this.chkShowTop.CheckedChanged += new System.EventHandler(this.chkShowLayer_CheckedChanged);
            // 
            // chkShowHeight
            // 
            this.chkShowHeight.AutoSize = true;
            this.chkShowHeight.Location = new System.Drawing.Point(329, 14);
            this.chkShowHeight.Name = "chkShowHeight";
            this.chkShowHeight.Size = new System.Drawing.Size(86, 17);
            this.chkShowHeight.TabIndex = 11;
            this.chkShowHeight.Text = "Height Layer";
            this.chkShowHeight.UseVisualStyleBackColor = true;
            this.chkShowHeight.CheckedChanged += new System.EventHandler(this.chkShowLayer_CheckedChanged);
            // 
            // tmrAnimate
            // 
            this.tmrAnimate.Interval = 140;
            this.tmrAnimate.Tick += new System.EventHandler(this.tmrAnimate_Tick);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(902, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 13;
            this.btnExport.Text = "Export Map";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPlay.Image = global::FFTA_MapEditor.Properties.Resources.play;
            this.btnPlay.Location = new System.Drawing.Point(862, 10);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(34, 23);
            this.btnPlay.TabIndex = 14;
            this.btnPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(986, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Export CadEditor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // picHmapEdit
            // 
            this.picHmapEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHmapEdit.Location = new System.Drawing.Point(568, 86);
            this.picHmapEdit.Name = "picHmapEdit";
            this.picHmapEdit.Size = new System.Drawing.Size(523, 493);
            this.picHmapEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picHmapEdit.TabIndex = 16;
            this.picHmapEdit.TabStop = false;
            this.picHmapEdit.Paint += new System.Windows.Forms.PaintEventHandler(this.picHmapEdit_Paint);
            this.picHmapEdit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picHmapEdit_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 610);
            this.Controls.Add(this.picHmapEdit);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.chkShowHeight);
            this.Controls.Add(this.chkShowTop);
            this.Controls.Add(this.chkShowBottom);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboTileCompression);
            this.Controls.Add(this.chkPalCompressed);
            this.Controls.Add(this.picPalette);
            this.Controls.Add(this.chkHeightPacked);
            this.Controls.Add(this.chkHeightCompressed);
            this.Controls.Add(this.picMain);
            this.Controls.Add(this.cboMapIndex);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPalette)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHmapEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMapIndex;
        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.CheckBox chkHeightCompressed;
        private System.Windows.Forms.CheckBox chkHeightPacked;
        private System.Windows.Forms.PictureBox picPalette;
        private System.Windows.Forms.CheckBox chkPalCompressed;
        private System.Windows.Forms.ComboBox cboTileCompression;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picTiles;
        private System.Windows.Forms.CheckBox chkShowBottom;
        private System.Windows.Forms.CheckBox chkShowTop;
        private System.Windows.Forms.CheckBox chkShowHeight;
        private System.Windows.Forms.Timer tmrAnimate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox btnPlay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox picHmapEdit;
    }
}

