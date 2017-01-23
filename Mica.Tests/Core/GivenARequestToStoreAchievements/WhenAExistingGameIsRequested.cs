using System.Collections.Generic;
using System.Linq;
using Mica.Core.Communication;
using Mica.Core.Communication.Models;
using Mica.Core.Models;
using Mica.Core.Repositories;
using Mica.Core.Repositories.SqlModels;
using Mica.Core.Services;
using Moq;
using NUnit.Framework;

namespace Mica.Tests.Core.GivenARequestToStoreAchievements
{
    [TestFixture]
    class WhenAExistingGameIsRequested
    {
        private Mock<ISteamRepository> _steamRepositoryMock;
        private string _gameName = "Witcher 3";

        [SetUp]
        public void Setup()
        {
            _steamRepositoryMock = new Mock<ISteamRepository>();
            _steamRepositoryMock.Setup(x => x.GetGame(It.IsAny<string>())).Returns(new List<MicaGame>
            {
                new MicaGame
                {
                    Name = _gameName
                }
            });

            var steamClientMock = new Mock<ISteamClient>();
            steamClientMock.Setup(x => x.GetUserAchievementsForGame(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PlayerStatsForGame
                {
                    Achievements = new List<PlayerAchievement>
                    {
                        new PlayerAchievement
                        {
                            Achieved = 1,
                            Name = "Lilac and Gooseberries",
                            AchievedTimestamp = "1483002574"
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = _gameName,
                        ApplicationId = "W3"
                    }
                });

            var subject = new SteamService(steamClientMock.Object, _steamRepositoryMock.Object);

            var result = subject.UpdateSteamAchievementsFor("211420", "flave_229");
        }

        [Test]
        public void ThenTheGameIsNotAddedToTheDatabase()
        {
            _steamRepositoryMock.Verify(x => x.InsertGame(It.Is<GameInfo>(game => game.Name == _gameName)), Times.Never);
        }
    }
}
