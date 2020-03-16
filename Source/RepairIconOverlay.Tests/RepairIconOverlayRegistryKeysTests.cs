using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepairIconOverlay.Commands;
using RepairIconOverlay.Model;

namespace RepairIconOverlay.Tests
{
    [TestClass]
    public class RepairIconOverlayRegistryKeysTests
    {
        [TestMethod]
        public void CanGatherDuplicates()
        {
            var mockIdentifiers = new Mock<IShellIconOverlayIdentifiers>(MockBehavior.Strict);
            mockIdentifiers.Setup(o => o.GetIdentifiers()).Returns(new List<ShellIconOverlayIdentifier> {
                new ShellIconOverlayIdentifier("   TestExample", "abcde-fghij-etc"),
                new ShellIconOverlayIdentifier("  TestExample", "hijklm-nopq"),
                new ShellIconOverlayIdentifier(" TestExample", "rstuvw"),
                new ShellIconOverlayIdentifier("TestExample", "xyz-etc"),
            });
            var subject = new RepairIconOverlayRegistryKeys(mockIdentifiers.Object);
            var result = subject.GatherDuplicates().ToList();
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GatherChanges()
        {
            var configuration = new Configuration
            {
                Sets = new List<KeySet>
                {
                    new KeySet
                    {
                        Rank = 2,
                        Keys = new List<string>
                        {
                            "Tortoise1Normal",
                            "Tortoise2Modified",
                            "Tortoise3Conflict"
                        }
                    },
                    new KeySet
                    {
                        Rank = 1,
                        Keys = new List<string>
                        {
                            "DropboxExt01",
                            "DropboxExt02",
                            "DropboxExt03"
                        }
                    },
                    new KeySet
                    {
                        Rank = 0,
                        Keys = new List<string>
                        {
                            "EnhancedStorageShell",
                            "Offline Files"
                        }
                    }
                }
            };

            var mockIdentifiers = new Mock<IShellIconOverlayIdentifiers>(MockBehavior.Strict);
            mockIdentifiers.Setup(o => o.GetIdentifiers()).Returns(new List<ShellIconOverlayIdentifier> {
                new ShellIconOverlayIdentifier("  DropboxExt01", "not-important"),
                new ShellIconOverlayIdentifier(" DropboxExt02", "not-important"),
                new ShellIconOverlayIdentifier("  DropboxExt03", "not-important"),
                new ShellIconOverlayIdentifier(" Tortoise1Normal", "not-important"),
                new ShellIconOverlayIdentifier("  Tortoise2Modified", "not-important"),
                new ShellIconOverlayIdentifier(" Tortoise3Conflict", "not-important"),
                new ShellIconOverlayIdentifier("Offline Files", "not-important"),
                new ShellIconOverlayIdentifier("EnhancedStorageShell", "not-important"),
            });

            var mockConsole = new Mock<ConsoleDisplay>(MockBehavior.Loose);

            var subject = new RepairIconOverlayRegistryKeys(mockIdentifiers.Object);
            var result = subject.GatherChanges(mockConsole.Object, configuration).ToList();
            Assert.AreEqual(4, result.Count);


            Assert.AreEqual("Tortoise1Normal", result[0].Item1.Name);
            Assert.AreEqual(2, result[0].Item2);

            Assert.AreEqual("Tortoise3Conflict", result[1].Item1.Name);
            Assert.AreEqual(2, result[1].Item2);

            Assert.AreEqual("DropboxExt01", result[2].Item1.Name);
            Assert.AreEqual(1, result[2].Item2);

            Assert.AreEqual("DropboxExt03", result[3].Item1.Name);
            Assert.AreEqual(1, result[3].Item2);
        }
    }
}
