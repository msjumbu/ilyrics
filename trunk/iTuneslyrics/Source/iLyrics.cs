using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iTunesLib;

namespace iTuneslyrics
{
    public partial class iLyrics : Form
    {
        iTunesLib.IiTunes iTunesApp;
        org.lyricwiki.LyricWiki lyricsWiki;
        public iLyrics()
        {
            InitializeComponent();
            iTunesApp = new iTunesLib.iTunesAppClass();
            iTunesApp.BrowserWindow.Visible = true;
            iTunesApp.BrowserWindow.Minimized = false;
        }

        private void btnAlbums_Click(object sender, EventArgs e)
        {
            IITTrackCollection selectedTracks = iTunesApp.SelectedTracks;
            if ((selectedTracks == null))
            {
                MessageBox.Show("Nothing seems to be selected");
                return;
            }

            lyricsWiki = new org.lyricwiki.LyricWiki();

            if (chkAuto.Checked == true)
            {
                frmResult fr = new frmResult(selectedTracks, lyricsWiki, chkOverwrite.Checked);
                fr.ShowDialog();
            }
            else
            {
                int updatedSongsCount = 0;
                for (int i = 1; i <= selectedTracks.Count; i++)
                {
                    IITFileOrCDTrack currentTrack = (IITFileOrCDTrack)selectedTracks[i];
                    if (currentTrack.Lyrics != null)
                        continue;

                    updatedSongsCount++;
                    ManualUpdate ab = new ManualUpdate();
                    ab.currentTrack = currentTrack;
                    ab.lyricsWiki = lyricsWiki;
                    DialogResult dr = ab.ShowDialog();
                    if (dr == DialogResult.Abort)
                        break;
                }
                if (updatedSongsCount == 0)
                    MessageBox.Show("All selected songs seems to have lyrics");
                else
                    MessageBox.Show("Update completed");
            }
        }

        private void chkAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAuto.Checked)
                chkOverwrite.Enabled = true;
            else
                chkOverwrite.Enabled = false;
        }
    }
}