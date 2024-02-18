namespace Paint
{
    partial class PaintForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintForm));
            toolStrip = new ToolStrip();
            toolStripFigures = new ToolStripComboBox();
            toolStripButtonToFill = new ToolStripButton();
            toolStripClearButton = new ToolStripButton();
            toolStripDeleteButton = new ToolStripButton();
            toolStripDrawColorButton = new ToolStripButton();
            toolStripFillColorButton = new ToolStripButton();
            colorDialog = new ColorDialog();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripFigures, toolStripButtonToFill, toolStripClearButton, toolStripDeleteButton, toolStripDrawColorButton, toolStripFillColorButton });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1264, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripFigures
            // 
            toolStripFigures.Name = "toolStripFigures";
            toolStripFigures.Size = new Size(121, 25);
            // 
            // toolStripButtonToFill
            // 
            toolStripButtonToFill.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonToFill.Image = (Image)resources.GetObject("toolStripButtonToFill.Image");
            toolStripButtonToFill.ImageTransparentColor = Color.Magenta;
            toolStripButtonToFill.Name = "toolStripButtonToFill";
            toolStripButtonToFill.Size = new Size(56, 22);
            toolStripButtonToFill.Text = "Заливка";
            toolStripButtonToFill.Click += toolStripButtontoFill_Click;
            // 
            // toolStripClearButton
            // 
            toolStripClearButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripClearButton.Image = (Image)resources.GetObject("toolStripClearButton.Image");
            toolStripClearButton.ImageTransparentColor = Color.Magenta;
            toolStripClearButton.Name = "toolStripClearButton";
            toolStripClearButton.Size = new Size(63, 22);
            toolStripClearButton.Text = "Очистить";
            toolStripClearButton.Click += toolStripClearButton_Click;
            // 
            // toolStripDeleteButton
            // 
            toolStripDeleteButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDeleteButton.Enabled = false;
            toolStripDeleteButton.Image = (Image)resources.GetObject("toolStripDeleteButton.Image");
            toolStripDeleteButton.ImageTransparentColor = Color.Magenta;
            toolStripDeleteButton.Name = "toolStripDeleteButton";
            toolStripDeleteButton.Size = new Size(55, 22);
            toolStripDeleteButton.Text = "Удалить";
            toolStripDeleteButton.Click += toolStripDeleteButton_Click;
            // 
            // toolStripDrawColorButton
            // 
            toolStripDrawColorButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDrawColorButton.Image = (Image)resources.GetObject("toolStripDrawColorButton.Image");
            toolStripDrawColorButton.ImageTransparentColor = Color.Magenta;
            toolStripDrawColorButton.Name = "toolStripDrawColorButton";
            toolStripDrawColorButton.Size = new Size(84, 22);
            toolStripDrawColorButton.Text = "Цвет контура";
            toolStripDrawColorButton.Click += toolStripDrawColorButton_Click;
            // 
            // toolStripFillColorButton
            // 
            toolStripFillColorButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripFillColorButton.Image = (Image)resources.GetObject("toolStripFillColorButton.Image");
            toolStripFillColorButton.ImageTransparentColor = Color.Magenta;
            toolStripFillColorButton.Name = "toolStripFillColorButton";
            toolStripFillColorButton.Size = new Size(84, 22);
            toolStripFillColorButton.Text = "Цвет заливки";
            toolStripFillColorButton.Click += toolStripFillColorButton_Click;
            // 
            // PaintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(toolStrip);
            DoubleBuffered = true;
            MaximizeBox = false;
            MaximumSize = new Size(1280, 720);
            MinimumSize = new Size(1280, 720);
            Name = "PaintForm";
            Text = "Form1";
            Paint += PaintForm_Paint;
            MouseDown += PaintForm_MouseDown;
            MouseMove += PaintForm_MouseMove;
            MouseUp += PaintForm_MouseUp;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripComboBox toolStripFigures;
        private ColorDialog colorDialog;
        private ToolStripButton toolStripClearButton;
        private ToolStripButton toolStripDeleteButton;
        private ToolStripButton toolStripButtonToFill;
        private ToolStripButton toolStripDrawColorButton;
        private ToolStripButton toolStripFillColorButton;
    }
}
