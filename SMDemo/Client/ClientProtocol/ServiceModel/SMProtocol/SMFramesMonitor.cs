﻿//-----------------------------------------------------------------------
// <copyright file="SMFramesMonitor.cs" company="Microsoft Open Technologies, Inc.">
//
// ---------------------------------------
// HTTPbis
// Copyright Microsoft Open Technologies, Inc.
// ---------------------------------------
// Microsoft Reference Source License.
// 
// This license governs use of the accompanying software. 
// If you use the software, you accept this license. 
// If you do not accept the license, do not use the software.
// 
// 1. Definitions
// 
// The terms "reproduce," "reproduction," and "distribution" have the same meaning here 
// as under U.S. copyright law.
// "You" means the licensee of the software.
// "Your company" means the company you worked for when you downloaded the software.
// "Reference use" means use of the software within your company as a reference, in read // only form, 
// for the sole purposes of debugging your products, maintaining your products, 
// or enhancing the interoperability of your products with the software, 
// and specifically excludes the right to distribute the software outside of your company.
// "Licensed patents" means any Licensor patent claims which read directly on the software 
// as distributed by the Licensor under this license. 
// 
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, the Licensor grants you a non-transferable, 
// non-exclusive, worldwide, royalty-free copyright license to reproduce the software for reference use.
// (B) Patent Grant- Subject to the terms of this license, the Licensor grants you a non-transferable, 
// non-exclusive, worldwide, royalty-free patent license under licensed patents for reference use. 
// 
// 3. Limitations
// (A) No Trademark License- This license does not grant you any rights 
// to use the Licensor’s name, logo, or trademarks.
// (B) If you begin patent litigation against the Licensor over patents that you think may apply 
// to the software (including a cross-claim or counterclaim in a lawsuit), your license 
// to the software ends automatically. 
// (C) The software is licensed "as-is." You bear the risk of using it. 
// The Licensor gives no express warranties, guarantees or conditions. 
// You may have additional consumer rights under your local laws 
// which this license cannot change. To the extent permitted under your local laws, 
// the Licensor excludes the implied warranties of merchantability, 
// fitness for a particular purpose and non-infringement. 
// 
// -----------------End of License---------
//
// </copyright>
//-----------------------------------------------------------------------
namespace System.ServiceModel.SMProtocol
{
    using System.ServiceModel.SMProtocol.SMFrames;

    /// <summary>
    /// Class that can be used to monitor frames on session.
    /// </summary>
    public class SMFramesMonitor : IDisposable
    {
        /// <summary>
        /// SM Session.
        /// </summary>
        private readonly SMSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SMFramesMonitor"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="filter">The filter.</param>
        public SMFramesMonitor(SMSession session, Func<BaseFrame, bool> filter)
        {
            this.session = session;
            this.Filter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SMFramesMonitor"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SMFramesMonitor(SMSession session)
            : this(session, null)
        {
        }

        /// <summary>
        /// Occurs when frame is sent.
        /// </summary>
        public event EventHandler<FrameEventArgs> OnFrameSent;

        /// <summary>
        /// Occurs when frame is received.
        /// </summary>
        public event EventHandler<FrameEventArgs> OnFrameReceived;

        /// <summary>
        /// Gets or sets the filter for frames.
        /// </summary>
        public Func<BaseFrame, bool> Filter { get; set; }

        /// <summary>
        /// Attaches monitr to session.
        /// </summary>
        public void Attach()
        {
            this.session.Protocol.OnFrameSent += this.ProtocolOnSendFrame;
            this.session.Protocol.OnFrameReceived += this.ProtocolOnFrameReceived;            
        }

        /// <summary>
        /// Detach monitor from session.
        /// </summary>
        public void Dispose()
        {
            this.session.Protocol.OnFrameSent -= this.ProtocolOnSendFrame;
            this.session.Protocol.OnFrameReceived -= this.ProtocolOnFrameReceived;
        }

        /// <summary>
        /// On send frame event handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void ProtocolOnSendFrame(object sender, FrameEventArgs e)
        {
            if (this.Filter == null || this.Filter(e.Frame))
            {
                if (this.OnFrameSent != null)
                {
                    this.OnFrameSent(this, e);
                }
            }
        }

        /// <summary>
        /// On receive frame event handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void ProtocolOnFrameReceived(object sender, FrameEventArgs e)
        {
            if (this.Filter == null || this.Filter(e.Frame))
            {
                if (this.OnFrameReceived != null)
                {
                    this.OnFrameReceived(this, e);
                }
            }
        }
    }
}