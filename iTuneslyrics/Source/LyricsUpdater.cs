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

                String artist = currentTrack.Artist;
                String song = currentTrack.Name;

                if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(song))
                {
                    String[] row = { song, artist, "Processing..." };
                    int index = (int)m_form.Invoke(m_form.m_DelegateAddRow, new Object[] { row });

                    if (currentTrack.Lyrics != null && !m_overwrite)
                    {
                        m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, "skip" });
                        continue;
                    }

                    try
                    {
                        if (m_lyricsWiki.checkSongExists(artist, song) == true)
                        {
                            org.lyricwiki.LyricsResult result = m_lyricsWiki.getSong(artist, song);
                            if (m_overwrite || currentTrack.Lyrics == null)
                            {
                                Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
                                currentTrack.Lyrics = Encoding.UTF8.GetString(iso8859.GetBytes(result.lyrics));
                            }
                            m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, "true" });
                        }
                        else
                        {
                            m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, "false" });
                        }
                    }
                    catch (Exception e)
                    {
                        //throw;
                        MessageBox.Show(e.Message);
                        m_form.Invoke(m_form.m_DelegateUpdateRow, new Object[] { index, "false" });
                    }
                }
            }
            MessageBox.Show("Completed");
        }

    }


}
