namespace XYZDownloadManager
{
    partial class ErrorDialog
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
            label1 = new Label();
            errorText = new Label();
            okButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(124, 15);
            label1.TabIndex = 0;
            label1.Text = "An error has occurred!";
            // 
            // errorText
            // 
            errorText.AutoSize = true;
            errorText.Location = new Point(12, 24);
            errorText.Name = "errorText";
            errorText.Size = new Size(81, 15);
            errorText.TabIndex = 1;
            errorText.Text = "Error text here";
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 42);
            okButton.Name = "okButton";
            okButton.Size = new Size(311, 23);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // ErrorDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 76);
            Controls.Add(okButton);
            Controls.Add(errorText);
            Controls.Add(label1);
            Name = "ErrorDialog";
            Text = "Error";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label errorText;
        private Button okButton;
    }
}