using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xunit
{
    /// <summary>
    /// XUnit backwards compatibility for feature generator
    /// </summary>
    /// <remarks>
    /// Delete this when generator no longer uses this class
    /// https://github.com/techtalk/SpecFlow/issues/419
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface IUseFixture<T> : Xunit.IClassFixture<T> where T : class, new()
    {
    }
}
