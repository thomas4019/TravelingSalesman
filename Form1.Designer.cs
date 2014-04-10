namespace TSP
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSeed = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.lblProblem = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bNewProblem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tbProblemSize = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tbCostOfTour = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tbElapsedTime = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.tbSearched = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.tbPruned = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.tbMaxAgenda = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
            this.tbInitial = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel11 = new System.Windows.Forms.ToolStripLabel();
            this.tbBound = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtSeed,
            this.toolStripLabel5,
            this.lblProblem,
            this.toolStripSeparator3,
            this.btnRun,
            this.toolStripSeparator1,
            this.bNewProblem,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.tbProblemSize,
            this.toolStripSeparator5,
            this.toolStripLabel3,
            this.tbCostOfTour,
            this.toolStripSeparator4,
            this.toolStripLabel4,
            this.tbElapsedTime});
            this.toolStrip1.Location = new System.Drawing.Point(0, 467);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(959, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel1.Text = "Seed";
            // 
            // txtSeed
            // 
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(50, 25);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(49, 22);
            this.toolStripLabel5.Text = "Problem:";
            // 
            // lblProblem
            // 
            this.lblProblem.AutoSize = false;
            this.lblProblem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProblem.Name = "lblProblem";
            this.lblProblem.Size = new System.Drawing.Size(50, 22);
            this.lblProblem.Text = "--";
            this.lblProblem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(30, 22);
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bNewProblem
            // 
            this.bNewProblem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bNewProblem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNewProblem.Name = "bNewProblem";
            this.bNewProblem.Size = new System.Drawing.Size(72, 22);
            this.bNewProblem.Text = "new problem";
            this.bNewProblem.Click += new System.EventHandler(this.bNewProblem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(67, 22);
            this.toolStripLabel2.Text = "Problem Size";
            // 
            // tbProblemSize
            // 
            this.tbProblemSize.Name = "tbProblemSize";
            this.tbProblemSize.Size = new System.Drawing.Size(50, 25);
            this.tbProblemSize.Text = "20";
            this.tbProblemSize.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel3.Text = "Cost of tour";
            // 
            // tbCostOfTour
            // 
            this.tbCostOfTour.Name = "tbCostOfTour";
            this.tbCostOfTour.ReadOnly = true;
            this.tbCostOfTour.Size = new System.Drawing.Size(100, 25);
            this.tbCostOfTour.Text = "--";
            this.tbCostOfTour.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(53, 22);
            this.toolStripLabel4.Text = "Solved in ";
            // 
            // tbElapsedTime
            // 
            this.tbElapsedTime.Name = "tbElapsedTime";
            this.tbElapsedTime.ReadOnly = true;
            this.tbElapsedTime.Size = new System.Drawing.Size(140, 25);
            this.tbElapsedTime.Text = "--";
            this.tbElapsedTime.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.tbSearched,
            this.toolStripLabel7,
            this.tbPruned,
            this.toolStripLabel8,
            this.toolStripLabel9,
            this.tbMaxAgenda,
            this.toolStripLabel10,
            this.tbInitial,
            this.toolStripLabel11,
            this.tbBound});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(959, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(52, 22);
            this.toolStripLabel6.Text = "Searched";
            // 
            // tbSearched
            // 
            this.tbSearched.Name = "tbSearched";
            this.tbSearched.ReadOnly = true;
            this.tbSearched.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel7.Text = "Pruned";
            // 
            // tbPruned
            // 
            this.tbPruned.Name = "tbPruned";
            this.tbPruned.ReadOnly = true;
            this.tbPruned.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(67, 22);
            this.toolStripLabel8.Text = "Max Agenda";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(0, 22);
            // 
            // tbMaxAgenda
            // 
            this.tbMaxAgenda.Name = "tbMaxAgenda";
            this.tbMaxAgenda.ReadOnly = true;
            this.tbMaxAgenda.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new System.Drawing.Size(58, 22);
            this.toolStripLabel10.Text = "Initial Cost";
            // 
            // tbInitial
            // 
            this.tbInitial.Name = "tbInitial";
            this.tbInitial.ReadOnly = true;
            this.tbInitial.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel11
            // 
            this.toolStripLabel11.Name = "toolStripLabel11";
            this.toolStripLabel11.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel11.Text = "Bound";
            // 
            // tbBound
            // 
            this.tbBound.Name = "tbBound";
            this.tbBound.ReadOnly = true;
            this.tbBound.Size = new System.Drawing.Size(100, 25);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 492);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "zz";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtSeed;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.ToolStripButton bNewProblem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox tbProblemSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        public System.Windows.Forms.ToolStripTextBox tbCostOfTour;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        public System.Windows.Forms.ToolStripTextBox tbElapsedTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel lblProblem;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        public System.Windows.Forms.ToolStripTextBox tbSearched;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        public System.Windows.Forms.ToolStripTextBox tbPruned;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        public System.Windows.Forms.ToolStripTextBox tbMaxAgenda;
        private System.Windows.Forms.ToolStripLabel toolStripLabel10;
        public System.Windows.Forms.ToolStripTextBox tbInitial;
        private System.Windows.Forms.ToolStripLabel toolStripLabel11;
        public System.Windows.Forms.ToolStripTextBox tbBound;



    }
}

