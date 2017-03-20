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
            this.store = new System.Windows.Forms.Button();
            this.storeLeague = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // store
            // 
            this.store.Location = new System.Drawing.Point(28, 98);
            this.store.Name = "store";
            this.store.Size = new System.Drawing.Size(75, 23);
            this.store.TabIndex = 1;
            this.store.Text = "Store";
            this.store.UseVisualStyleBackColor = true;
            this.store.Click += new System.EventHandler(this.store_Click);
            // 
            // storeLeague
            // 
            this.storeLeague.Location = new System.Drawing.Point(164, 98);
            this.storeLeague.Name = "storeLeague";
            this.storeLeague.Size = new System.Drawing.Size(110, 23);
            this.storeLeague.TabIndex = 2;
            this.storeLeague.Text = "Store League";
            this.storeLeague.UseVisualStyleBackColor = true;
            this.storeLeague.Click += new System.EventHandler(this.storeLeague_Click);
            // 
            // CouchbaseN1QL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 261);
            this.Controls.Add(this.storeLeague);
            this.Controls.Add(this.store);
            this.Name = "CouchbaseN1QL";
            this.Text = "CouchbaseN1QL";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button store;
        private System.Windows.Forms.Button storeLeague;
    }
}