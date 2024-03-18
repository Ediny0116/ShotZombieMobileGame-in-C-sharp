using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 三鍵反應遊戲
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //變數                   
        public string txt, txtA = "O==", txtB = "=O=", txtC = "==O", txtE = "XXXXXXXXXXXXXXXXXXXX";

        public int txtL, tag, cnt;

        public Random rnd = new Random();

        double DScnt, DTcnt = 0, CCnt;

        public bool miss;

        //教學
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "開始遊戲：按下鍵盤Enter或｢開始｣按鈕\r\n" +
                "數字鍵123：打擊遊戲畫面上的對應位置\r\n\r\n" +
                "遊戲畫面上會出現30個目標｢O｣\r\n" +
                "使用數字鍵123來消除對應目標\r\n" +
                "若打錯目標將會被禁止打擊0.5秒\r\n\r\n" +
                "嘗試在最短時間內消除所有目標！"
                , "玩法", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //鍵盤輸入
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }

            if (miss == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.NumPad1:
                        gameLogic(3);
                        break;
                    case Keys.NumPad2:
                        gameLogic(2);
                        break;
                    case Keys.NumPad3:
                        gameLogic(1);
                        break;
                }
            }
            else
                return;
        }


        //避免聚焦textbox
        private void Form1_Activated(object sender, EventArgs e)
        {
            button4.Focus();
        }

        //開始按鈕
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = txtE;
            button1.Enabled = false;
            button4.Enabled = false;
            CCnt = 0;
            countDown.Enabled = true;
            label3.Text = " 倒數3";
        }

        //倒數計時
        private void countDown_Tick(object sender, EventArgs e)
        {

            miss = true;
            CCnt++;

            switch (CCnt)
            {
                case 1:
                    label3.Text = " 倒數2";
                    break;
                case 2:
                    label3.Text = " 倒數1";
                    start();
                    break;
                case 3:
                    label3.Text = " １ ２ ３";
                    countDown.Enabled = false;
                    miss = false;
                    timerGame.Enabled = true;
                    button5.Enabled = true;
                    button5.Focus();
                    break;
            }
        }

        //開始按鈕
        private void start()
        {
            textBox1.Enabled = true;
            textBox1.Text = Convert.ToString(txtE);

            //生成打擊目標
            for (cnt = 1; cnt <= 30; cnt++)
            {
                int tag = rnd.Next(1, 4);
                switch (tag)
                {
                    case 1:
                        textBox1.Text = textBox1.Text + "\r\n" + Convert.ToString(txtA);
                        break;
                    case 2:
                        textBox1.Text = textBox1.Text + "\r\n" + Convert.ToString(txtB);
                        break;
                    case 3:
                        textBox1.Text = textBox1.Text + "\r\n" + Convert.ToString(txtC);
                        break;
                }
            }

            txtL = textBox1.Text.Length;

            //將畫面移至最下方
            this.textBox1.Select(txtL, 0);                                               //選取到最後一個字元
            this.textBox1.ScrollToCaret();                                               //捲動至選取的位置


            stT = DateTime.Now.TimeOfDay;
            //button4.Enabled = false;

        }


        //結束按鈕
        private void button5_Click(object sender, EventArgs e)
        {
            timerGame.Enabled = false;
            button4.Enabled = true;
            button4.Focus();
            button1.Enabled = true;
            button5.Enabled = false;
        }

        //懲罰計時
        private void timeDelay_Tick(object sender, EventArgs e)
        {
            //button4.Enabled = false;
            txtL = textBox1.Text.Length;

            DTcnt++;

            double BT = 10 - DTcnt;

            DScnt = BT / 10;

            label3.Text = "懲罰" + Convert.ToString(DScnt);


            if (DTcnt >= 5)
            {
                timeDelay.Enabled = false;
                label3.Text = " １ ２ ３";
                miss = false;
            }

        }

        //時間計時
        TimeSpan ST, nT, stT;
        private void timerGame_Tick(object sender, EventArgs e)
        {
            nT = DateTime.Now.TimeOfDay;

            ST = nT - stT;

            double S = Math.Round((double)Convert.ToDouble(ST.TotalMilliseconds) / 1000, 2);

            label4.Text = Convert.ToString(S) + "s";

            if (textBox1.Text.Length == 20)
            {
                timerGame.Enabled = false;
                textBox2.Text = Convert.ToString(S)+"s" + "\r\n"+textBox2.Text;
                button4.Enabled = true;
                button4.Focus();
                button1.Enabled = true;
                button5.Enabled = false;
            }

        }

        //目標判斷
        public void gameLogic(int pos)
        {
            txtL = textBox1.Text.Length;
            txt = textBox1.Text;

            switch (txt.Substring(txtL - pos, 1))
            {
                case "O":
                    miss = false;
                    textBox1.Text = txt.Substring(0, txtL - 5);
                    txtL = textBox1.Text.Length;
                    this.textBox1.Select(txtL, 0);
                    this.textBox1.ScrollToCaret();
                    break;
                case "=":
                    miss = true;
                    DTcnt = 0;
                    timeDelay.Enabled = true;
                    break;
            }

        }

    }
}
