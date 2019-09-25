
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
    }
}
