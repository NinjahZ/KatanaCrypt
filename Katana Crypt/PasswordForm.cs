using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Katana_Crypt
{
    public partial class PasswordForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Password { get; private set; }

        public PasswordForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.PasswordStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // PasswordTextBox
            this.PasswordTextBox.Location = new System.Drawing.Point(12, 12);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(260, 20);
            this.PasswordTextBox.TabIndex = 0;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);

            // SubmitButton
            this.SubmitButton.Location = new System.Drawing.Point(197, 62);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.TabIndex = 1;
            this.SubmitButton.Text = "OK";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);

            // PasswordStatusLabel
            this.PasswordStatusLabel.AutoSize = true;
            this.PasswordStatusLabel.Location = new System.Drawing.Point(12, 38);
            this.PasswordStatusLabel.Name = "PasswordStatusLabel";
            this.PasswordStatusLabel.Size = new System.Drawing.Size(220, 13);
            this.PasswordStatusLabel.TabIndex = 2;
            this.PasswordStatusLabel.Text = "Password must be at least 12 characters.";
            this.PasswordStatusLabel.ForeColor = System.Drawing.Color.Red;

            // PasswordForm
            this.ClientSize = new System.Drawing.Size(284, 97);
            this.Controls.Add(this.PasswordStatusLabel);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Name = "PasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Password";
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Prevent resizing
            this.MaximizeBox = false; // Disable maximize button
            this.MinimizeBox = false; // Disable minimize button
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            this.KeyPress += new KeyPressEventHandler(this.PasswordForm_KeyPress); // For handling Escape key
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            // Set focus on the PasswordTextBox when the form loads
            PasswordTextBox.Focus();
            SubmitButton.Enabled = false;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            // Enable or disable Submit button based on password length
            SubmitButton.Enabled = PasswordTextBox.Text.Length >= 12;

            // Update the password status label with the correct message
            if (PasswordTextBox.Text.Length >= 12)
            {
                PasswordStatusLabel.Text = "Password length is sufficient.";
                PasswordStatusLabel.ForeColor = System.Drawing.Color.Green; // Green color for valid password
            }
            else
            {
                PasswordStatusLabel.Text = "Password must be at least 12 characters.";
                PasswordStatusLabel.ForeColor = System.Drawing.Color.Red; // Red color for invalid password
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            Password = PasswordTextBox.Text;
            this.DialogResult = DialogResult.OK; // Close the form and return OK result
            this.Close();
        }

        private void PasswordForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Close the form on Escape key press
            if (e.KeyChar == 27) // 27 is the ASCII code for the Escape key
            {
                this.DialogResult = DialogResult.Cancel; // Return Cancel result
                this.Close();
            }
        }

        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Label PasswordStatusLabel;
    }
}