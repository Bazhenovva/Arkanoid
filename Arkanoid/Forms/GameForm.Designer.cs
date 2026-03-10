using System.ComponentModel;
using Arkanoid.Logic.Game;

namespace Arkanoid.WinForms.Forms;

partial class GameForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        this.components = new System.ComponentModel.Container();
        this.timer = new System.Windows.Forms.Timer(this.components);
        this.SuspendLayout();
        //
        // timer
        //
        this.timer.Interval = 5;
        this.timer.Tick += new System.EventHandler(this.Timer_Tick);
        //
        // GameForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Black;
        this.ClientSize = new System.Drawing.Size(1153, 708);
        this.DoubleBuffered = true;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "GameForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Arkanoid";
        this.KeyPreview = true;
        this.Load += new System.EventHandler(this.Form1_Load);
        this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
        this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Timer timer;
}
