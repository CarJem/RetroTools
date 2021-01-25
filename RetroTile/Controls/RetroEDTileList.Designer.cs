﻿namespace RetroTile.Controls
{
    partial class RetroEDTileList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vScrollBar1Old = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // vScrollBar1Old
            // 
            this.vScrollBar1Old.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1Old.LargeChange = 32;
            this.vScrollBar1Old.Location = new System.Drawing.Point(133, 0);
            this.vScrollBar1Old.Name = "vScrollBar1Old";
            this.vScrollBar1Old.Size = new System.Drawing.Size(17, 150);
            this.vScrollBar1Old.SmallChange = 16;
            this.vScrollBar1Old.TabIndex = 0;
            this.vScrollBar1Old.Visible = true;
            this.vScrollBar1Old.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.LargeChange = 32;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 133);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(133, 17);
            this.hScrollBar1.SmallChange = 16;
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Visible = false;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // RetroEDTileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1Old);
            this.DoubleBuffered = true;
            this.Name = "RetroEDTileList";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TileList_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TileList_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TileList_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TileList_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.TileList_MouseWheel);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TileList_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.TileList_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1Old;
        private System.Windows.Forms.HScrollBar hScrollBar1;
    }
}
