using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace CoLocatedCardSystem.CollaborationWindow
{
    public delegate void ViewReleasedHandler(Object sender, EventArgs e);
    public sealed class CollaborationWindowLifeEventControl : INotifyPropertyChanged
    {
        CoreDispatcher dispatcher;
        CoreWindow window;
        int viewID;
        bool released = false;
        int refCount = 0;
        bool madeVisible = false;
        event ViewReleasedHandler InternalReleased;
        string docFileDir = "";
        string tableFileDir = "";
        public CoreDispatcher Dispatcher
        {
            get
            {
                // This property never changes, so there's no need to lock
                return dispatcher;
            }
        }
        public int Id
        {
            get
            {
                // This property never changes, so there's no need to lock
                return viewID;
            }
        }

        public string DocFileDir
        {
            get
            {
                return docFileDir;
            }

            set
            {
                docFileDir = value;
            }
        }

        public string TableFileDir
        {
            get
            {
                return tableFileDir;
            }

            set
            {
                tableFileDir = value;
            }
        }

        private CollaborationWindowLifeEventControl(CoreWindow newWindow)
        {
            dispatcher = newWindow.Dispatcher;
            window = newWindow;
            viewID = ApplicationView.GetApplicationViewIdForWindow(window);
            RegisterForEvents();
        }

        private void RegisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
        }
        private void UnregisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }
        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs e)
        {
            StopViewInUse();
        }


        public int StartViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;

            // This method is called from several different threads
            // (each view lives on its own thread)
            lock (this)
            {
                releasedCopy = this.released;
                if (!released)
                {
                    refCountCopy = ++refCount;
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed");
            }

            return refCountCopy;
        }
        private void StopViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;

            // This method is called from several different threads
            // (each view lives on its own thread)
            lock (this)
            {
                releasedCopy = this.released;
                if (!released)
                {
                    refCountCopy = --refCount;
                    if ((refCountCopy == 0) && madeVisible)
                    {
                        dispatcher.RunAsync(CoreDispatcherPriority.Low, FinalizeRelease);
                    }
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed");
            }
        }

        private void FinalizeRelease()
        {
            bool justReleased = false;
            lock (this)
            {
                if (refCount == 0)
                {
                    justReleased = true;
                    released = true;
                }
            }

            // This assumes that released will never be made false after it
            // it has been set to true
            if (justReleased)
            {
                UnregisterForEvents();
                InternalReleased(this, null);
            }
        }
        public static CollaborationWindowLifeEventControl CreateForCurrentView()
        {
            return new CollaborationWindowLifeEventControl(CoreWindow.GetForCurrentThread());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event ViewReleasedHandler Released
        {
            add
            {
                bool releasedCopy = false;
                lock (this)
                {
                    releasedCopy = released;
                    if (!released)
                    {
                        InternalReleased += value;
                    }
                }

                if (releasedCopy)
                {
                    throw new InvalidOperationException("This view is being disposed");
                }
            }

            remove
            {
                lock (this)
                {
                    InternalReleased -= value;
                }
            }
        }
    }
}
