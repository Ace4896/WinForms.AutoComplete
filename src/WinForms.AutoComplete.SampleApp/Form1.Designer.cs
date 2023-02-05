namespace WinForms.AutoComplete.SampleApp
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
            this.comboBoxNoWildcards = new WinForms.AutoComplete.Controls.EasyCompletionComboBox();
            this.comboBoxUseWildcards = new WinForms.AutoComplete.Controls.EasyCompletionComboBox();
            this.comboBoxRegex = new WinForms.AutoComplete.Controls.EasyCompletionComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxAvailableItems = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // comboBoxNoWildcards
            // 
            this.comboBoxNoWildcards.FormattingEnabled = true;
            this.comboBoxNoWildcards.Location = new System.Drawing.Point(123, 69);
            this.comboBoxNoWildcards.Name = "comboBoxNoWildcards";
            this.comboBoxNoWildcards.Size = new System.Drawing.Size(94, 23);
            this.comboBoxNoWildcards.TabIndex = 10;
            // 
            // comboBoxUseWildcards
            // 
            this.comboBoxUseWildcards.FormattingEnabled = true;
            this.comboBoxUseWildcards.Location = new System.Drawing.Point(123, 98);
            this.comboBoxUseWildcards.MatchingMethod = WinForms.AutoComplete.StringMatchingMethod.UseWildcards;
            this.comboBoxUseWildcards.Name = "comboBoxUseWildcards";
            this.comboBoxUseWildcards.Size = new System.Drawing.Size(94, 23);
            this.comboBoxUseWildcards.TabIndex = 11;
            // 
            // comboBoxRegex
            // 
            this.comboBoxRegex.FormattingEnabled = true;
            this.comboBoxRegex.Location = new System.Drawing.Point(123, 127);
            this.comboBoxRegex.MatchingMethod = WinForms.AutoComplete.StringMatchingMethod.UseRegexs;
            this.comboBoxRegex.Name = "comboBoxRegex";
            this.comboBoxRegex.Size = new System.Drawing.Size(94, 23);
            this.comboBoxRegex.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Regex";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "No Wildcards";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Use Wildcards (*)";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "ComboBox Items";
            // 
            // listBoxAvailableItems
            // 
            this.listBoxAvailableItems.FormattingEnabled = true;
            this.listBoxAvailableItems.ItemHeight = 15;
            this.listBoxAvailableItems.Location = new System.Drawing.Point(245, 39);
            this.listBoxAvailableItems.Name = "listBoxAvailableItems";
            this.listBoxAvailableItems.Size = new System.Drawing.Size(188, 154);
            this.listBoxAvailableItems.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 228);
            this.Controls.Add(this.listBoxAvailableItems);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxUseWildcards);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxNoWildcards);
            this.Controls.Add(this.comboBoxRegex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "Form1";
            this.Text = "WinForms.AutoComplete Sample App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.EasyCompletionComboBox comboBoxNoWildcards;
        private Controls.EasyCompletionComboBox comboBoxUseWildcards;
        private Controls.EasyCompletionComboBox comboBoxRegex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxAvailableItems;
    }
}