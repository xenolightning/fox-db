using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoxDb.Tests
{
    public class MiscellaneousTests
    {

        [Fact]
        public void UnixTimeStamp_Origin_Is_Zero()
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AsUnixTimestamp();
            Assert.Equal(0, origin);
        }

        [Fact]
        public void UnixTimeStamp_2000_Is_Valid()
        {
            var origin = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc).AsUnixTimestamp();
            Assert.Equal(946684800, origin); //known value

        }

    }
}
