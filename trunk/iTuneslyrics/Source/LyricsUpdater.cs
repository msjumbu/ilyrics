using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iTunesLib;

namespace iTuneslyrics
{
    class LyricsUpdater
    {
        private IITTrackCollection m_selectedTracks;
        private org.lyricwiki.LyricWiki m_lyricsWiki;
        private frmResult m_form;
        private Boolean m_overwrite = false;

        public LyricsUpdater(IITTrackCollection selectedTracks, org.lyricwiki.LyricWiki lyricsWiki, Boolean overwrite, frmResult form)
        {
            this.m_selectedTracks = selectedTracks;
            this.m_lyricsWiki = lyricsWiki;
            this.m_overwrite = overwrite;
            this.m_form = form;
        }

        public void UpdateLyrics()
        {
            for (int i = 1; i <= m_selectedTracks.Count; i++)
            {
                IITFileOrCDTrack currentTrack = (IITFileOrCDTrack)m_selectedTracks[i];
                try
                {
                    String artist = currentTrack.Artist;
                    String song = currentTrack.Name;
                    String[] row = { song, artist, "Processing..." };
                    int index = (int)m_form.Invoke(m_form.m_DelegateAddRow, new Object[] { row });

                    if (m_lyricsWiki.checkSongExists(artist, song) == true)
                    {
                        org.lyricwiki.LyricsResult result = m_lyricsWiki.getSong(artist, song);
                        if (m_overwrite || currentTrack.Lyrics == null || currentTrack.Lyrics.Equals("")) 
                            currentTrack.Lyrics = result.lyrics;

                        m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, true });
                    }
                    else
                    {
                        m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, false });
                    }
                }
                catch (Exception e)
                {
                    //throw;
                    MessageBox.Show(e.Message);
                }
            }
            MessageBox.Show("Completed");
        }

    }


}
