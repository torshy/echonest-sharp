using System.Configuration;
using EchoNest.Artist;
using NUnit.Framework;

namespace EchoNest.Tests
{
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "OK.")]
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
