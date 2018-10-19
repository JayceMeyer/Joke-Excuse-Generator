using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace JokeGenerator
{
    public partial class JokeExcuseGenerator : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        Random rand = new Random();
        int Count = 0;

        public JokeExcuseGenerator()
        {
            InitializeComponent();
            tabControl1.SelectedIndex = Settings.Default.SelectedTab;
            resetSeenJokes();
            resetSeenExcuses();
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.SelectedTab = tabControl1.SelectedIndex;
            Settings.Default.Save();
        }


        #region Jokes Section
        public string getJoke()
        {
            string hereItIs = "";
            int numJokes = Settings.Default.JokeList.Count;
            int indx = rand.Next(0, numJokes);
            bool allSeen = checkAllSeen();

            if (allSeen)
            {
                JokeResult.Hide();
                RestartButton.Show();
                Notification.Text = "You've seen all the jokes!";
                JokeResult.BackColor = Color.BlanchedAlmond;
                JokeButton.Hide();
                Like.Hide();
                Dislike.Hide();
            }
            else
            {
                if (Settings.Default.JokeSeen[indx] == "0")
                {
                    hereItIs = Settings.Default.JokeList[indx].ToString();
                    string jokeVal = Settings.Default.LikeOrDislike[indx].ToString();
                    if (jokeVal == "+")
                    {
                        JokeResult.BackColor = Color.OliveDrab;
                        Notification.Text = "You liked this joke...";
                    }
                    if (jokeVal == "-")
                    {
                        JokeResult.BackColor = Color.IndianRed;
                        Notification.Text = "You disliked this joke...";
                    }
                }
                else
                {
                    hereItIs = findNextUnseen();
                }

                Settings.Default.CurrentJoke = indx;
                Settings.Default.JokeSeen[indx] = "1";
                Settings.Default.Save();
            }
            return hereItIs;
        }

        private void JokeButton_Click(object sender, EventArgs e)
        {
            JokeResult.BackColor = Color.BlanchedAlmond;
            Notification.Text = "";
            string joke = getJoke();
            JokeResult.Text = joke;
        }

        private void Like_Click(object sender, EventArgs e)
        {
            if (JokeResult.Text != "Prepare for LULZ.")
            {
                JokeResult.BackColor = Color.OliveDrab;
                Settings.Default.LikeOrDislike[Settings.Default.CurrentJoke] = "+";
                Settings.Default.Save();
            }
        }

        private void Dislike_Click(object sender, EventArgs e)
        {
            if (JokeResult.Text != "Prepare for LULZ.")
            {
                JokeResult.BackColor = Color.IndianRed;
                Settings.Default.LikeOrDislike[Settings.Default.CurrentJoke] = "-";
                Settings.Default.Save();
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            RestartButton.Hide();
            JokeResult.Show();
            JokeResult.BackColor = Color.BlanchedAlmond;
            Notification.Text = "";
            JokeResult.Text = "Prepare for LULZ.";
            JokeButton.Show();
            Like.Show();
            Dislike.Show();
            Settings.Default.CurrentJoke = 0;
            for (int i = 0; i < Settings.Default.LikeOrDislike.Count; i++)
            {
                Settings.Default.LikeOrDislike[i] = "0";
                Settings.Default.JokeSeen[i] = "0";
                Settings.Default.Save();
            }
            Settings.Default.Save();
        }

        public void resetSeenJokes()
        {
            for (int i = 0; i < Settings.Default.JokeSeen.Count; i++)
            {
                Settings.Default.JokeSeen[i] = "0";
                Settings.Default.Save();
            }
        }

        public bool checkAllSeen()
        {
            for (int i = 0; i < Settings.Default.JokeSeen.Count; i++)
            {
                if (Settings.Default.JokeSeen[i] == "0")
                {
                    return false;
                }
                if ((i + 1 == Settings.Default.JokeSeen.Count) && (Settings.Default.JokeSeen[i] == "1"))
                {
                    return true;
                }
            }
            return false;
        }

        public string findNextUnseen()
        {
            string unseen = "";
            for (int indx = 0; indx < Settings.Default.JokeSeen.Count; indx++)
            {
                if (Settings.Default.JokeSeen[indx] == "0")
                {
                    unseen = Settings.Default.JokeList[indx];
                    Settings.Default.JokeSeen[indx] = "1";
                    Settings.Default.CurrentJoke = indx;
                    Settings.Default.Save();

                    string jokeVal = Settings.Default.LikeOrDislike[indx].ToString();
                    if (jokeVal == "+")
                    {
                        JokeResult.BackColor = Color.OliveDrab;
                        Notification.Text = "You liked this joke...";
                    }
                    if (jokeVal == "-")
                    {
                        JokeResult.BackColor = Color.IndianRed;
                        Notification.Text = "You disliked this joke...";
                    }

                    return unseen;
                }
            }
            return unseen;
        }

        private void RestartButton_Click_1(object sender, EventArgs e)
        {
            resetSeenJokes();
            RestartButton.Hide();
            JokeResult.Show();
            Notification.Text = "";
            JokeResult.Text = "Prepare for LULZ.";
            JokeButton.Show();
            Like.Show();
            Dislike.Show();
            Settings.Default.CurrentJoke = 0;
            Settings.Default.Save();
        }

        #endregion

        #region Excuse Section

        public string getExcuse()
        {
            string hereItIs = "";
            int numExcuses = Settings.Default.ExcuseList.Count;
            int indx = rand.Next(0, numExcuses);
            bool allSeen = checkAllSeenExcuses();

            if (allSeen)
            {
                ExcuseText.Hide();
                RestartExcuses.Show();
                ExcuseNotification.Text = "You've seen all the excuses!";
                ExcuseButton.Hide();
                SendToEmail.Hide();
                CopyButton.Hide();
            }
            else
            {
                if (Settings.Default.ExcuseSeen[indx] == "0")
                {
                    hereItIs = Settings.Default.ExcuseList[indx].ToString();
                }
                else
                {
                    hereItIs = findNextUnseenExcuse();
                }

                Settings.Default.CurrentExcuse = indx;
                Settings.Default.ExcuseSeen[indx] = "1";
                Settings.Default.Save();
            }
            return hereItIs;
        }
        
        string TotExcuses = Settings.Default.ExcuseSeen.Count.ToString();
        private void ExcuseButton_Click(object sender, EventArgs e)
        {
            if (Count > 0)
            {
                BackBox.Items.Insert(0, ExcuseText.Text);
                BackBox.EndUpdate();
            }
            ExcuseNotification.ForeColor = Color.BlanchedAlmond;
            ExcuseNotification.Text = "Excuse #" + (Settings.Default.CurrentExcuse + 1).ToString() + "/" + TotExcuses;
            CopyButton.Show();
            SendToEmail.Show();
            BackButton.Show();
            ForwardButton.Show();
            string excuse = getExcuse();
            ExcuseText.Text = excuse;
            Count += 1;
            ForwardBox.Items.Clear();
            ForwardBox.EndUpdate();
        }

        private void ResetExcuses_Click(object sender, EventArgs e)
        {
            RestartExcuses.Hide();
            ExcuseText.Show();
            ExcuseButton.Show();
            BackButton.Hide();
            ForwardButton.Hide();
            CopyButton.Hide();
            SendToEmail.Hide();
            ExcuseNotification.Text = "";
            ExcuseText.Text = "Excuses Ahead...";
            BackBox.Items.Clear();
            ForwardBox.Items.Clear();
            BackBox.EndUpdate();
            ForwardBox.EndUpdate();
            Count = 0;
            Settings.Default.CurrentExcuse = 0;
            for (int i = 0; i < Settings.Default.ExcuseSeen.Count; i++)
            {
                Settings.Default.ExcuseSeen[i] = "0";
                Settings.Default.Save();
            }
            Settings.Default.Save();
        }

        private void RestartExcuses_Click(object sender, EventArgs e)
        {
            resetSeenExcuses();
            RestartExcuses.Hide();
            ExcuseText.Show();
            ExcuseButton.Show();
            BackButton.Hide();
            ForwardButton.Hide();
            ExcuseNotification.Text = "";
            ExcuseText.Text = "Excuses Ahead...";
            BackBox.Items.Clear();
            ForwardBox.Items.Clear();
            BackBox.EndUpdate();
            ForwardBox.EndUpdate();
            Count = 0;
            Settings.Default.CurrentExcuse = 0;
            Settings.Default.Save();
        }

        public void resetSeenExcuses()
        {
            for (int i = 0; i < Settings.Default.ExcuseSeen.Count; i++)
            {
                Settings.Default.ExcuseSeen[i] = "0";
                Settings.Default.Save();
            }
        }

        public bool checkAllSeenExcuses()
        {
            for (int i = 0; i < Settings.Default.ExcuseSeen.Count; i++)
            {
                if (Settings.Default.ExcuseSeen[i] == "0")
                {
                    return false;
                }
                if ((i + 1 == Settings.Default.ExcuseSeen.Count) && (Settings.Default.ExcuseSeen[i] == "1"))
                {
                    return true;
                }
            }
            return false;
        }

        public string findNextUnseenExcuse()
        {
            string unseen = "";
            for (int indx = 0; indx < Settings.Default.ExcuseSeen.Count; indx++)
            {
                if (Settings.Default.ExcuseSeen[indx] == "0")
                {
                    unseen = Settings.Default.ExcuseList[indx];
                    Settings.Default.ExcuseSeen[indx] = "1";
                    Settings.Default.CurrentExcuse = indx;
                    Settings.Default.Save();

                    return unseen;
                }
            }
            return unseen;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(ExcuseText.Text.ToString());
            ExcuseNotification.Text = "Excuse Copied to Clipboard!";
            ExcuseNotification.ForeColor = Color.LightSalmon;
        }

        private void SendToEmail_Click(object sender, EventArgs e)
        {
            string command = @"mailto:?subject=My Excuse&body=" + ExcuseText.Text.ToString();
            Process.Start(string.Format(command));
            ExcuseNotification.Text = "Excuse Opened in Email!";
            ExcuseNotification.ForeColor = Color.LightSalmon;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (BackBox.Items.Count > 0)
            {
                ExcuseNotification.Text = "";
                ForwardBox.Items.Add(ExcuseText.Text);
                ExcuseText.Text = BackBox.Items[0].ToString();
                BackBox.Items.Remove(ExcuseText.Text);
                BackBox.EndUpdate();
                ForwardBox.EndUpdate();
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            if (ForwardBox.Items.Count != 0)
            {
                ExcuseNotification.Text = "";
                BackBox.Items.Insert(0, ExcuseText.Text);
                ExcuseText.Text = ForwardBox.Items[ForwardBox.Items.Count - 1].ToString();
                ForwardBox.Items.Remove(ExcuseText.Text);
                BackBox.EndUpdate();
                ForwardBox.EndUpdate();
            }
            else
            {
                ExcuseButton.PerformClick();
            }
        }


        #endregion


    }
}
