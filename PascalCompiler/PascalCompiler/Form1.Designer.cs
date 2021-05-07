
namespace PascalCompiler
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtInputEditor = new System.Windows.Forms.TextBox();
            this.txtOutputEditor = new System.Windows.Forms.TextBox();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.stStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.semanticStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtInputEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtOutputEditor);
            this.splitContainer1.Size = new System.Drawing.Size(747, 803);
            this.splitContainer1.SplitterDistance = 425;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtInputEditor
            // 
            this.txtInputEditor.Location = new System.Drawing.Point(12, 12);
            this.txtInputEditor.Multiline = true;
            this.txtInputEditor.Name = "txtInputEditor";
            this.txtInputEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInputEditor.Size = new System.Drawing.Size(723, 358);
            this.txtInputEditor.TabIndex = 0;
            // 
            // txtOutputEditor
            // 
            this.txtOutputEditor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtOutputEditor.ForeColor = System.Drawing.SystemColors.Window;
            this.txtOutputEditor.Location = new System.Drawing.Point(12, 12);
            this.txtOutputEditor.Multiline = true;
            this.txtOutputEditor.Name = "txtOutputEditor";
            this.txtOutputEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutputEditor.Size = new System.Drawing.Size(723, 322);
            this.txtOutputEditor.TabIndex = 0;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.executeToolStripMenuItem.Text = "&Execute";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem,
            this.stStripMenuItem,
            this.syntaxStripMenuItem,
            this.semanticStripMenuItem2,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(747, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // stStripMenuItem
            // 
            this.stStripMenuItem.Name = "stStripMenuItem";
            this.stStripMenuItem.Size = new System.Drawing.Size(146, 24);
            this.stStripMenuItem.Text = "&ViewSymbolsTable";
            // 
            // syntaxStripMenuItem
            // 
            this.syntaxStripMenuItem.Name = "syntaxStripMenuItem";
            this.syntaxStripMenuItem.Size = new System.Drawing.Size(115, 24);
            this.syntaxStripMenuItem.Text = "&Syntax Report";
            // 
            // semanticStripMenuItem2
            // 
            this.semanticStripMenuItem2.Name = "semanticStripMenuItem2";
            this.semanticStripMenuItem2.Size = new System.Drawing.Size(133, 24);
            this.semanticStripMenuItem2.Text = "&Semantic Report";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 838);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtInputEditor;
        private System.Windows.Forms.TextBox txtOutputEditor;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem stStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syntaxStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem semanticStripMenuItem2;
    }
}

