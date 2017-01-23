using System.Collections.Generic;
using System.Linq;
using Mica.Core.Communication;
using Mica.Core.Communication.Models;
using Mica.Core.Models;
using Mica.Core.Repositories;
using Mica.Core.Services;
using Moq;
using NUnit.Framework;

namespace Mica.Tests.Core.GivenARequestToStoreAchievements
{
    [TestFixture]
    public class WhenAllTheAchievementsAreNew
    {
        private Mock<ISteamRepository> _steamRepositoryMock;
        private string _achievementName1 = "Lilac and Gooseberries";
        private string _achievementName2 = "A Friend in Need";

        [SetUp]
        public void Setup()
        {
            _steamRepositoryMock = new Mock<ISteamRepository>();

            var steamClientMock = new Mock<ISteamClient>();
            steamClientMock.Setup(x => x.GetUserAchievementsForGame(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PlayerStatsForGame
                {
                    Achievements = new List<PlayerAchievement>
                    {
                        new PlayerAchievement
                        {
                            Achieved = 1,
                            Name = _achievementName1,
                            AchievedTimestamp = "1483002574"
                        },
                        new PlayerAchievement
                        {
                            Achieved = 1,
                            Name = _achievementName2,
                            AchievedTimestamp = "1483002574"
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = "Witcher 3"
                    }
                });

            var subject = new SteamService(steamClientMock.Object, _steamRepositoryMock.Object);

            var result = subject.UpdateSteamAchievementsFor("211420", "flave_229");
        }

        [Test]
        public void ThenAllAchievementsAreAddedToTheDatabase()
        {
            _steamRepositoryMock.Verify(x => x.InsertAchievement(It.Is<Achievement>(
                achievement => achievement.Name == _achievementName1), It.IsAny<string>()), Times.Exactly(1));

            _steamRepositoryMock.Verify(x => x.InsertAchievement(It.Is<Achievement>(
                achievement => achievement.Name == _achievementName2), It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
