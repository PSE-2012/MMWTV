namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This is the hook up point for OQAT to start. From here
    /// all the intitialization work is done, i.e. VM_OQAT and
    /// first call to the PluginManager.
    /// </summary>
    public class OqatApp
	{

        static void Main(string[] args)
        {
        }


        /// <summary>
        /// A reference to the OQAT main ViewModel.
        /// </summary>
		private VM_Oqat vm_Oqat
		{
			get;
			set;
		}

        /// <summary>
        /// This is the only "not ViewModel" to listen
        /// for the toggleView event. Other components can 
        /// ask OqatApp for the current ViewState.
        /// </summary>
        /// <remarks>
        /// Currently this feature is no needed by any component,
        /// since every OqatIntern class can (and should) listen
        /// for the toggleView event. The only known reasen to have
        /// OqatApp to listen for toggleView is the shutdown process,
        /// i.e. ViewType = shutdown.
        /// </remarks>
        /// <param name="sender">Reference to the caller</param>
        /// <param name="e">Holds the new (global) ViewType.</param>
		public delegate void onToggleView(object sender, ViewTypeEventArgs e);


        /// <summary>
        /// Initializes the main ViewModel the <see cref="VM_Oqat"/>.
        /// 
        /// </summary>
		private void initOqat()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Constructor ist empty. If no interesting usecase is found
        /// at implementation time this will be deleted.
        /// </summary>
		public OqatApp()
		{
		}

        /// <summary>
        /// Initializes the <see cref="PluginManager"/>.
        /// </summary>
		private void initPluginManager()
		{
			throw new System.NotImplementedException();
		}

	}
}

