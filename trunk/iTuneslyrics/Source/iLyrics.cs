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

            lyricsWiki = new org.lyricwiki.LyricWiki();

            if (chkAuto.Checked == true)
            {
                frmResult fr = new frmResult(selectedTracks, lyricsWiki, chkOverwrite.Checked);
                fr.ShowDialog();
            }
            else
            {
                for (int i = 1; i <= selectedTracks.Count; i++)
                {
                    ManualUpdate ab = new ManualUpdate();
                    ab.currentTrack = (IITFileOrCDTrack)selectedTracks[i];
                    ab.lyricsWiki = lyricsWiki;
                    DialogResult dr = ab.ShowDialog();
                    if (dr == DialogResult.Abort)
                        break;
                }
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