using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepairIconOverlay.Tests
{
    [TestClass]
    public class CommandLineParserTests
    {
        [TestMethod]
        public void CanCheckForSwitches1()
        {
            var input = new[] { "Not switched", "-h", "switch arg", "-hi", "t-est" };
            var subject = new CommandLineParser(input);

            Assert.IsTrue(subject.ContainsCommand('h'));
            Assert.IsTrue(subject.ContainsCommand("h"));
            Assert.IsFalse(subject.ContainsCommand('e'));
            Assert.IsFalse(subject.ContainsCommand("e"));
            Assert.IsFalse(subject.ContainsCommand("est"));
        }
        
        [TestMethod]
        public void CanCheckForSwitches2()
        {
            var input = new[] { "Not switched", "-hi", "t-est" };
            var subject = new CommandLineParser(input);

            Assert.IsFalse(subject.ContainsCommand('h'));
            Assert.IsFalse(subject.ContainsCommand("h"));
        }

        [TestMethod]
        public void CanCheckForSwitches1AltPrefix()
        {
            var input = new[] { "Not switched", "/h", "switch arg", "/hi", "t/est" };
            var subject = new CommandLineParser(input);

            Assert.IsTrue(subject.ContainsCommand('h'));
            Assert.IsTrue(subject.ContainsCommand("h"));
            Assert.IsFalse(subject.ContainsCommand('e'));
            Assert.IsFalse(subject.ContainsCommand("e"));
            Assert.IsFalse(subject.ContainsCommand("est"));
        }

        [TestMethod]
        public void CanGetCommandParameter()
        {
            var input = new[] { "/first", "/h", "switch arg", "/hi", "t/est", "/last" };
            var subject = new CommandLineParser(input);

            { // SCOPE
                Assert.IsTrue(subject.GetCommandParameter('h', out var parameter));
                Assert.AreEqual("switch arg", parameter);
            }

            { // SCOPE
                Assert.IsTrue(subject.GetCommandWithParameter("h", out var parameter));
                Assert.AreEqual("switch arg", parameter);
            }

            { // SCOPE
                Assert.IsTrue(subject.GetCommandWithParameter("hi", out var parameter));
                Assert.AreEqual("t/est", parameter);
            }

            { // SCOPE
                Assert.IsFalse(subject.ContainsCommand("e"));
                Assert.IsFalse(subject.GetCommandWithParameter("e", out var parameter));
                Assert.IsNull(parameter);
            }

            { // SCOPE
                Assert.IsFalse(subject.ContainsCommand("est"));
                Assert.IsFalse(subject.GetCommandWithParameter("est", out var parameter));
                Assert.IsNull(parameter);
            }

            { // SCOPE
                Assert.IsTrue(subject.ContainsCommand("last"));
                Assert.IsTrue(subject.GetCommandWithParameter("last", out var parameter));
                Assert.IsNull(parameter);
            }

            { // SCOPE
                Assert.IsTrue(subject.ContainsCommand("first"));
                Assert.IsTrue(subject.GetCommandWithParameter("first", out var parameter));
                Assert.IsNull(parameter);
            }
        }
    }
}
