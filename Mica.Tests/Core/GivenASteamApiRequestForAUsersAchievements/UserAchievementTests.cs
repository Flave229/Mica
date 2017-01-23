using System.Collections.Generic;
using System.Linq;
using Mica.Core.Builders;
using Mica.Core.Communication;
using Mica.Core.Communication.Models;
using Mica.Core.Models;
using Mica.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace Mica.Tests.Core.GivenASteamApiRequestForAUsersAchievements
{
    [TestFixture]
    public class WhenTheUserHasCompletedHalfOfTheAchievements
    {
        private List<Achievement> _result;

        [SetUp]
        public void Setup()
        {
            var steamRepositoryMock = new Mock<ISteamRepository>();
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

            var subject = new SteamAchievementBuilder(steamClientMock.Object, steamRepositoryMock.Object);

            _result = subject.BuildFor("", "");
        }

        [Test]
        public void ThenOnlyHalfAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(1));
        }

        [Test]
        public void ThenTheAchievedAchievementIsReturned()
        {
            Assert.That(_result.FirstOrDefault()?.Name, Is.EqualTo("AchievementDisplayName1"));
        }
    }
    
    [TestFixture]
    public class WhenTheUserHasMultipleGamesInTheirAccount
    {
        private List<Achievement> _result;

        [SetUp]
        public void Setup()
        {
            var steamRepositoryMock = new Mock<ISteamRepository>();
            var steamClientMock = new Mock<ISteamClient>();
            steamClientMock.Setup(x => x.GetSteam64IdCode(It.IsAny<string>()))
                .Returns("229");
            steamClientMock.Setup(x => x.GetOwnedGameListFor(It.IsAny<string>()))
                .Returns(new GameListResponse
                {
                    response = new GameList
                    {
                        games = new List<SteamGame>
                        {
                            new SteamGame
                            {
                                appid = 1
                            },
                            new SteamGame
                            {
                                appid = 2
                            }
                        }
                    }
                });
            steamClientMock.SetupSequence(x => x.GetUserAchievementsForGame(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PlayerStatsForGame
                {
                    Achievements = new List<PlayerAchievement>
                    {
                        new PlayerAchievement
                        {
                            ApiName = "Achievement1",
                            Name = "AchievementDisplayName1",
                            AchievedTimestamp = "1483002574",
                            Achieved = 1
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = "FirstGame"
                    }
                })
                .Returns(new PlayerStatsForGame
                {
                    Achievements = new List<PlayerAchievement>
                    {
                        new PlayerAchievement
                        {
                            ApiName = "Achievement2",
                            Name = "AchievementDisplayName2",
                            AchievedTimestamp = "1483002574",
                            Achieved = 1
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = "SecondGame"
                    }
                });

            var subject = new SteamAchievementBuilder(steamClientMock.Object, steamRepositoryMock.Object);

            _result = subject.BuildAll("");
        }

        [Test]
        public void ThenAchievementsForBothGamesAreReturned()
        {
            Assert.That(_result.ElementAt(0).Name, Is.EqualTo("AchievementDisplayName1"));
            Assert.That(_result.ElementAt(1).Name, Is.EqualTo("AchievementDisplayName2"));
        }
    }
}
