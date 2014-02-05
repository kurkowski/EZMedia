using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using EZMedia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace TestSuite
{
    [TestClass]
    public class TestPlaylistClass
    {
        [TestMethod]
        public void TestInitialization()
        {
            EZPlaylist play = new EZPlaylist("YUP");
            Assert.IsTrue(play.Name == "YUP");
        }

        [TestMethod]
        public void TestInitializationSanitation()
        {
            //Playlists can only be named with chars A-Z, a-z, 0-9, and the chars in this string "! , . _ = - ( ) & % $ # @"
            EZPlaylist play = new EZPlaylist("YUP!");
            Assert.IsTrue(play.Name == "YUP!");

            play = new EZPlaylist("YUP.");
            Assert.IsTrue(play.Name == "YUP.");

            play = new EZPlaylist("YUP_");
            Assert.IsTrue(play.Name == "YUP_");

            play = new EZPlaylist("YUP8922");
            Assert.IsTrue(play.Name == "YUP8922");

            play = new EZPlaylist("YUP*");
            Assert.IsTrue(play.Name == "YUP");

            play = new EZPlaylist("YUP/");
            Assert.IsTrue(play.Name == "YUP");

            play = new EZPlaylist("YUP=");
            Assert.IsTrue(play.Name == "YUP=");

            play = new EZPlaylist("YUP,-()&%$#@");
            Assert.IsTrue(play.Name == "YUP,-()&%$#@");

            //Playlist names can only be 25 characters long
            play = new EZPlaylist("YUPYUPYUPYUPYUPYUPYUPYUPYUP");
            Assert.IsTrue(play.Name == "YUPYUPYUPYUPYUPYUPYUPYUPY");
        }

        [TestMethod]
        public void TestAddingSong()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            if (lib.Songs.Count > 0)
            {
                Song song = lib.Songs[0];
                SongInfo si = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                playlist.Add(si);
                Assert.IsTrue(playlist.Count == 1);
                Assert.IsTrue(playlist.Songs.Contains(si));
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add.");
            }
        }

        [TestMethod]
        public void TestRemovingSongs()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            if (lib.Songs.Count > 0)
            {
                Song song = lib.Songs[0];
                SongInfo si = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                playlist.Add(si);
                Assert.IsTrue(playlist.Count == 1);
                Assert.IsTrue(playlist.Songs.Contains(si));

                playlist.Remove(si);
                Assert.IsTrue(playlist.Count == 0);
                Assert.IsFalse(playlist.Songs.Contains(si));
            }
            else
            {
                Assert.Fail("Can't test removing a song because there are no songs to add.");
            }
        }

        [TestMethod]
        public void TestAddingListOfSongs()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            if (lib.Songs.Count > 1)
            {
                List<SongInfo> list = new List<SongInfo>();

                Song song = lib.Songs[0];
                SongInfo si1 = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                list.Add(si1);

                song = lib.Songs[1];
                SongInfo si2 = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                list.Add(si2);

                playlist.AddList(list.AsReadOnly());

                Assert.IsTrue(playlist.Count == 2);
                Assert.IsTrue(playlist.Songs.Contains(si1));
                Assert.IsTrue(playlist.Songs.Contains(si2));
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add or there is only one song.");
            }
        }

        [TestMethod]
        public void TestEqualityOfSongInfo()
        {
            SongInfo s1 = new SongInfo("a", "b", "c", new TimeSpan(1), 2);
            SongInfo s2 = new SongInfo("a", "b", "c", new TimeSpan(1), 2);

            Assert.IsTrue(s1.Equals(s2));
            
        }

        [TestMethod]
        public void TestCreatingPlaylistFile()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            if (lib.Songs.Count > 0)
            {
                Song song = lib.Songs[0];
                SongInfo si = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                playlist.Add(si);

                playlist.SavePlaylist();
                playlist.Clear();

                playlist = new EZPlaylist();
                playlist.ReadFromFile("My Awesome Playlist");
                Assert.IsTrue(playlist.Contains(si));
                Assert.IsTrue(playlist.Count == 1);
                Assert.IsTrue(playlist.Name == "My Awesome Playlist");
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add.");
            }
        }

        [TestMethod]
        public void TestCreatingPlaylistOfEntireLibrary()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            List<SongInfo> SIlist = new List<SongInfo>();
            if (lib.Songs.Count > 0)
            {
                foreach (Song song in lib.Songs)
                {
                    SongInfo si = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                    playlist.Add(si);
                    SIlist.Add(si);
                }

                playlist.SavePlaylist();
                playlist.Clear();

                playlist = new EZPlaylist();
                playlist.ReadFromFile("My Awesome Playlist");
                foreach (SongInfo si in SIlist)
                {
                    Assert.IsTrue(playlist.Contains(si));
                }
                Assert.IsTrue(playlist.Count == SIlist.Count);
                Assert.IsTrue(playlist.Name == "My Awesome Playlist");
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add.");
            }
        }

        [TestMethod]
        public void TestCreatingPlaylistOfEntireLibraryUsingAddListMethod()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            List<SongInfo> SIlist = new List<SongInfo>();
            if (lib.Songs.Count > 0)
            {
                foreach (Song song in lib.Songs)
                {
                    SongInfo si = new SongInfo(song.Artist.Name, song.Album.Name, song.Name, song.Duration, song.TrackNumber);
                    SIlist.Add(si);
                }
                playlist.AddList(SIlist.AsReadOnly());
                playlist.SavePlaylist();
                playlist.Clear();

                playlist = new EZPlaylist();
                playlist.ReadFromFile("My Awesome Playlist");
                foreach (SongInfo si in SIlist)
                {
                    Assert.IsTrue(playlist.Contains(si));
                }
                Assert.IsTrue(playlist.Count == SIlist.Count);
                Assert.IsTrue(playlist.Name == "My Awesome Playlist");
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add.");
            }
        }
        
        [TestMethod]
        public void TestDeletingPlaylistFile()
        {
            EZPlaylist playlist = new EZPlaylist("YUP");
            playlist.SavePlaylist(); //creates "YUP.xml"
            playlist.Clear();

            

            playlist.ReadFromFile("YUP"); //reading from "YUP.xml" confirming that it exists
            playlist.DeletePlaylist();

            playlist = new EZPlaylist("NOPE");
            playlist.ReadFromFile("YUP"); //If "YUP" exists, playlist should be named "YUP"
            Assert.IsTrue(playlist.Name == "NOPE"); //Since YUP does not exist, playlist's name is still NOPE
        }

        [TestMethod]
        public void TestGettingTwoCopiesOfSameSong()
        {
            EZPlaylist playlist = new EZPlaylist("My Awesome Playlist");
            MediaLibrary lib = new MediaLibrary();
            
            if (lib.Songs.Count > 0)
            {
                for(int i = 0; i < lib.Songs.Count; i++)
                {
                    if (lib.Songs[i].Artist.Name == "The Doors")
                    {
                        List<Song> thedoors = new List<Song>();
                        foreach (Song song in lib.Songs)
                        {
                            if (song.Artist.Name == "The Doors")
                                thedoors.Add(song);
                        }
                        if(thedoors[0].GetHashCode() == thedoors[1].GetHashCode())
                            System.Diagnostics.Debug.WriteLine(thedoors.Count);
                    }
                }
            }
            else
            {
                Assert.Fail("Can't test adding a song because there are no songs to add.");
            }

        }
    }
}
