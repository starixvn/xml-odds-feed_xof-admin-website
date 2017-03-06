namespace CouchbaseApp
{
    partial class CouchbaseN1QL
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
            this.query = new System.Windows.Forms.Button();
            this.store = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // query
            // 
            this.query.Location = new System.Drawing.Point(159, 106);
            this.query.Name = "query";
            this.query.Size = new System.Drawing.Size(75, 23);
            this.query.TabIndex = 0;
            this.query.Text = "Query";
            this.query.UseVisualStyleBackColor = true;
            this.query.Click += new System.EventHandler(this.query_Click);
            // 
            // store
            // 
            this.store.Location = new System.Drawing.Point(40, 106);
            this.store.Name = "store";
            this.store.Size = new System.Drawing.Size(75, 23);
            this.store.TabIndex = 1;
            this.store.Text = "Store";
            this.store.UseVisualStyleBackColor = true;
            this.store.Click += new System.EventHandler(this.store_Click);
            // 
            // CouchbaseN1QL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.store);
            this.Controls.Add(this.query);
            this.Name = "CouchbaseN1QL";
            this.Text = "CouchbaseN1QL";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button query;
        private System.Windows.Forms.Button store;
    }
}