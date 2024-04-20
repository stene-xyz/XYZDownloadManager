namespace XYZDownloadManager
{
    partial class AddURLForm
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
            urlBox = new TextBox();
            cancelButton = new Button();
            addURL = new Button();
            SuspendLayout();
            // 
            // urlBox
            // 
            urlBox.Location = new Point(12, 12);
            urlBox.Name = "urlBox";
            urlBox.PlaceholderText = "URL";
            urlBox.Size = new Size(244, 23);
            urlBox.TabIndex = 0;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(181, 41);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // addURL
            // 
            addURL.Location = new Point(100, 41);
            addURL.Name = "addURL";
            addURL.Size = new Size(75, 23);
            addURL.TabIndex = 2;
            addURL.Text = "Add";
            addURL.UseVisualStyleBackColor = true;
            addURL.Click += addURL_Click;
            // 
            // AddURLForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(268, 77);
            Controls.Add(addURL);
            Controls.Add(cancelButton);
            Controls.Add(urlBox);
            Name = "AddURLForm";
            Text = "Add URL";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox urlBox;
        private Button cancelButton;
        private Button addURL;
    }
}