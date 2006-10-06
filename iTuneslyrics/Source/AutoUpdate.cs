using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using iTunesLib;

namespace iTuneslyrics
{
    // delegates used to call MainForm functions from
    //  worker thread
    public delegate int DelegateAddRow(String[] row);
    public delegate void DelegateUpdateRow(int index, Boolean result);

    public partial class frmResult : Form
    {
        private IITTrackCollection m_selectedTracks;
        private org.lyricwiki.LyricWiki m_lyricsWiki;
        private Boolean m_overwrite = false;

        // Delegate instances used to call user interface
        // functions from worker thread:
        public DelegateAddRow m_DelegateAddRow;
        public DelegateUpdateRow m_DelegateUpdateRow;

        public frmResult(IITTrackCollection selectedTracks, org.lyricwiki.LyricWiki lyricsWiki, Boolean overwrite) : this()
        {
            this.m_selectedTracks = selectedTracks;
            this.m_lyricsWiki = lyricsWiki;
            this.m_overwrite = overwrite;
        }

        public frmResult()
        {
            InitializeComponent();

            // initialize delegates
            m_DelegateAddRow =
                 new DelegateAddRow(this.addRow);
            m_DelegateUpdateRow =
                 new DelegateUpdateRow(this.updateRow);
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            LyricsUpdater lu = new LyricsUpdater(m_selectedTracks, m_lyricsWiki, m_overwrite, this);
            ThreadStart threadDelegate = new ThreadStart(lu.UpdateLyrics);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();
        }

        private int addRow(String[] row)
        {
            int index = this.dataGridView1.Rows.Add(row);
            return index;
        }

        private void updateRow(int index, Boolean result)
        {
            if (result == true)
            {
                this.dataGridView1.Rows[index].Cells[2].Value = "Updated";
            }
            else
            {
                this.dataGridView1.Rows[index].Cells[2].Value = "Not Found";
                this.dataGridView1.Rows[index].ErrorText = "No mathcing song found";
            }
        }

    }
}