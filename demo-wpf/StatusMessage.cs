using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_wpf
{
    /// <summary>
    /// Class encapsulating the status message that has to be
    /// displayed in the FluidStatusBar
    /// </summary>
    public class StatusMessage
    {
        #region Properties

        /// <summary>
        /// The message to be displayed in the FluidStatusBar
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Flag to indicate whether to animate the fade out of
        /// the currently displayed message before showing the 
        /// new message.
        /// </summary>
        public bool IsAnimated { get; set; }

        #endregion

        #region Construction / Initialization

        /// <summary>
        /// Ctor
        /// </summary>
        public StatusMessage()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message">The message to be displayed in the FluidStatusBar</param>
        /// <param name="isAnimated">Flag to indicate whether to animate the fade out of 
        /// the currently displayed message before showing the new message.</param>
        public StatusMessage(string message, bool isAnimated = false)
        {
            Message = message;
            IsAnimated = isAnimated;
        }

        #endregion
    }
}
