<<<<<<< HEAD
﻿
namespace ProgramManager.Services
{
    using System;

    /// <summary>
    /// The base class for event model.
    /// </summary>
    public class BaseEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs"/> class.
        /// Initializes Arguments property use the value of constructor arguments.
        /// </summary>
        /// <param name="args">
        /// Parameters that must be passed to event subscribers.
        /// </param>
        public BaseEventArgs(object args)
        {
            this.Package = args;
        }

        /// <summary>
        /// Gets the package.
        /// </summary>
        public object Package { get; }
=======
﻿using ProgramManager.Models.PackageModel;
using System;

namespace ProgramManager.Services
{
    public class BaseEventArgs : EventArgs
    {
        public BaseEventArgs(object param)
        {
            Package = param;
        }
        public object Package { get; private set; }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
