namespace LibTestProgram
{
    partial class TestForm
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
            this.btnTop = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnBottom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTop
            // 
            this.btnTop.Location = new System.Drawing.Point(45, 12);
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(26, 23);
            this.btnTop.TabIndex = 0;
            this.btnTop.Text = "T";
            this.btnTop.UseVisualStyleBackColor = true;
            this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(12, 41);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(26, 23);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.Text = "L";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(45, 41);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(26, 23);
            this.btnNone.TabIndex = 2;
            this.btnNone.Text = "N";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(77, 41);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(26, 23);
            this.btnRight.TabIndex = 3;
            this.btnRight.Text = "R";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnBottom
            // 
            this.btnBottom.Location = new System.Drawing.Point(45, 70);
            this.btnBottom.Name = "btnBottom";
            this.btnBottom.Size = new System.Drawing.Size(26, 23);
            this.btnBottom.TabIndex = 4;
            this.btnBottom.Text = "B";
            this.btnBottom.UseVisualStyleBackColor = true;
            this.btnBottom.Click += new System.EventHandler(this.btnBottom_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 102);
            this.Controls.Add(this.btnBottom);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnTop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestForm";
            this.Text = "Test App";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTop;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBottom;
    }
}

