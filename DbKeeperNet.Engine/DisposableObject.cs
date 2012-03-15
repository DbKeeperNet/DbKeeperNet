using System;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Base class implementing IDisposable pattern
    /// </summary>
    public abstract class DisposableObject: IDisposable
    {
        #region Private members
        private bool _isDisposed; 
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indidication whether this class has been already disposed or not.
        /// </summary>
        public bool IsDisposed{get { return _isDisposed; }}

        /// <summary>
        /// Invoked from <seealso cref="Dispose()"/> method
        /// and finalizer.
        /// </summary>
        /// <param name="disposing">Indicates, whether this has been invoked from finalizer or <seealso cref="IDisposable.Dispose"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                _isDisposed = true;
            }
        }
    }
}
