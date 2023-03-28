using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;  // DllImport
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions; // PermissionSetAttribute
using System.IO;
using System.Linq;

namespace Impersonate
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDomain;
        private Impersonator newUser;
        private RichTextBox txtFilesList;
        private Label lblParentFolder;
        private TextBox txtSelectedFolder;
        private Button btnBrowse;
        private Label label6;
        private Button buttonRevert;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            // set logged on username
            lblCurrentUser.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            // populate logon domain name
            string sTempUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (sTempUser.IndexOf("\\") != -1)
            {
                string[] aryUser = new String[2];
                char[] splitter = { '\\' };
                aryUser = sTempUser.Split(splitter);
                textBoxDomain.Text = aryUser[0];
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogon = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDomain = new System.Windows.Forms.TextBox();
            this.txtFilesList = new System.Windows.Forms.RichTextBox();
            this.lblParentFolder = new System.Windows.Forms.Label();
            this.txtSelectedFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonRevert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Running As:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.Location = new System.Drawing.Point(156, 37);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(280, 24);
            this.lblCurrentUser.TabIndex = 1;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(156, 123);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(280, 27);
            this.textBoxUsername.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "New Username:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "New Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(156, 160);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(280, 27);
            this.textBoxPassword.TabIndex = 4;
            // 
            // buttonLogon
            // 
            this.buttonLogon.Location = new System.Drawing.Point(330, 256);
            this.buttonLogon.Name = "buttonLogon";
            this.buttonLogon.Size = new System.Drawing.Size(105, 35);
            this.buttonLogon.TabIndex = 6;
            this.buttonLogon.Text = "Logon";
            this.buttonLogon.Click += new System.EventHandler(this.buttonLogon_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "Logon Domain:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDomain
            // 
            this.textBoxDomain.Location = new System.Drawing.Point(156, 87);
            this.textBoxDomain.Name = "textBoxDomain";
            this.textBoxDomain.Size = new System.Drawing.Size(280, 27);
            this.textBoxDomain.TabIndex = 7;
            // 
            // txtFilesList
            // 
            this.txtFilesList.Location = new System.Drawing.Point(5, 325);
            this.txtFilesList.Name = "txtFilesList";
            this.txtFilesList.Size = new System.Drawing.Size(431, 202);
            this.txtFilesList.TabIndex = 10;
            this.txtFilesList.Text = "";
            // 
            // lblParentFolder
            // 
            this.lblParentFolder.AutoSize = true;
            this.lblParentFolder.Location = new System.Drawing.Point(14, 300);
            this.lblParentFolder.Name = "lblParentFolder";
            this.lblParentFolder.Size = new System.Drawing.Size(0, 20);
            this.lblParentFolder.TabIndex = 11;
            // 
            // txtSelectedFolder
            // 
            this.txtSelectedFolder.Location = new System.Drawing.Point(156, 201);
            this.txtSelectedFolder.Name = "txtSelectedFolder";
            this.txtSelectedFolder.Size = new System.Drawing.Size(202, 27);
            this.txtSelectedFolder.TabIndex = 13;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(365, 203);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 30);
            this.btnBrowse.TabIndex = 14;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(14, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 24);
            this.label6.TabIndex = 15;
            this.label6.Text = "Browse:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRevert
            // 
            this.buttonRevert.Enabled = false;
            this.buttonRevert.Location = new System.Drawing.Point(219, 256);
            this.buttonRevert.Name = "buttonRevert";
            this.buttonRevert.Size = new System.Drawing.Size(105, 35);
            this.buttonRevert.TabIndex = 16;
            this.buttonRevert.Text = "Revert";
            this.buttonRevert.Click += new System.EventHandler(this.buttonRevert_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 20);
            this.ClientSize = new System.Drawing.Size(453, 538);
            this.Controls.Add(this.buttonRevert);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSelectedFolder);
            this.Controls.Add(this.lblParentFolder);
            this.Controls.Add(this.txtFilesList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDomain);
            this.Controls.Add(this.buttonLogon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.lblCurrentUser);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Impersonation Example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }

        private void buttonLogon_Click(object sender, System.EventArgs e)
        {
            if (textBoxUsername.Text != "" && textBoxPassword.Text != "")
            {
                if (string.IsNullOrEmpty(txtSelectedFolder.Text))
                {
                    MessageBox.Show(this, "Chose a folder first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    // attempt to impersonate specified user
                    using (var _newUser = new Impersonator(textBoxUsername.Text, textBoxDomain.Text, textBoxPassword.Text, () =>
                    {
                        lblCurrentUser.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        buttonLogon.Enabled = false;
                        buttonRevert.Enabled = true;
                        //Load files name of selected folder
                        ListFiles(txtSelectedFolder.Text);
                    })){}
                }
                catch (Exception ex)
                {
                    // why did it fail?
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show(this, "Complete all the logon credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void FolderHandler()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    lblParentFolder.Text = "Selected folder:" + Path.GetFileName(fbd.SelectedPath);
                    txtSelectedFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void ListFiles(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            txtFilesList.Lines = files.Select(a=>Path.GetFileName(a)).ToArray();
        }
        /// <summary>
        /// Revert back to previous user
        /// </summary>
        private void buttonRevert_Click(object sender, System.EventArgs e)
        {
            txtSelectedFolder.Text = "";
            txtFilesList.Clear();
            lblParentFolder.Text = "";
            buttonRevert.Enabled = false;
            buttonLogon.Enabled = true;
            // update the running as name
            lblCurrentUser.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderHandler();
        }
    }
}
