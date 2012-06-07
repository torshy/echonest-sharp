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
            //arrange
            const string artistName = "James Taylor";

            using (var session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                //act
                ProfileResponse profileResponse = session.Query<Profile>().Execute(artistName);

                //assert
                Assert.IsNotNull(profileResponse);
                Assert.AreEqual(artistName, profileResponse.Artist.Name, artistName);
            }
        }
    }
}