using System.Collections.Generic;
using System.Linq;
using Mica.Core.Builders;
using Mica.Core.Communication;
using Mica.Core.Communication.Models;
using Mica.Core.Models;
using Moq;
using NUnit.Framework;

namespace Mica.Tests.Core.Communication
{
    [TestFixture]
    public class GivenARequestForAUsersAchievementsOnAGame
    {
        private List<Achievement> _result;

        [SetUp]
        public void WhenTheUserHasCompletedHalfOfTheAchievements()
        {
            var steamClientMock = new Mock<ISteamClient>();
            steamClientMock.Setup(x => x.GetUserAchievementsForGame(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PlayerStatsForGame
                {
                    Achievements =  new List<PlayerAchievement>
                    {
                        new PlayerAchievement
                        {
                            ApiName = "Achievement1",
                            Name = "AchievementDisplayName1",
                            AchievedTimestamp = "1483002574",
                            Achieved = 1
                        },
                        new PlayerAchievement
                        {
                            ApiName = "Achievement2",
                            Name = "AchievementDisplayName2",
                            Achieved = 0
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = "GameName"
                    }
                });

            var subject = new SteamAchievementBuilder(steamClientMock.Object);

            _result = subject.Build("", "");
        }

        [Test]
        public void ThenOnlyHalfAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(1));
        }

        [Test]
        public void ThenTheAchievedAchievementIsReturned()
        {
            Assert.That(_result.FirstOrDefault()?.AchievementName, Is.EqualTo("AchievementDisplayName1"));
        }
    }
}
