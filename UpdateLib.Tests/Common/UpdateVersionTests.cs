using System;
using UpdateLib.Core;
using UpdateLib.Core.Enums;
using Xunit;

namespace UpdateLib.Tests.Common
{
    public class UpdateVersionTests
    {
        [Theory]
        [InlineData("1.2.3-beta", 1, 2, 3, VersionLabel.Beta)]
        [InlineData("1.2.3-rc", 1, 2, 3, VersionLabel.RC)]
        [InlineData("1.2.3-alpha", 1, 2, 3, VersionLabel.Alpha)]
        [InlineData("1.2.3", 1, 2, 3, VersionLabel.None)]
        [InlineData("1.2", 1, 2, 0, VersionLabel.None)]
        [InlineData("1.2-beta", 1, 2, 0, VersionLabel.Beta)]
        [InlineData("1", 1, 0, 0, VersionLabel.None)]
        [InlineData("1-rc", 1, 0, 0, VersionLabel.RC)]
        public void TestTryParseGood(string input, int major, int minor, int patch, VersionLabel label)
        {
            var v = new UpdateVersion(input);

            Assert.Equal(major, v.Major);
            Assert.Equal(minor, v.Minor);
            Assert.Equal(patch, v.Patch);
            Assert.Equal(label, v.Label);
        }

        [Theory]
        [InlineData("1-beta-alpha")]
        [InlineData("1-xxx")]
        [InlineData("xxx-1.2.3")]
        [InlineData("1-2.3.4")]
        [InlineData("blabla")]
        public void TestTryParseBad(string input)
        {
            Assert.ThrowsAny<Exception>(() => new UpdateVersion(input));
        }

        [Fact]
        public void TestTryParseReturnsFalseInBadCase()
        {
            string input = "1.2.3.beta";

            Assert.False(UpdateVersion.TryParse(input, out UpdateVersion _));
        }

        [Fact]
        public void TestStringValue()
        {
            var v = new UpdateVersion(1, 2, 3, VersionLabel.RC);

            Assert.Equal("1.2.3-rc", v.Value);

            v.Value = "3.1.2";

            Assert.Equal(3, v.Major);
            Assert.Equal(1, v.Minor);
            Assert.Equal(2, v.Patch);
            Assert.Equal(VersionLabel.None, v.Label);
        }

        [Fact]
        public void ConstructorThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateVersion(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateVersion(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateVersion(1, 1, -1));
            Assert.Throws<ArgumentException>(() => new UpdateVersion("blabla"));
        }

        [Fact]
        public void TestOperators()
        {
            UpdateVersion v1 = new UpdateVersion(1);
            UpdateVersion v2 = new UpdateVersion(1);
            UpdateVersion v3 = new UpdateVersion(1, 1);
            UpdateVersion v4 = new UpdateVersion(1, 1, 1);
            UpdateVersion v5 = new UpdateVersion(1, 1, 1, VersionLabel.Alpha);
            UpdateVersion v6 = new UpdateVersion(1, 1, 1, VersionLabel.Beta);
            UpdateVersion v7 = new UpdateVersion(1, 1, 1, VersionLabel.RC);

            Assert.True(v1 == v2, "v1 == v2");
            Assert.True(v1 != v3, "v1 != v3");

            Assert.True(v3 > v1, "v3 > v1");
            Assert.False(v4 < v3, "v4 < v3");

            Assert.True(v7 > v6, "v7 > v6");
            Assert.True(v6 > v5, "v6 > v5");
        }

        [Fact]
        public void TestConversion()
        {
            string input = "1.1.1-rc";

            UpdateVersion v = input;

            Assert.Equal(1, v.Major);
            Assert.Equal(1, v.Minor);
            Assert.Equal(1, v.Patch);
            Assert.Equal(VersionLabel.RC, v.Label);

            string output = v;

            Assert.Equal(input, output);
        }
    }
}
