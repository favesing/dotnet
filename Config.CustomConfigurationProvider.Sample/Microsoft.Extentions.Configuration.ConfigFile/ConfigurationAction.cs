using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extentions.Configuration.ConfigFile
{
    /// <summary>
    /// This represents the action for that *.config element.
    /// There are 3 possible element names:
    ///     <list type="number">
    ///         <item>
    ///             <term>add</term>
    ///         </item>
    ///         <item>
    ///             <term>remove</term>
    ///         </item>
    ///         <item>
    ///             <term>clear</term>
    ///         </item>
    ///     </list>
    /// https://msdn.microsoft.com/en-us/library/aa903313(v=vs.71).aspx
    /// </summary>
    internal enum ConfigurationAction
    {
        Add,
        Remove,
        Clear
    }
}
