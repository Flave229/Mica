using System;
using System.Collections.Generic;
using Mica.Core.Communication;
using Mica.Core.Communication.Models;
using Mica.Core.Services;
using Moq;
using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace GivenARequestToStoreAchievements
{
    [TestFixture]
    public class WhenAllTheAchievementsAreNew
    {
        [Test]
        public void ThenAllAchievementsAreAddedToTheDatabase()
        {
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
                        },
                        new PlayerAchievement
                        {
                            Achieved = 1,
                            Name = "A Friend in Need",
                            AchievedTimestamp = "1483002574"
                        }
                    },
                    GameInfo = new GameInfo
                    {
                        Name = "Witcher 3"
                    }
                });

            var subject = new SteamService(steamClientMock.Object);

            var result = subject.UpdateSteamAchievementsFor("211420", "flave_229");
        }
    }
}
