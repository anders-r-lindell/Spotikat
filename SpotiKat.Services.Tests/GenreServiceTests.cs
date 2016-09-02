using FakeItEasy;

using NUnit.Framework;

using SpotiKat.Services.Configuration;

namespace SpotiKat.Services.Tests {
	[TestFixture]
	public class GenreServiceTests {
		[Test]
		public void GetGenres_GenreConfigurationGenresDefined_ReturnsGenres() {
			var genreConfigurationFake = A.Fake<IGenreConfiguration>();
			A.CallTo(() => genreConfigurationFake.Genres).Returns("<options><option value=\"0\"><![CDATA[ABC]]></option><option value=\"1\"><![CDATA[DEF]]></option></options>");

			var genres = new GenreService(genreConfigurationFake).GetGenres();

			Assert.That(genres.Count, Is.EqualTo(2));
			Assert.That(genres[0].Text, Is.EqualTo("ABC"));
			Assert.That(genres[0].Value, Is.EqualTo(0));
		}
	}
}
