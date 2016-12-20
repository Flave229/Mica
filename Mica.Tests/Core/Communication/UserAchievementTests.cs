using System.Collections.Generic;
using System.Linq;
using Mica.Communication;
using Mica.Communication.Models;
using Mica.Core.Builders;
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
                .Returns(new SteamGameStats
                {
                    playerstats = new PlayerStats
                    {
                        achievements = new List<PlayerAchievement>
                        {
                            new PlayerAchievement
                            {
                                apiname = "Achievement1",
                                achieved = 1
                            },
                            new PlayerAchievement
                            {
                                apiname = "Achievement2",
                                achieved = 0
                            }
                        }
                    }
                });
            steamClientMock.Setup(x => x.GetInfoForGame(It.IsAny<string>()))
                .Returns(new SteamGameInfo()
                {
                    game = new SteamGame()
                    {
                        availableGameStats = new GameStats
                        {
                            achievements = new List<GameAchievements>()
                            {
                                new GameAchievements
                                {
                                    name = "Achievement1",
                                    displayName = "AchievementDisplayName1"
                                },
                                new GameAchievements
                                {
                                    name = "Achievement2",
                                    displayName = "AchievementDisplayName2"
                                }
                            }
                        }
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
