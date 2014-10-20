using System.Configuration;
using EchoNest.Artist;
using NUnit.Framework;

namespace EchoNest.Tests
{
    [TestFixture]
    public class ArtistTests
    {
        [Test]
        public void GetArtistByArtistName_JamesTaylor_IsProfileJamesTaylor()
        {
            const string artistName = "James Taylor";

            using (var session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                ProfileResponse profileResponse = session.Query<Profile>().Execute(artistName);

                Assert.IsNotNull(profileResponse);
                Assert.AreEqual(artistName, profileResponse.Artist.Name, artistName);
            }
        }
    }
}
