namespace XYZDownloadManager
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
            components = new System.ComponentModel.Container();
            downloadListView = new ListView();
            groupBox1 = new GroupBox();
            progressText = new Label();
            progressBar = new ProgressBar();
            aboutButton = new Button();
            deleteURLButton = new Button();
            addURLButton = new Button();
            refreshTimer = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // downloadListView
            // 
            downloadListView.Activation = ItemActivation.OneClick;
            downloadListView.HoverSelection = true;
            downloadListView.LabelWrap = false;
            downloadListView.Location = new Point(6, 22);
            downloadListView.MultiSelect = false;
            downloadListView.Name = "downloadListView";
            downloadListView.Size = new Size(323, 186);
            downloadListView.TabIndex = 1;
            downloadListView.UseCompatibleStateImageBehavior = false;
            downloadListView.View = View.List;
            downloadListView.SelectedIndexChanged += downloadListView_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(progressText);
            groupBox1.Controls.Add(progressBar);
            groupBox1.Controls.Add(aboutButton);
            groupBox1.Controls.Add(deleteURLButton);
            groupBox1.Controls.Add(addURLButton);
            groupBox1.Controls.Add(downloadListView);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(335, 286);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Downloads List";
            // 
            // progressText
            // 
            progressText.AutoSize = true;
            progressText.Location = new Point(6, 268);
            progressText.Name = "progressText";
            progressText.Size = new Size(91, 15);
            progressText.TabIndex = 6;
            progressText.Text = "No File Selected";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(6, 243);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(323, 23);
            progressBar.TabIndex = 5;
            // 
            // aboutButton
            // 
            aboutButton.Location = new Point(254, 214);
            aboutButton.Name = "aboutButton";
            aboutButton.Size = new Size(75, 23);
            aboutButton.TabIndex = 4;
            aboutButton.Text = "About";
            aboutButton.UseVisualStyleBackColor = true;
            aboutButton.Click += aboutButton_Click;
            // 
            // deleteURLButton
            // 
            deleteURLButton.Location = new Point(87, 214);
            deleteURLButton.Name = "deleteURLButton";
            deleteURLButton.Size = new Size(75, 23);
            deleteURLButton.TabIndex = 3;
            deleteURLButton.Text = "Delete URL";
            deleteURLButton.UseVisualStyleBackColor = true;
            deleteURLButton.Click += deleteURLButton_Click;
            // 
            // addURLButton
            // 
            addURLButton.Location = new Point(6, 214);
            addURLButton.Name = "addURLButton";
            addURLButton.Size = new Size(75, 23);
            addURLButton.TabIndex = 2;
            addURLButton.Text = "Add URL";
            addURLButton.UseVisualStyleBackColor = true;
            addURLButton.Click += addURLButton_Click;
            // 
            // refreshTimer
            // 
            refreshTimer.Enabled = true;
            refreshTimer.Tick += refreshTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 310);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "XYZDownloadManager";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView downloadListView;
        private GroupBox groupBox1;
        private Button deleteURLButton;
        private Button addURLButton;
        private Button aboutButton;
        private Label progressText;
        private ProgressBar progressBar;
        private System.Windows.Forms.Timer refreshTimer;
    }
}
