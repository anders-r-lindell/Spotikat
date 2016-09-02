using System;
using System.Collections.Generic;

using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Services.Configuration;
using SpotiKat.Services.Entities;
using SpotiKat.Spotify.Entities;
using SpotiKat.Spotify.Entities.Lookup;
using SpotiKat.Spotify.Entities.Search;
using SpotiKat.Spotify.Exceptions;
using SpotiKat.Spotify.Interfaces;

namespace SpotiKat.Services.Tests {
	[TestFixture]
	public class SpotifyAlbumServiceTests {
		[Test]
		public void FindAlbum_ArtistHrefIsNull_ThrowsArgumentNullException() {
			var spotifyAlbumService = new SpotifyAlbumService(A.Fake<ILookupService>());

			Assert.Throws<ArgumentNullException>(() => spotifyAlbumService.FindAlbum(null, "abc", "def"));
		}

		[Test]
		public void FindAlbum_AlbumNameIsNull_ThrowsArgumentNullException() {
			var spotifyAlbumService = new SpotifyAlbumService(A.Fake<ILookupService>());

			Assert.Throws<ArgumentNullException>(() => spotifyAlbumService.FindAlbum("jkl", null, "def"));
		}

		[Test]
		public void FindAlbum_ArtistHrefAndAlbumNameIsNotNull_CallsLookupServiceArtistLookupWithArtistHref() {
			var lookupServiceFake = A.Fake<ILookupService>();
			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			A.CallTo(() => lookupServiceFake.ArtistLookup("jkl")).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void FindAlbum_ArtistLookupReturnsNull_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(null);

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupArtistIsNull_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo { Artist = null });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupArtistAlbumsIsNull_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = null
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupArtistAlbumsIsEmpty_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo>()
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupNoMatchingAlbumName_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo> {
			                                                                                         	                                                     	                             	new AlbumInfo {
			                                                                                         	                                                     	                             	              	Album = new SpotiKat.Spotify.Entities.Lookup.Album {
			                                                                                         	                                                     	                             	              	                                                   	Name = "ghi", Availability = new Availability {
			                                                                                         	                                                     	                             	              	                                                   	                                              	TerritoriesAsString = "def uio"
			                                                                                         	                                                     	                             	              	                                                   	                                              }
			                                                                                         	                                                     	                             	              	                                                   }
			                                                                                         	                                                     	                             	              }
			                                                                                         	                                                     	                             }
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupNoMatchingAlbumTerritory_ReturnsNull() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo> {
			                                                                                         	                                                     	                             	new AlbumInfo {
			                                                                                         	                                                     	                             	              	Album = new SpotiKat.Spotify.Entities.Lookup.Album {
			                                                                                         	                                                     	                             	              	                                                   	Name = "abc", Availability = new Availability {
			                                                                                         	                                                     	                             	              	                                                   	                                              	TerritoriesAsString = "qwe uio"
			                                                                                         	                                                     	                             	              	                                                   	                                              }
			                                                                                         	                                                     	                             	              	                                                   }
			                                                                                         	                                                     	                             	              }
			                                                                                         	                                                     	                             }
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupMatchingAlbumNameAndTerritory_ReturnsAlbum() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo> {
			                                                                                         	                                                     	                             	new AlbumInfo {
			                                                                                         	                                                     	                             	              	Album = new SpotiKat.Spotify.Entities.Lookup.Album {
			                                                                                         	                                                     	                             	              	                                                   	Name = "abc", Availability = new Availability {
			                                                                                         	                                                     	                             	              	                                                   	                                              	TerritoriesAsString = "def uio"
			                                                                                         	                                                     	                             	              	                                                   	                                              }
			                                                                                         	                                                     	                             	              	                                                   }
			                                                                                         	                                                     	                             	              }
			                                                                                         	                                                     	                             }
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Not.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupMatchingAlbumNameIgnoreCaseAndTerritory_ReturnsAlbum() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo> {
			                                                                                         	                                                     	                             	new AlbumInfo {
			                                                                                         	                                                     	                             	              	Album = new SpotiKat.Spotify.Entities.Lookup.Album {
			                                                                                         	                                                     	                             	              	                                                   	Name = "AbC", Availability = new Availability {
			                                                                                         	                                                     	                             	              	                                                   	                                              	TerritoriesAsString = "def uio"
			                                                                                         	                                                     	                             	              	                                                   	                                              }
			                                                                                         	                                                     	                             	              	                                                   }
			                                                                                         	                                                     	                             	              }
			                                                                                         	                                                     	                             }
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", "def");

			Assert.That(album, Is.Not.Null);
		}

		[Test]
		public void FindAlbum_ArtistLookupMatchingAlbumNameAndNullTerritory_ReturnsAlbum() {
			var lookupServiceFake = A.Fake<ILookupService>();
			A.CallTo(() => lookupServiceFake.ArtistLookup(A<string>.Ignored)).Returns(new ArtistInfo {
			                                                                                         	Artist = new SpotiKat.Spotify.Entities.Lookup.Artist {
			                                                                                         	                                                     	Albums = new List<AlbumInfo> {
			                                                                                         	                                                     	                             	new AlbumInfo {
			                                                                                         	                                                     	                             	              	Album = new SpotiKat.Spotify.Entities.Lookup.Album {
			                                                                                         	                                                     	                             	              	                                                   	Name = "abc", Availability = new Availability {
			                                                                                         	                                                     	                             	              	                                                   	                                              	TerritoriesAsString = "def uio"
			                                                                                         	                                                     	                             	              	                                                   	                                              }
			                                                                                         	                                                     	                             	              	                                                   }
			                                                                                         	                                                     	                             	              }
			                                                                                         	                                                     	                             }
			                                                                                         	                                                     }
			                                                                                         });

			var spotifyAlbumService = new SpotifyAlbumService(lookupServiceFake);

			var album = spotifyAlbumService.FindAlbum("jkl", "abc", null);

			Assert.That(album, Is.Not.Null);
		}
	}
}