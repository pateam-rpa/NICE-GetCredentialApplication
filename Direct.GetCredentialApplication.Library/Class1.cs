using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Direct.Shared;
using Direct.Shared.Library;
using Direct.RTSMicroServicesSupport;
using Studio.Configuration;
using Direct.Interface;
using System.Configuration;
using Studio.Dom;
using log4net;


namespace Direct.GetCredentialApplication.Library
{

	[DirectDom("CCM")]
	public class CCM
	{
		private static readonly ILog logArchitect = LogManager.GetLogger(Loggers.LibraryObjects);
		[DirectDom("Get Application Id")]
		[DirectDomMethod("Get Application Id for {Application}")]
		[MethodDescription("Get Id for Credential Application")]
		public static int GetApplications(string applicationname)
		{
			try
			{
				CredentialManager manager = new CredentialManager();
				IUserInfoServices userInfoServices = (IUserInfoServices)((IDirectFramework)AppDomain.CurrentDomain.GetData("Framework"))["UserInfo"];
			
				if (!userInfoServices.IsLoggedToServer)
				{
					if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - User not connected to Server "); }
					return -1; 
				}

				// applications endpoint only available in designer config, to support replacing client endpoint
				Dictionary<int, ApplicationInfo> mapApplications = manager.GetApplicationsList(CCMConfigurator.ConfigManager.CCMCredentialsUrl.Replace("credentials/", "applications"));
				if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Looking for Application name"); }
				foreach (var item in mapApplications)
				{
					if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Adding Application: " + item.Value.name + " - " + item.Value.id); }
					if (item.Value.name == applicationname)
					{
						return item.Value.id;
					}
				}
		
				if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Application: " + applicationname + " not found"); }
				return -1;
			}
			catch (Exception e)
			{
				logArchitect.Error("Direct.GetCredentialApplication.Library - Get Credential Applications Exception", e);
				return -1;
			}
		}
	}
}
