using System;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PasiveRadar
{
    public partial class RadarControl : UserControl
    {
        public delegate void MyDelegate(Flags LocalFlags);
        public static event MyDelegate RadarSettings;
        bool SET = false;  //Avoid sending settings to the main program
        uint BufferSize;

        public RadarControl()
        {
            InitializeComponent();
            Form1.FlagsDelegate += new Form1.MyDelegate(Initialize);
        }

        void Initialize(Flags flags)
        {
            SET = false;
            Flags LocalFlags = new Flags();
            LocalFlags.BufferSize = flags.BufferSize;
            LocalFlags.PasiveGain = flags.PasiveGain;
            LocalFlags.DopplerZoom = flags.DopplerZoom;
            LocalFlags.average = flags.average;
            LocalFlags.remove_symetrics = flags.remove_symetrics;
            LocalFlags.TwoDonglesMode = flags.TwoDonglesMode;
            LocalFlags.Rows = flags.Rows;
            LocalFlags.Columns = flags.Columns;
            LocalFlags.OpenCL = flags.OpenCL;
            LocalFlags.DistanceShift = flags.DistanceShift;
            LocalFlags.CorrectBackground = flags.CorrectBackground;
            LocalFlags.NrCorrectionPoints = flags.NrCorrectionPoints;
            LocalFlags.ColectEvery = flags.ColectEvery;
            LocalFlags.CorectionWeight = flags.CorectionWeight;
            LocalFlags.AMDdriver = flags.AMDdriver;
            LocalFlags.TwoDonglesMode = flags.TwoDonglesMode;
            LocalFlags.scale_type = flags.scale_type;
            LocalFlags.Nr_active_radio = flags.Nr_active_radio;
            LocalFlags.alpha = flags.alpha;
            LocalFlags.MaxAverage = flags.MaxAverage;
            LocalFlags.FreezeBackground = flags.FreezeBackground;

            trackBar4.Maximum = (int)LocalFlags.MaxAverage;

            if (LocalFlags.BufferSize == 1024 * 256) comboBox1.SelectedIndex = 0;
            if (LocalFlags.BufferSize == 1024 * 512) comboBox1.SelectedIndex = 1;
            if (LocalFlags.BufferSize == 1024 * 1024) comboBox1.SelectedIndex = 2;
            if (LocalFlags.BufferSize == 1024 * 2048) comboBox1.SelectedIndex = 3;
            if (LocalFlags.BufferSize == 1024 * 4096) comboBox1.SelectedIndex = 4;

            trackBar2.Value = Clamp((int)(LocalFlags.PasiveGain * 10), trackBar2.Minimum, trackBar2.Maximum);
            trackBar3.Value = Clamp((int)LocalFlags.DopplerZoom, trackBar3.Minimum, trackBar3.Maximum);
            trackBar4.Value = Clamp((int)LocalFlags.average, trackBar4.Minimum, trackBar4.Maximum);
            trackBar5.Value = Clamp((int)LocalFlags.Rows, trackBar5.Minimum, trackBar5.Maximum);
            trackBar6.Value = Clamp((int)LocalFlags.DistanceShift, trackBar6.Minimum, trackBar6.Maximum);
            trackBar7.Value = Clamp(LocalFlags.NrCorrectionPoints, trackBar7.Minimum, trackBar7.Maximum);
            trackBar8.Value = Clamp(LocalFlags.ColectEvery, trackBar8.Minimum, trackBar8.Maximum);
            trackBar9.Value = Clamp((int)(LocalFlags.CorectionWeight * 100), trackBar9.Minimum, trackBar9.Maximum);
            trackBar10.Value = Clamp((int)LocalFlags.scale_type, trackBar10.Minimum, trackBar10.Maximum);
            trackBar_alpha.Value = Clamp(LocalFlags.alpha, trackBar_alpha.Minimum, trackBar_alpha.Maximum);
            trackBar1.Value = Clamp((int)LocalFlags.Columns, trackBar1.Minimum, trackBar1.Maximum);

            checkBox4.Checked = LocalFlags.remove_symetrics;
            checkBox3.Checked = LocalFlags.CorrectBackground;
            checkBox1.Checked = LocalFlags.TwoDonglesMode;

            label1.Text = "" + LocalFlags.PasiveGain;
            label2.Text = "" + LocalFlags.DopplerZoom;
            label7.Text = "" + LocalFlags.average;
            label13.Text = "" + LocalFlags.DistanceShift;
            label15.Text = "" + LocalFlags.NrCorrectionPoints;
            label17.Text = "" + LocalFlags.ColectEvery;
            label19.Text = "" + LocalFlags.CorectionWeight;
            label_alpha.Text = "" + LocalFlags.alpha;
            label11.Text = "" + LocalFlags.Columns;
            label12.Text = "" + trackBar5.Value;

            if (LocalFlags.Nr_active_radio < 2)
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }

            MaxMemory(); // Memory protection
			        SetActiveInactiveControls(LocalFlags);
        SET = true;
    }

    public void ActiveDeactivateColumnsControll(bool state)
    {
        // trackBar1.Enabled = state;
    }

    private void SetActiveInactiveControls(Flags LocalFlags)
    {
        // Add logic here if needed
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBox1.SelectedIndex == 0) BufferSize = 1024 * 256;
        if (comboBox1.SelectedIndex == 1) BufferSize = 1024 * 512;
        if (comboBox1.SelectedIndex == 2) BufferSize = 1024 * 1024;
        if (comboBox1.SelectedIndex == 3) BufferSize = 1024 * 2048;
        if (comboBox1.SelectedIndex == 4) BufferSize = 1024 * 4096;

        SendSettings();
    }

    void MaxMemory()
    {
        ulong d = BufferSize * (uint)Clamp(trackBar1.Value, trackBar1.Minimum, trackBar1.Maximum);
        label25.Text = "" + d;

        if (d > 1024L * 1024 * 400)
            label25.ForeColor = System.Drawing.Color.Yellow;
        else
            label25.ForeColor = System.Drawing.Color.Green;

        if (d > 1024L * 1024 * 512)
            label25.ForeColor = System.Drawing.Color.Red;

        if (comboBox1.SelectedIndex == 3)
        {
            if (trackBar1.Value > 255)
            {
                trackBar1.Value = Clamp(255, trackBar1.Minimum, trackBar1.Maximum);
                trackBar1.Maximum = 256;
            }
            if (trackBar5.Value > 2500)
            {
                trackBar5.Value = Clamp(2500, trackBar5.Minimum, trackBar5.Maximum);
                trackBar5.Maximum = 2501;
            }
        }
        else if (comboBox1.SelectedIndex == 4)
        {
            if (trackBar1.Value > 140)
            {
                trackBar1.Value = Clamp(140, trackBar1.Minimum, trackBar1.Maximum);
                trackBar1.Maximum = 141;
            }
            if (trackBar5.Value > 1000)
            {
                trackBar5.Value = Clamp(1000, trackBar5.Minimum, trackBar5.Maximum);
                trackBar5.Maximum = 1001;
            }
        }
        else
        {
            trackBar1.Maximum = 512;
            trackBar5.Maximum = 4000;
        }

        trackBar1.Update();
        trackBar5.Update();
        trackBar1.Refresh();
        trackBar5.Refresh();

        Thread.Sleep(100);
    }

    void SendSettings()
    {
        MaxMemory();
        if (!SET) return;

        Flags LocalFlags = new Flags();
		        LocalFlags.BufferSize = BufferSize;
        LocalFlags.PasiveGain = 0.1f * Clamp(trackBar2.Value, trackBar2.Minimum, trackBar2.Maximum);
        LocalFlags.DopplerZoom = (uint)Clamp(trackBar3.Value, trackBar3.Minimum, trackBar3.Maximum);
        LocalFlags.average = Clamp(trackBar4.Value, trackBar4.Minimum, trackBar4.Maximum);
        LocalFlags.remove_symetrics = checkBox4.Checked;
        LocalFlags.DistanceShift = Clamp(trackBar6.Value, trackBar6.Minimum, trackBar6.Maximum);
        LocalFlags.CorrectBackground = checkBox3.Checked;
        LocalFlags.NrCorrectionPoints = Clamp(trackBar7.Value, trackBar7.Minimum, trackBar7.Maximum);
        LocalFlags.ColectEvery = Clamp(trackBar8.Value, trackBar8.Minimum, trackBar8.Maximum);
        LocalFlags.CorectionWeight = 0.01f * Clamp(trackBar9.Value, trackBar9.Minimum, trackBar9.Maximum);
        LocalFlags.scale_type = (short)Clamp(trackBar10.Value, trackBar10.Minimum, trackBar10.Maximum);
        LocalFlags.TwoDonglesMode = checkBox1.Checked;
        LocalFlags.alpha = (byte)Clamp(trackBar_alpha.Value, trackBar_alpha.Minimum, trackBar_alpha.Maximum);
        LocalFlags.FreezeBackground = checkBoxFreeze.Checked;

        label1.Text = "" + LocalFlags.PasiveGain;
        label2.Text = "" + LocalFlags.DopplerZoom;
        label7.Text = "" + LocalFlags.average;
        label13.Text = "" + LocalFlags.DistanceShift;
        label15.Text = "" + LocalFlags.NrCorrectionPoints;
        label17.Text = "" + LocalFlags.ColectEvery;
        label19.Text = "" + LocalFlags.CorectionWeight;
        label_alpha.Text = "" + LocalFlags.alpha;

        LocalFlags.Columns = (uint)Clamp(trackBar1.Value, trackBar1.Minimum, trackBar1.Maximum);
        LocalFlags.Rows = (uint)Clamp(trackBar5.Value, trackBar5.Minimum, trackBar5.Maximum);

        label11.Text = "" + LocalFlags.Columns;
        label12.Text = "" + LocalFlags.Rows;

        SetActiveInactiveControls(LocalFlags);

        RadarSettings?.Invoke(LocalFlags);
    }

    private void trackBar2_Scroll(object sender, EventArgs e) => SendSettings();
    private void trackBar3_Scroll(object sender, EventArgs e) => SendSettings();
    private void trackBar4_Scroll(object sender, EventArgs e) => SendSettings();
    private void checkBox4_CheckedChanged(object sender, EventArgs e) => SendSettings();
    private void numericUpDown1_ValueChanged(object sender, EventArgs e) => SendSettings(); // columns
    private void numericUpDown2_ValueChanged(object sender, EventArgs e) => SendSettings(); // rows
    private void checkBox1_CheckedChanged(object sender, EventArgs e) => SendSettings(); // background
    private void checkBox2_CheckedChanged(object sender, EventArgs e) => SendSettings(); // background

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        int temp_6value = Clamp(trackBar6.Value, trackBar6.Minimum, trackBar6.Maximum);
        trackBar6.Value = 0;
        trackBar6.Maximum = Clamp(trackBar1.Value - 1, trackBar6.Minimum, trackBar1.Maximum);

        if (trackBar6.Maximum < trackBar1.Value - 1)
        {
            trackBar6.Maximum = trackBar1.Value - 1;
        }

        if (trackBar6.Value > trackBar1.Value - 1)
            trackBar6.Value = trackBar6.Maximum;
        else
            trackBar6.Value = Clamp(temp_6value, trackBar6.Minimum, trackBar6.Maximum);

        SendSettings(); // columns
    }
	    private void trackBar5_Scroll(object sender, EventArgs e) => SendSettings(); // rows
    private void trackBar6_Scroll(object sender, EventArgs e) => SendSettings(); // mix mode
    private void checkBox3_CheckedChanged(object sender, EventArgs e) => SendSettings(); // background
    private void trackBar7_Scroll(object sender, EventArgs e) => SendSettings(); // nr correction points
    private void trackBar8_Scroll(object sender, EventArgs e) => SendSettings(); // collected every
    private void trackBar9_Scroll(object sender, EventArgs e) => SendSettings(); // correction weight
    private void checkBox1_CheckedChanged_1(object sender, EventArgs e) => SendSettings();

    private void label20_Click(object sender, EventArgs e) { }
    private void label13_Click(object sender, EventArgs e) { }
    private void label8_Click(object sender, EventArgs e) { }

    private void trackBar10_Scroll(object sender, EventArgs e) => SendSettings(); // scale type
    private void trackBar_alpha_Scroll(object sender, EventArgs e) => SendSettings(); // alpha
private int Clamp(int value, int min, int max)
{
    return Math.Max(min, Math.Min(value, max));
}
    private void RadarControl_Load(object sender, EventArgs e) { }
    private void trackBar1_MouseUp(object sender, MouseEventArgs e) { }
    private void numericUpDown1_ValueChanged_1(object sender, EventArgs e) { }

    private void checkBoxFreeze_CheckedChanged(object sender, EventArgs e) => SendSettings();
}
}
