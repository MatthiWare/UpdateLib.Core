using System;
using System.Collections.Generic;
using System.Text;
using UpdateLib.Core;
using UpdateLib.Core.Common;
using Xunit;

namespace UpdateLib.Tests.Core.Common
{
    public class UpdateInfoTests
    {
        [Theory]
        [InlineData("1.0.0", "2.0.0")]
        [InlineData("1.0.0", "1.0.0")]
        public void BasedOnVersionHigherThenSelfVersionThrowsException(string currVersion, string baseVersion)
        {
            var basedOnVersion = new UpdateVersion(baseVersion);
            var version = new UpdateVersion(currVersion);

            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateInfo(version, basedOnVersion, "", ""));
        }
    }
}
